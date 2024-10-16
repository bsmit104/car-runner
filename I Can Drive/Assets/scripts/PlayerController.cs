using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to the Player GameObject
public class PlayerController : MonoBehaviour {
    public float speed = 10.0f;
    public float laneDistance = 2.5f; // Distance between lanes
    private int currentLane = 1; // 0: left, 1: middle, 2: right

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveLane(-1);  // Move Left
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveLane(1);   // Move Right
        }

        // Move the player forward at constant speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void MoveLane(int direction) {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
        Vector3 targetPosition = transform.position;
        targetPosition.x = currentLane * laneDistance;
        transform.position = targetPosition;
    }
}
