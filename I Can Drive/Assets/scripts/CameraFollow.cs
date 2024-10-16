using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;  // Reference to the player GameObject
    public Vector3 offset;    // Offset of the camera from the player (adjust in Inspector)
    public float followSpeed = 5.0f;  // Smooth follow speed (adjust as needed)

    void LateUpdate() {
        // Desired position for the camera
        Vector3 targetPosition = player.position + offset;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Attach this script to the Camera GameObject
// public class CameraFollow : MonoBehaviour {
//     public Transform player; // Drag the Player GameObject into this field in the inspector
//     public Vector3 offset;   // Set the offset from the player in the inspector (e.g., x=0, y=3, z=-6)

//     void LateUpdate() {
//         // Camera follows the player's position + offset
//         Vector3 newPosition = player.position + offset;
//         transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10); // Smooth follow
//     }
// }
