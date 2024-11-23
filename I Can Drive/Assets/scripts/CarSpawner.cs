using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;      // Array of car prefabs to spawn
    public Transform player;             // Reference to the player for position tracking
    public float spawnDistance = 75.0f;   // Distance in front of the player to spawn cars
    public float carSpeed = 15.0f;        // Speed at which the cars move toward the player
    public float laneDistance = 4.0f;     // Distance between lanes
    public float carWidth = 2.0f;         // Approximate width of the car models
    public int totalLanes = 41;           // Total number of lanes (41 lanes system for 20 to the left and right)
    public float minCarSpacing = 2.5f;    // Minimum spacing between cars in terms of car widths
    public float xOffset = -1.6f;         // Offset for shifting cars left or right

    private float lastCarZ = 0.0f;        // Z position of the last spawned car
    private List<GameObject> activeCars = new List<GameObject>(); // List of active cars

    private float leftLaneBoundary;       // Leftmost lane boundary relative to the player
    private float rightLaneBoundary;      // Rightmost lane boundary relative to the player

    void Start()
    {
        // Initialize the lane boundaries
        leftLaneBoundary = player.position.x - (20 * laneDistance);
        rightLaneBoundary = player.position.x + (20 * laneDistance);

        // Start spawning cars
        StartCoroutine(SpawnCars());
    }

    void Update()
    {
        // Move the active cars toward the player
        for (int i = activeCars.Count - 1; i >= 0; i--)
        {
            GameObject car = activeCars[i];

            // Move the car toward the player (negative Z direction)
            car.transform.Translate(Vector3.forward * -carSpeed * Time.deltaTime);

            // Remove cars that pass behind the player
            if (car.transform.position.z < player.position.z - 15)
            {
                Destroy(car);
                activeCars.RemoveAt(i); // Use RemoveAt to avoid modifying the list during iteration
            }
        }

        // Update lane boundaries based on the player's movement
        UpdateLaneBoundaries();
    }

    // Update the left and right lane boundaries based on the player's position
    void UpdateLaneBoundaries()
    {
        leftLaneBoundary = player.position.x - (20 * laneDistance);
        rightLaneBoundary = player.position.x + (20 * laneDistance);
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            // Define the spawn position for cars relative to the player
            float spawnZ = player.position.z + spawnDistance;

            // Determine the number of cars to spawn per cycle
            int carsToSpawn = Random.Range(3, 5); // Spawn 3-5 cars per interval

            for (int i = 0; i < carsToSpawn; i++)
            {
                // Select a random lane between left and right boundaries
                float randomLaneX = Random.Range(leftLaneBoundary, rightLaneBoundary);

                // Ensure we are on a valid lane by rounding to the nearest multiple of laneDistance
                float spawnX = Mathf.Round(randomLaneX / laneDistance) * laneDistance + xOffset;

                // Ensure there's at least `minCarSpacing` between the last car's Z position and the new car's Z position
                if (spawnZ - lastCarZ < minCarSpacing * carWidth)
                {
                    spawnZ = lastCarZ + minCarSpacing * carWidth; // Move the spawn point forward
                }

                // Instantiate a car at the calculated position
                GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], new Vector3(spawnX, 0, spawnZ), Quaternion.identity);

                // Add the car to the active cars list
                activeCars.Add(car);

                // Update last car Z position
                lastCarZ = car.transform.position.z;
            }

            // Reduce the spawn interval to make the game more challenging
            yield return new WaitForSeconds(0.35f); // Decrease spawn interval for a faster game pace
        }
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CarSpawner : MonoBehaviour
// {
//     public GameObject[] carPrefabs;      // Array of car prefabs to spawn
//     public Transform player;             // Reference to the player for position tracking
//     public float spawnDistance = 50.0f;   // Distance in front of the player to spawn cars (adjustable)
//     public float carSpeed = 15.0f;         // Speed at which the cars move toward the player
//     public float laneDistance = 4.0f;      // Distance between lanes
//     public float carWidth = 2.0f;           // Approximate width of the car models
//     public int totalLanes = 25;            // Total number of lanes
//     public float minCarSpacing = 2.5f;      // Minimum spacing between cars in terms of car widths
//     public float xOffset = -1.6f;           // Offset for shifting cars left or right

//     private float lastCarZ = 0.0f;         // Z position of the last spawned car
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

//             // Move the car toward the player (negative Z direction)
//             car.transform.Translate(Vector3.forward * -carSpeed * Time.deltaTime);

//             // Remove cars that pass behind the player
//             if (car.transform.position.z < player.position.z - 15)
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
//             // Determine the number of cars to spawn
//             int carsToSpawn = Random.Range(3, 5); // Spawn 3-5 cars per interval

//             for (int i = 0; i < carsToSpawn; i++)
//             {
//                 // Select a random lane
//                 int lane = Random.Range(0, totalLanes);

//                 // Calculate the spawn position for the car
//                 float laneOffset = (lane - (totalLanes / 2)) * laneDistance; // Spread lanes evenly
//                 float spawnZ = player.position.z + spawnDistance;
//                 float spawnX = laneOffset + xOffset;  // Apply the xOffset here to shift cars left or right

//                 // Ensure there's at least `minCarSpacing` between the last car's Z position and the new car's Z position
//                 if (spawnZ - lastCarZ < minCarSpacing * carWidth)
//                 {
//                     spawnZ = lastCarZ + minCarSpacing * carWidth; // Move the spawn point forward
//                 }

//                 // Instantiate a car
//                 GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], new Vector3(spawnX, 0, spawnZ), Quaternion.identity);
                
//                 // Add the car to the active cars list
//                 activeCars.Add(car);

//                 // Update last car Z position
//                 lastCarZ = car.transform.position.z;
//             }

//             // Reduce the spawn interval to make the game more challenging
//             yield return new WaitForSeconds(0.35f); // Decrease spawn interval
//         }
//     }
// }






// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CarSpawner : MonoBehaviour
// {
//     public GameObject[] carPrefabs;      // Array of car prefabs to spawn
//     public Transform player;             // Reference to the player for position tracking
//     public float spawnDistance = 75.0f;   // Distance in front of the player to spawn cars
//     public float carSpeed = 15.0f;         // Speed at which the cars move toward the player
//     public float laneDistance = 4.0f;      // Distance between lanes
//     public float carWidth = 2.0f;           // Approximate width of the car models
//     public int totalLanes = 25;            // Total number of lanes
//     public float minCarSpacing = 2.5f;      // Minimum spacing between cars in terms of car widths

//     private float lastCarZ = 0.0f;         // Z position of the last spawned car
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

//             // Move the car toward the player (negative Z direction)
//             car.transform.Translate(Vector3.forward * -carSpeed * Time.deltaTime);

//             // Remove cars that pass behind the player
//             if (car.transform.position.z < player.position.z - 15)
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
//             // Determine the number of cars to spawn
//             int carsToSpawn = Random.Range(3, 5); // Spawn 3-5 cars per interval

//             for (int i = 0; i < carsToSpawn; i++)
//             {
//                 // Select a random lane
//                 int lane = Random.Range(0, totalLanes);

//                 // Calculate the spawn position for the car
//                 float laneOffset = (lane - (totalLanes / 2)) * laneDistance; // Spread lanes evenly
//                 float spawnZ = player.position.z + spawnDistance;
//                 float spawnX = laneOffset;

//                 // Ensure there's at least `minCarSpacing` between the last car's Z position and the new car's Z position
//                 if (spawnZ - lastCarZ < minCarSpacing * carWidth)
//                 {
//                     spawnZ = lastCarZ + minCarSpacing * carWidth; // Move the spawn point forward
//                 }

//                 // Instantiate a car
//                 GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], new Vector3(spawnX, 0, spawnZ), Quaternion.identity);
                
//                 // Add the car to the active cars list
//                 activeCars.Add(car);

//                 // Update last car Z position
//                 lastCarZ = car.transform.position.z;
//             }

//             // Reduce the spawn interval to make the game more challenging
//             yield return new WaitForSeconds(0.35f); // Decrease spawn interval
//         }
//     }
// }











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