using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;      // Array of car prefabs to spawn
    public Transform player;             // Reference to the player for position tracking
    public float spawnDistance = 100.0f; // Distance in front of the player to spawn cars
    public float carSpeed = 10.0f;       // Speed at which the cars move toward the player
    public float laneDistance = 4.0f;    // Distance between lanes
    public float carWidth = 2.0f;        // Approximate width of the car models
    public int totalLanes = 25;          // Total number of lanes (can be changed dynamically)
    public float minCarSpacing = 3.0f;  // Minimum spacing between cars in terms of car lengths
    
    private float lastCarZ = 0.0f; // Z position of the last spawned car
    private List<GameObject> activeCars = new List<GameObject>(); // List of active cars

    void Start()
    {
        // Initialize car spawning
        StartCoroutine(SpawnCars());
    }

    void Update()
    {
        // Move the active cars toward the player
        for (int i = activeCars.Count - 1; i >= 0; i--)
        {
            GameObject car = activeCars[i];

            // Move the car towards the player (negative Z direction)
            car.transform.Translate(Vector3.forward * -carSpeed * Time.deltaTime);

            // Remove cars that pass behind the player
            if (car.transform.position.z < player.position.z - 10)
            {
                Destroy(car);
                activeCars.RemoveAt(i); // Use RemoveAt to avoid modifying the list during iteration
            }
        }
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            // Ensure at least one open lane
            List<int> openLanes = new List<int>();
            for (int i = 0; i < totalLanes; i++)
            {
                openLanes.Add(i); // Add all lanes to the open lanes list
            }

            // Spawn 1 to 2 cars per interval
            int carsToSpawn = Random.Range(1, 3); // Spawn 1 to 2 cars at a time

            for (int i = 0; i < carsToSpawn; i++)
            {
                if (openLanes.Count == 0) break; // Stop if no lanes are open

                // Pick a random lane from the remaining open lanes
                int laneIndex = Random.Range(0, openLanes.Count);
                int lane = openLanes[laneIndex];
                openLanes.RemoveAt(laneIndex); // Remove the lane from available lanes

                // Calculate spawn position for the car based on the lane
                float laneOffset = lane * laneDistance - (laneDistance * (totalLanes / 2)); // Center the lanes around the player

                // Calculate the new Z position for the car
                float spawnZ = player.position.z + spawnDistance;
                // Make sure there is at least 3 cars' distance between the last car and the new car
                if (spawnZ - lastCarZ < minCarSpacing * carWidth)
                {
                    spawnZ = lastCarZ + minCarSpacing * carWidth; // Ensure proper spacing
                }

                // Spawn position in the X plane, adjusting the car's lane offset
                Vector3 spawnPos = new Vector3(laneOffset, 0, spawnZ);

                // Spawn a random car
                GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPos, Quaternion.identity);
                
                // Add the car to the active cars list
                activeCars.Add(car);

                // Update last car Z position
                lastCarZ = car.transform.position.z;
            }

            // Adjust the spawn interval to prevent too many cars
            yield return new WaitForSeconds(0.5f); // Reduced spawn frequency (adjust as necessary)
        }
    }
}











// goood ////
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CarSpawner : MonoBehaviour
// {
//     public GameObject[] carPrefabs; // Array of car prefabs to spawn
//     public Transform player;        // Reference to the player for position tracking
//     public float spawnDistance = 100.0f; // Distance in front of the player to spawn cars
//     public float carSpeed = 10.0f;       // Speed at which the cars move toward the player
//     public float laneDistance = 4.0f;    // Distance between lanes
//     public float carWidth = 2.0f;        // Approximate width of the car models
//     private float spawnZ = 0.0f;         // Z position for spawning
//     private int lanes = 5;               // Number of lanes (0: Left, 1: Center, 2: Right)

//     private List<GameObject> activeCars = new List<GameObject>(); // List of active cars

//     void Start()
//     {
//         // Initialize car spawning
//         StartCoroutine(SpawnCars());
//     }

//     void Update()
//     {
//         // Move the active cars toward the player
//         for (int i = activeCars.Count - 1; i >= 0; i--)
//         {
//             GameObject car = activeCars[i];
            
//             // Move the car towards the player (negative Z direction)
//             car.transform.Translate(Vector3.forward * -carSpeed * Time.deltaTime);

//             // Remove cars that pass behind the player
//             if (car.transform.position.z < player.position.z - 10)
//             {
//                 Destroy(car);
//                 activeCars.RemoveAt(i); // Use RemoveAt to avoid modifying the list during iteration
//             }
//         }
//     }

//     IEnumerator SpawnCars()
//     {
//         while (true)
//         {
//             // Ensure at least one open lane
//             List<int> openLanes = new List<int> { 0, 1, 2 }; // All lanes initially open

//             int carsToSpawn = Random.Range(1, 3); // Spawn 1 to 2 cars at a time
//             for (int i = 0; i < carsToSpawn; i++)
//             {
//                 if (openLanes.Count == 0) break; // Stop if no lanes are open

//                 // Pick a random lane from the remaining open lanes
//                 int laneIndex = Random.Range(0, openLanes.Count);
//                 int lane = openLanes[laneIndex];
//                 openLanes.RemoveAt(laneIndex); // Remove the lane from available lanes

//                 // Adjust the X offset here to shift spawning to the left
//                 float xOffset = -1.6f; // Adjust this value (negative to shift left)

//                 // Calculate spawn position, shifting it by xOffset to the left
//                 Vector3 spawnPos = new Vector3(laneDistance * (lane - 1) + xOffset, 0, player.position.z + spawnDistance);

//                 // Spawn a random car
//                 GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPos, Quaternion.identity);
                
//                 // Add the car to the active cars list
//                 activeCars.Add(car);
//             }

//             // Wait before spawning the next set of cars
//             yield return new WaitForSeconds(1.0f); // Adjust the spawn interval as needed
//         }
//     }
// }



























// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CarSpawner : MonoBehaviour
// {
//     public GameObject[] carPrefabs; // Array of car prefabs to spawn
//     public Transform player;        // Reference to the player for position tracking
//     public float spawnDistance = 100.0f; // Distance in front of the player to spawn cars
//     public float carSpeed = 10.0f;       // Speed at which the cars move toward the player
//     public float laneDistance = 4.0f;    // Distance between lanes
//     public float carWidth = 2.0f;        // Approximate width of the car models
//     private float spawnZ = 0.0f;         // Z position for spawning
//     private int lanes = 3;               // Number of lanes (0: Left, 1: Center, 2: Right)

//     private List<GameObject> activeCars = new List<GameObject>(); // List of active cars

//     void Start()
//     {
//         // Initialize car spawning
//         StartCoroutine(SpawnCars());
//     }

//     void Update()
//     {
//         // Move the active cars toward the player
//         foreach (var car in activeCars)
//         {
//             car.transform.Translate(Vector3.back * carSpeed * Time.deltaTime);

//             // Remove cars that pass behind the player
//             if (car.transform.position.z < player.position.z - 10)
//             {
//                 Destroy(car);
//                 activeCars.Remove(car);
//                 break;
//             }
//         }
//     }

//     IEnumerator SpawnCars()
//     {
//         while (true)
//         {
//             // Ensure at least one open lane
//             List<int> openLanes = new List<int> { 0, 1, 2 }; // All lanes initially open

//             int carsToSpawn = Random.Range(1, 3); // Spawn 1 to 2 cars at a time
//             for (int i = 0; i < carsToSpawn; i++)
//             {
//                 if (openLanes.Count == 0) break; // Stop if no lanes are open

//                 // Pick a random lane from the remaining open lanes
//                 int laneIndex = Random.Range(0, openLanes.Count);
//                 int lane = openLanes[laneIndex];
//                 openLanes.RemoveAt(laneIndex); // Remove the lane from available lanes

//                 // Adjust the X offset here to shift spawning to the left
//                 float xOffset = -2.0f; // Adjust this value (negative to shift left)

//                 // Calculate spawn position, shifting it by xOffset to the left
//                 Vector3 spawnPos = new Vector3(laneDistance * (lane - 1) + xOffset, 0, player.position.z + spawnDistance);

//                 // Spawn a random car
//                 GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPos, Quaternion.identity);
//                 activeCars.Add(car);
//             }

//             // Wait before spawning the next set of cars
//             yield return new WaitForSeconds(1.0f); // Adjust the spawn interval as needed
//         }
//     }
// }

















    // IEnumerator SpawnCars() {
    //     while (true) {
    //         // Ensure at least one open lane
    //         List<int> openLanes = new List<int> { 0, 1, 2 }; // All lanes initially open

    //         int carsToSpawn = Random.Range(1, 3); // Spawn 1 to 2 cars at a time
    //         for (int i = 0; i < carsToSpawn; i++) {
    //             if (openLanes.Count == 0) break; // Stop if no lanes are open

    //             // Pick a random lane from the remaining open lanes
    //             int laneIndex = Random.Range(0, openLanes.Count);
    //             int lane = openLanes[laneIndex];
    //             openLanes.RemoveAt(laneIndex); // Remove the lane from available lanes

    //             // Calculate spawn position
    //             Vector3 spawnPos = new Vector3(laneDistance * (lane - 1), 0, player.position.z + spawnDistance);

    //             // Spawn a random car
    //             GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPos, Quaternion.identity);
    //             activeCars.Add(car);
    //         }

    //         // Wait before spawning the next set of cars
    //         yield return new WaitForSeconds(1.0f); // Adjust the spawn interval as needed
    //     }
    // }