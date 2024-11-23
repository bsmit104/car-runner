using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10.0f;  // Speed moving forward
    public float laneDistance = 4.0f;  // Distance between lanes
    private int targetLane = 0;        // Start in the center lane

    private float lastLaneChangeTime = 0.0f; // Track last lane change time
    private float laneChangeSpeed = 0.0f;   // Speed at which lanes are being changed
    private float rotationSpeed = 10.0f;     // Rotation speed for car tilt
    private float maxRotationAngle = 45.0f;  // Maximum rotation angle for more prominent effect
    private float maxRotationSpeed = 50.0f;  // Max speed of rotation when changing lanes fast
    private float rotationAcceleration = 5.0f; // How quickly rotation speeds up when changing lanes

    private bool isChangingLeft = false; // Track if the player is switching left
    private bool isChangingRight = false; // Track if the player is switching right

    private float currentRotation = 0.0f; // Current rotation of the car (in degrees)

    void Update()
    {
        // Move forward continuously
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Track lane change speed based on time between changes
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            float timeSinceLastChange = Time.time - lastLaneChangeTime;
            lastLaneChangeTime = Time.time;

            // If lane change is too fast, increase lane change speed
            laneChangeSpeed = timeSinceLastChange < 0.2f ? Mathf.Min(laneChangeSpeed + 2.0f, maxRotationSpeed) : 0.0f;
        }

        // Handle lane switching input (no bounds check for infinite lanes)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetLane--; // Move to the left lane
            isChangingLeft = true;
            isChangingRight = false; // Reset right change tracking
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetLane++; // Move to the right lane
            isChangingRight = true;
            isChangingLeft = false; // Reset left change tracking
        }

        // Smooth transition between lanes
        Vector3 targetPosition = new Vector3(laneDistance * targetLane, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime); // Adjust speed as needed

        // Apply rotation based on lane change speed and direction
        ApplyRotationBasedOnLaneChange();
    }

    void ApplyRotationBasedOnLaneChange()
    {
        // If the lane change is too fast, apply rotation
        if (laneChangeSpeed > 0.0f)
        {
            float targetRotation = 0.0f;

            // Rotate clockwise when moving left too fast
            if (isChangingLeft)
            {
                targetRotation = Mathf.Clamp(currentRotation + laneChangeSpeed * 3.0f, 0, maxRotationAngle); // Clockwise rotation
            }
            // Rotate counterclockwise when moving right too fast
            else if (isChangingRight)
            {
                targetRotation = Mathf.Clamp(currentRotation - laneChangeSpeed * 3.0f, -maxRotationAngle, 0); // Counter-clockwise rotation
            }

            // Apply rotation with dynamic speed based on lane change speed
            rotationSpeed = Mathf.Lerp(rotationSpeed, maxRotationSpeed, laneChangeSpeed * Time.deltaTime);
            currentRotation = Mathf.MoveTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0); // Apply smooth rotation
        }
        else
        {
            // If lane change stops, stabilize rotation smoothly with faster straightening
            rotationSpeed = Mathf.Lerp(rotationSpeed, maxRotationSpeed, 1.0f * Time.deltaTime); // Increase speed for straightening
            currentRotation = Mathf.MoveTowards(currentRotation, 0, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }
}


// better rotation
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float forwardSpeed = 10.0f;  // Speed moving forward
//     public float laneDistance = 4.0f;  // Distance between lanes
//     private int targetLane = 0;        // Start in the center lane

//     private float lastLaneChangeTime = 0.0f; // Track last lane change time
//     private float laneChangeSpeed = 0.0f;   // Speed at which lanes are being changed
//     private float rotationSpeed = 10.0f;     // Rotation speed for car tilt
//     private float maxRotationAngle = 45.0f;  // Maximum rotation angle for more prominent effect

//     private bool isChangingLeft = false; // Track if the player is switching left
//     private bool isChangingRight = false; // Track if the player is switching right

//     void Update()
//     {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

//         // Track lane change speed based on time between changes
//         if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             float timeSinceLastChange = Time.time - lastLaneChangeTime;
//             lastLaneChangeTime = Time.time;

//             // If lane change is too fast, increase lane change speed
//             laneChangeSpeed = timeSinceLastChange < 0.2f ? Mathf.Min(laneChangeSpeed + 2.0f, 20.0f) : 0.0f; // Increased speed factor
//         }

//         // Handle lane switching input (no bounds check for infinite lanes)
//         if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             targetLane--; // Move to the left lane
//             isChangingLeft = true;
//             isChangingRight = false; // Reset right change tracking
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             targetLane++; // Move to the right lane
//             isChangingRight = true;
//             isChangingLeft = false; // Reset left change tracking
//         }

//         // Smooth transition between lanes
//         Vector3 targetPosition = new Vector3(laneDistance * targetLane, transform.position.y, transform.position.z);
//         transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime); // Adjust speed as needed

//         // Apply rotation based on lane change speed and direction
//         ApplyRotationBasedOnLaneChange();
//     }

//     void ApplyRotationBasedOnLaneChange()
//     {
//         // If the lane change is too fast, apply rotation
//         if (laneChangeSpeed > 0.0f)
//         {
//             float rotationAmount = 0.0f;

//             // Rotate clockwise when moving left too fast
//             if (isChangingLeft)
//             {
//                 rotationAmount = Mathf.Clamp(laneChangeSpeed * 3.0f, 0, maxRotationAngle); // Clockwise rotation
//             }
//             // Rotate counterclockwise when moving right too fast
//             else if (isChangingRight)
//             {
//                 rotationAmount = Mathf.Clamp(-laneChangeSpeed * 3.0f, -maxRotationAngle, 0); // Counter-clockwise rotation
//             }

//             transform.rotation = Quaternion.Euler(0, rotationAmount, 0); // Apply rotation based on direction
//         }
//         else
//         {
//             // If lane change stops, stabilize rotation smoothly
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
//         }
//     }
// }




//slight rotation
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float forwardSpeed = 10.0f;  // Speed moving forward
//     public float laneDistance = 4.0f;  // Distance between lanes
//     private int targetLane = 0;        // Start in the center lane

//     private float lastLaneChangeTime = 0.0f; // Track last lane change time
//     private float laneChangeSpeed = 0.0f;   // Speed at which lanes are being changed
//     private float rotationSpeed = 5.0f;     // Rotation speed for car tilt
//     private float maxRotationAngle = 15.0f; // Maximum rotation angle

//     void Update()
//     {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

//         // Track lane change speed based on time between changes
//         if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             float timeSinceLastChange = Time.time - lastLaneChangeTime;
//             lastLaneChangeTime = Time.time;

//             // If lane change is too fast, increase lane change speed
//             laneChangeSpeed = timeSinceLastChange < 0.2f ? Mathf.Min(laneChangeSpeed + 1.0f, 10.0f) : 0.0f;
//         }

//         // Handle lane switching input (no bounds check for infinite lanes)
//         if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             targetLane--; // Move to the left lane
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             targetLane++; // Move to the right lane
//         }

//         // Smooth transition between lanes
//         Vector3 targetPosition = new Vector3(laneDistance * targetLane, transform.position.y, transform.position.z);
//         transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime); // Adjust speed as needed

//         // Apply rotation based on lane change speed and direction
//         ApplyRotationBasedOnLaneChange();
//     }

//     void ApplyRotationBasedOnLaneChange()
//     {
//         // If the lane change is too fast, apply rotation
//         if (laneChangeSpeed > 0.0f)
//         {
//             float rotationDirection = (targetLane == 0) ? 0 : (targetLane < 0 ? 1 : -1);
//             float rotationAmount = Mathf.Min(laneChangeSpeed * rotationDirection, maxRotationAngle);
//             transform.rotation = Quaternion.Euler(0, rotationAmount, 0); // Apply slight rotation
//         }
//         else
//         {
//             // If lane change stops, stabilize rotation smoothly
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
//         }
//     }
// }


// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float forwardSpeed = 10.0f;  // Speed moving forward
//     public float laneDistance = 4.0f;  // Distance between lanes
//     private int targetLane = 0;        // Start in the center lane

//     void Update()
//     {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

//         // Handle lane switching input (no bounds check for infinite lanes)
//         if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             targetLane--; // Move to the left lane
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             targetLane++; // Move to the right lane
//         }

//         // Smooth transition between lanes
//         Vector3 targetPosition = new Vector3(laneDistance * targetLane, transform.position.y, transform.position.z);
//         transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime); // Adjust speed as needed
//     }
// }











//slow move
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float forwardSpeed = 10.0f;  // Speed moving forward
//     public float laneDistance = 4.0f;  // Distance between lanes
//     private int targetLane = 0;        // Start in the center lane

//     void Update()
//     {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

//         // Handle lane switching input
//         if (Input.GetKeyDown(KeyCode.LeftArrow) && targetLane > -3) // Adjust lane bounds
//         {
//             targetLane--;
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow) && targetLane < 3)
//         {
//             targetLane++;
//         }

//         // Smooth transition between lanes
//         Vector3 targetPosition = new Vector3(laneDistance * targetLane, transform.position.y, transform.position.z);
//         transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime); // Adjust speed as needed
//     }

// }
















//same thing cleaner?///
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour {
//     public float forwardSpeed = 10.0f;  // Speed moving forward
//     public float laneDistance = 4.0f;  // Distance between lanes
//     private int targetLane = 0;        // Start in the center lane

//     void Update() {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

//         // Handle lane switching input
//         if (Input.GetKeyDown(KeyCode.LeftArrow)) {
//             targetLane--; // Move to the left lane
//         } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
//             targetLane++; // Move to the right lane
//         }

//         // Update position
//         Vector3 targetPosition = new Vector3(laneDistance * targetLane, transform.position.y, transform.position.z);
//         transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);
//     }
// }














//good//

// using System.Collections;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour {
//     public float forwardSpeed = 10.0f;      // Speed moving forward
//     public float laneSwitchSpeed = 10.0f;   // Speed of lane switching
//     public float laneDistance = 4.0f;       // Distance between lanes
//     private int targetLane = 1;             // 0: Left, 1: Center, 2: Right
//     private bool isChangingLane = false;    // Flag to prevent multiple lane changes at once

//     void Update() {
//         // Move forward continuously
//         transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

//         // Handle lane switching input
//         if (Input.GetKeyDown(KeyCode.LeftArrow) && targetLane > 0 && !isChangingLane) {
//             Debug.Log("Left Arrow Pressed");
//             targetLane--;  // Move to the left lane
//             ChangeLane();  // Call the change lane function directly
//         } 
//         else if (Input.GetKeyDown(KeyCode.RightArrow) && targetLane < 2 && !isChangingLane) {
//             Debug.Log("Right Arrow Pressed");
//             targetLane++;  // Move to the right lane
//             ChangeLane();  // Call the change lane function directly
//         }

//         // Debug output for the current lane
//         Debug.Log($"Current Lane: {targetLane}, isChangingLane: {isChangingLane}");
//     }

//     // Change lanes instantly for testing
//     void ChangeLane() {
//         isChangingLane = true;

//         // Calculate the target lane position
//         Vector3 targetPosition = new Vector3(laneDistance * (targetLane - 1), transform.position.y, transform.position.z);

//         // Snap to final lane position instantly for testing
//         transform.position = targetPosition;

//         // Debug output for position confirmation
//         Debug.Log($"Moved to Lane: {targetLane}, New Position: {transform.position}");

//         isChangingLane = false; // Allow another lane change
//     }
// }
























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