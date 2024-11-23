//5 lane//

using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject[] terrainPrefabs; // Array of terrain prefabs
    public Transform player;            // Reference to the player object
    private float segmentLength = 30.0f; // Length of each terrain segment
    private int numSegmentsOnScreen = 6; // Number of segments visible at once
    public float laneDistance = 4.0f;   // Distance between lanes
    public float prefabWidth = 5.0f;    // Number of lanes per prefab

    public int currentPrefabIndex = 0;  // Tracks which prefab the player is on
    // private Dictionary<int, List<GameObject>> activePrefabs; // Tracks prefabs by their indices
    public Dictionary<int, List<GameObject>> activePrefabs;
    private Dictionary<int, float> prefabSpawnZ; // Tracks the furthest Z for each prefab

    void Start()
    {
        activePrefabs = new Dictionary<int, List<GameObject>>();
        prefabSpawnZ = new Dictionary<int, float>();

        // Initialize the five prefabs: current, two on the left, and two on the right
        for (int i = -2; i <= 2; i++)
        {
            prefabSpawnZ[currentPrefabIndex + i] = player.position.z; // Start spawning from player's Z position
            SpawnPrefab(currentPrefabIndex + i);
        }
    }

    void Update()
    {
        // Determine which prefab the player is currently on
        int newPrefabIndex = Mathf.FloorToInt((player.position.x + (laneDistance * prefabWidth / 2)) / (laneDistance * prefabWidth));

        // If the player has moved to a new prefab, adjust active prefabs
        if (newPrefabIndex != currentPrefabIndex)
        {
            UpdateActivePrefabs(newPrefabIndex);
        }

        // Continuously spawn forward for the active prefabs
        foreach (var prefabIndex in activePrefabs.Keys)
        {
            SpawnForward(prefabIndex);
        }

        // Cleanup old prefabs
        CleanupPrefabs();
    }

    void SpawnPrefab(int prefabIndex)
    {
        if (!activePrefabs.ContainsKey(prefabIndex))
        {
            activePrefabs[prefabIndex] = new List<GameObject>();
        }

        float spawnZ = prefabSpawnZ[prefabIndex];
        for (int i = 0; i < numSegmentsOnScreen; i++)
        {
            GameObject segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
            float spawnX = prefabIndex * laneDistance * prefabWidth;
            segment.transform.position = new Vector3(spawnX, 0, spawnZ);
            activePrefabs[prefabIndex].Add(segment);
            spawnZ += segmentLength;
        }

        prefabSpawnZ[prefabIndex] = spawnZ;
    }

    void SpawnForward(int prefabIndex)
    {
        float spawnZ = prefabSpawnZ[prefabIndex];
        while (spawnZ < player.position.z + segmentLength * numSegmentsOnScreen)
        {
            GameObject segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
            float spawnX = prefabIndex * laneDistance * prefabWidth;
            segment.transform.position = new Vector3(spawnX, 0, spawnZ);
            activePrefabs[prefabIndex].Add(segment);
            spawnZ += segmentLength;
        }

        prefabSpawnZ[prefabIndex] = spawnZ;
    }

    void UpdateActivePrefabs(int newPrefabIndex)
    {
        currentPrefabIndex = newPrefabIndex;

        // Ensure the current, two left, and two right prefabs exist
        for (int i = -2; i <= 2; i++)
        {
            int index = currentPrefabIndex + i;
            if (!activePrefabs.ContainsKey(index))
            {
                prefabSpawnZ[index] = player.position.z;
                SpawnPrefab(index);
            }
        }

        // Remove prefabs no longer relevant (those that are too far from the player)
        List<int> keysToRemove = new List<int>();
        foreach (var index in activePrefabs.Keys)
        {
            if (index < currentPrefabIndex - 2 || index > currentPrefabIndex + 2)
            {
                keysToRemove.Add(index);
            }
        }

        foreach (var index in keysToRemove)
        {
            CleanupPrefab(index);
        }
    }

    void CleanupPrefab(int prefabIndex)
    {
        if (activePrefabs.ContainsKey(prefabIndex))
        {
            foreach (GameObject segment in activePrefabs[prefabIndex])
            {
                Destroy(segment);
            }
            activePrefabs.Remove(prefabIndex);
            prefabSpawnZ.Remove(prefabIndex);
        }
    }

    void CleanupPrefabs()
    {
        foreach (var prefabIndex in activePrefabs.Keys)
        {
            List<GameObject> segmentsToRemove = new List<GameObject>();

            foreach (GameObject segment in activePrefabs[prefabIndex])
            {
                if (segment.transform.position.z < player.position.z - segmentLength * 2)
                {
                    segmentsToRemove.Add(segment);
                }
            }

            foreach (GameObject segment in segmentsToRemove)
            {
                activePrefabs[prefabIndex].Remove(segment);
                Destroy(segment);
            }
        }
    }
}


//3 lane//
// using System.Collections.Generic;
// using UnityEngine;

// public class TerrainGenerator : MonoBehaviour
// {
//     public GameObject[] terrainPrefabs; // Array of terrain prefabs
//     public Transform player;            // Reference to the player object
//     private float segmentLength = 30.0f; // Length of each terrain segment
//     private int numSegmentsOnScreen = 6; // Number of segments visible at once
//     public float laneDistance = 4.0f;   // Distance between lanes
//     public float prefabWidth = 5.0f;   // Number of lanes per prefab

//     public int currentPrefabIndex = 0; // Tracks which prefab the player is on
//     private Dictionary<int, List<GameObject>> activePrefabs; // Tracks prefabs by their indices
//     private Dictionary<int, float> prefabSpawnZ; // Tracks the furthest Z for each prefab

//     void Start()
//     {
//         activePrefabs = new Dictionary<int, List<GameObject>>();
//         prefabSpawnZ = new Dictionary<int, float>();

//         // Initialize the three primary prefabs: current, left, and right
//         for (int i = -1; i <= 1; i++)
//         {
//             prefabSpawnZ[currentPrefabIndex + i] = player.position.z; // Start spawning from player's Z position
//             SpawnPrefab(currentPrefabIndex + i);
//         }
//     }

//     // void Update()
//     // {
//     //     // Determine which prefab the player is on
//     //     int newPrefabIndex = Mathf.FloorToInt(player.position.x / (laneDistance * prefabWidth));

//     //     // If the player has moved to a new prefab, adjust active prefabs
//     //     if (newPrefabIndex != currentPrefabIndex)
//     //     {
//     //         UpdateActivePrefabs(newPrefabIndex);
//     //     }

//     //     // Continuously spawn forward for the active prefabs
//     //     foreach (var prefabIndex in activePrefabs.Keys)
//     //     {
//     //         SpawnForward(prefabIndex);
//     //     }

//     //     // Cleanup old prefabs
//     //     CleanupPrefabs();
//     // }
//     void Update()
// {
//     // Determine which prefab the player is currently on
//     int newPrefabIndex = Mathf.FloorToInt((player.position.x + (laneDistance * prefabWidth / 2)) / (laneDistance * prefabWidth));

//     // If the player has moved to a new prefab, adjust active prefabs
//     if (newPrefabIndex != currentPrefabIndex)
//     {
//         UpdateActivePrefabs(newPrefabIndex);
//     }

//     // Continuously spawn forward for the active prefabs
//     foreach (var prefabIndex in activePrefabs.Keys)
//     {
//         SpawnForward(prefabIndex);
//     }

//     // Cleanup old prefabs
//     CleanupPrefabs();
// }


//     void SpawnPrefab(int prefabIndex)
//     {
//         if (!activePrefabs.ContainsKey(prefabIndex))
//         {
//             activePrefabs[prefabIndex] = new List<GameObject>();
//         }

//         float spawnZ = prefabSpawnZ[prefabIndex];
//         for (int i = 0; i < numSegmentsOnScreen; i++)
//         {
//             GameObject segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
//             float spawnX = prefabIndex * laneDistance * prefabWidth;
//             segment.transform.position = new Vector3(spawnX, 0, spawnZ);
//             activePrefabs[prefabIndex].Add(segment);
//             spawnZ += segmentLength;
//         }

//         prefabSpawnZ[prefabIndex] = spawnZ;
//     }

//     void SpawnForward(int prefabIndex)
//     {
//         float spawnZ = prefabSpawnZ[prefabIndex];
//         while (spawnZ < player.position.z + segmentLength * numSegmentsOnScreen)
//         {
//             GameObject segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
//             float spawnX = prefabIndex * laneDistance * prefabWidth;
//             segment.transform.position = new Vector3(spawnX, 0, spawnZ);
//             activePrefabs[prefabIndex].Add(segment);
//             spawnZ += segmentLength;
//         }

//         prefabSpawnZ[prefabIndex] = spawnZ;
//     }

//     void UpdateActivePrefabs(int newPrefabIndex)
//     {
//         currentPrefabIndex = newPrefabIndex;

//         // Ensure the current, left, and right prefabs exist
//         for (int i = -1; i <= 1; i++)
//         {
//             int index = currentPrefabIndex + i;
//             if (!activePrefabs.ContainsKey(index))
//             {
//                 prefabSpawnZ[index] = player.position.z;
//                 SpawnPrefab(index);
//             }
//         }

//         // Remove prefabs no longer relevant
//         List<int> keysToRemove = new List<int>();
//         foreach (var index in activePrefabs.Keys)
//         {
//             if (index < currentPrefabIndex - 1 || index > currentPrefabIndex + 1)
//             {
//                 keysToRemove.Add(index);
//             }
//         }

//         foreach (var index in keysToRemove)
//         {
//             CleanupPrefab(index);
//         }
//     }

//     void CleanupPrefab(int prefabIndex)
//     {
//         if (activePrefabs.ContainsKey(prefabIndex))
//         {
//             foreach (GameObject segment in activePrefabs[prefabIndex])
//             {
//                 Destroy(segment);
//             }
//             activePrefabs.Remove(prefabIndex);
//             prefabSpawnZ.Remove(prefabIndex);
//         }
//     }

//     void CleanupPrefabs()
//     {
//         foreach (var prefabIndex in activePrefabs.Keys)
//         {
//             List<GameObject> segmentsToRemove = new List<GameObject>();

//             foreach (GameObject segment in activePrefabs[prefabIndex])
//             {
//                 if (segment.transform.position.z < player.position.z - segmentLength * 2)
//                 {
//                     segmentsToRemove.Add(segment);
//                 }
//             }

//             foreach (GameObject segment in segmentsToRemove)
//             {
//                 activePrefabs[prefabIndex].Remove(segment);
//                 Destroy(segment);
//             }
//         }
//     }
// }