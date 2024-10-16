using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10.0f;
    public float laneDistance = 4.0f;  // Adjust for the distance between lanes
    private int lane = 1; // 0: Left, 1: Center, 2: Right
    private bool isMoving = false;

    void Update() {
        // Move forward continuously
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Handle swipe/arrow movement between lanes
        if (Input.GetKeyDown(KeyCode.LeftArrow) && lane > 0 && !isMoving) {
            lane--;
            StartCoroutine(MoveToLane());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && lane < 2 && !isMoving) {
            lane++;
            StartCoroutine(MoveToLane());
        }
    }

    IEnumerator MoveToLane() {
        isMoving = true;
        Vector3 targetPosition = new Vector3(laneDistance * (lane - 1), transform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }
        isMoving = false;
    }
}