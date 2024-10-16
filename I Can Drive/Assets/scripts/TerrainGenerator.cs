using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    public GameObject[] terrainPrefabs; // Array of terrain segment prefabs
    public Transform player; // Reference to the player object
    private float spawnZ = 0.0f;  // Starting Z coordinate for spawning
    private float segmentLength = 30.0f;  // Length of each terrain segment
    private int numSegmentsOnScreen = 5;  // Number of segments to keep visible at a time

    void Start() {
        for (int i = 0; i < numSegmentsOnScreen; i++) {
            SpawnTerrain();
        }
    }

    void Update() {
        // If player passes a certain point, spawn new terrain
        if (player.position.z - segmentLength * 3 > spawnZ - numSegmentsOnScreen * segmentLength) {
            SpawnTerrain();
        }
    }

    void SpawnTerrain() {
        GameObject segment = Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)]);
        segment.transform.SetPositionAndRotation(new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnZ += segmentLength;
    }
}