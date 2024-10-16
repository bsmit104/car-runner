using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float forwardSpeed = 10.0f;      // Speed moving forward
    public float laneSwitchSpeed = 10.0f;   // Speed of lane switching
    public float laneDistance = 4.0f;       // Distance between lanes
    private int targetLane = 1;             // 0: Left, 1: Center, 2: Right
    private bool isChangingLane = false;    // Flag to prevent multiple lane changes at once

    void Update() {
        // Move forward continuously
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Handle lane switching input
        if (Input.GetKeyDown(KeyCode.LeftArrow) && targetLane > 0 && !isChangingLane) {
            Debug.Log("Left Arrow Pressed");
            targetLane--;  // Move to the left lane
            ChangeLane();  // Call the change lane function directly
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow) && targetLane < 2 && !isChangingLane) {
            Debug.Log("Right Arrow Pressed");
            targetLane++;  // Move to the right lane
            ChangeLane();  // Call the change lane function directly
        }

        // Debug output for the current lane
        Debug.Log($"Current Lane: {targetLane}, isChangingLane: {isChangingLane}");
    }

    // Change lanes instantly for testing
    void ChangeLane() {
        isChangingLane = true;

        // Calculate the target lane position
        Vector3 targetPosition = new Vector3(laneDistance * (targetLane - 1), transform.position.y, transform.position.z);

        // Snap to final lane position instantly for testing
        transform.position = targetPosition;

        // Debug output for position confirmation
        Debug.Log($"Moved to Lane: {targetLane}, New Position: {transform.position}");

        isChangingLane = false; // Allow another lane change
    }
}




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour {
//     public float speed = 10.0f;
//     public float laneDistance = 4.0f;  // Adjust for the distance between lanes
//     private int lane = 1; // 0: Left, 1: Center, 2: Right
//     private bool isMoving = false;

//     void Update() {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * speed * Time.deltaTime);

//         // Handle swipe/arrow movement between lanes
//         if (Input.GetKeyDown(KeyCode.LeftArrow) && lane > 0 && !isMoving) {
//             lane--;
//             StartCoroutine(MoveToLane());
//         }
//         if (Input.GetKeyDown(KeyCode.RightArrow) && lane < 2 && !isMoving) {
//             lane++;
//             StartCoroutine(MoveToLane());
//         }
//     }

//     IEnumerator MoveToLane() {
//         isMoving = true;
//         Vector3 targetPosition = new Vector3(laneDistance * (lane - 1), transform.position.y, transform.position.z);
//         while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
//             transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
//             yield return null;
//         }
//         isMoving = false;
//     }
// }