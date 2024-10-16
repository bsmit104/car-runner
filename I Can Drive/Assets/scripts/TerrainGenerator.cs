using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    public GameObject[] terrainPrefabs; // Array of terrain segment prefabs
    public Transform player; // Reference to the player object
    private float spawnZ = 0.0f;  // Starting Z coordinate for spawning
    private float segmentLength = 30.0f;  // Length of each terrain segment
    private int numSegmentsOnScreen = 6;  // Max number of segments to keep visible at a time
    private List<GameObject> activeSegments; // List to track active terrain segments
    private Queue<GameObject> segmentPool; // Pool of inactive terrain segments

    void Start() {
        activeSegments = new List<GameObject>();
        segmentPool = new Queue<GameObject>();

        for (int i = 0; i < numSegmentsOnScreen; i++) {
            SpawnTerrain();
        }
    }

    void Update() {
        // If player passes a certain point, spawn new terrain
        if (player.position.z - segmentLength * 3 > spawnZ - numSegmentsOnScreen * segmentLength) {
            RecycleTerrain();
        }
    }

    // This method will reuse inactive terrain from the pool or instantiate new ones if the pool is empty
    void SpawnTerrain() {
        GameObject segment;

        // Check if there's a segment in the pool to reuse
        if (segmentPool.Count > 0) {
            segment = segmentPool.Dequeue(); // Reuse from pool
            // Change the prefab appearance by using a random prefab from the array
            ReplaceSegmentPrefab(segment);
        } else {
            // If pool is empty, instantiate a new one
            segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
        }

        // Set the position of the terrain segment
        segment.transform.SetPositionAndRotation(new Vector3(0, 0, spawnZ), Quaternion.identity);
        segment.SetActive(true); // Reactivate the segment
        activeSegments.Add(segment); // Add to the active list
        spawnZ += segmentLength;
    }

    // Recycle the oldest terrain and spawn a new one
    void RecycleTerrain() {
        // Get the first active terrain segment
        GameObject oldSegment = activeSegments[0];
        activeSegments.RemoveAt(0); // Remove from the active list
        oldSegment.SetActive(false); // Deactivate the segment
        segmentPool.Enqueue(oldSegment); // Add it back to the pool

        // Spawn a new segment
        SpawnTerrain();
    }

    // Replace the old segment with a new random prefab from the pool
    void ReplaceSegmentPrefab(GameObject segment) {
        // Pick a random prefab from the terrainPrefabs array
        GameObject newPrefab = terrainPrefabs[Random.Range(0, terrainPrefabs.Length)];
        MeshFilter meshFilter = segment.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = segment.GetComponent<MeshRenderer>();

        // Copy the mesh and materials from the random prefab to the reused segment
        if (meshFilter && meshRenderer) {
            MeshFilter newMeshFilter = newPrefab.GetComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = newPrefab.GetComponent<MeshRenderer>();

            if (newMeshFilter && newMeshRenderer) {
                meshFilter.sharedMesh = newMeshFilter.sharedMesh;
                meshRenderer.sharedMaterials = newMeshRenderer.sharedMaterials;
            }
        }
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TerrainGenerator : MonoBehaviour {
//     public GameObject[] terrainPrefabs; // Array of terrain segment prefabs
//     public Transform player; // Reference to the player object
//     private float spawnZ = 0.0f;  // Starting Z coordinate for spawning
//     private float segmentLength = 30.0f;  // Length of each terrain segment
//     private int numSegmentsOnScreen = 5;  // Number of segments to keep visible at a time

//     void Start() {
//         for (int i = 0; i < numSegmentsOnScreen; i++) {
//             SpawnTerrain();
//         }
//     }

//     void Update() {
//         // If player passes a certain point, spawn new terrain
//         if (player.position.z - segmentLength * 3 > spawnZ - numSegmentsOnScreen * segmentLength) {
//             SpawnTerrain();
//         }
//     }

//     void SpawnTerrain() {
//         GameObject segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
//         segment.transform.SetPositionAndRotation(new Vector3(0, 0, spawnZ), Quaternion.identity);
//         spawnZ += segmentLength;
//     }
// }