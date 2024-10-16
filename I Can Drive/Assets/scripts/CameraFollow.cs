using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to the Camera GameObject
public class CameraFollow : MonoBehaviour {
    public Transform player; // Drag the Player GameObject into this field in the inspector
    public Vector3 offset;   // Set the offset from the player in the inspector (e.g., x=0, y=3, z=-6)

    void LateUpdate() {
        // Camera follows the player's position + offset
        Vector3 newPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10); // Smooth follow
    }
}
