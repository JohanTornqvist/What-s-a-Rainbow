using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D platform;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private float enemySpeed = 2f;

    private Vector2 platformPoint; // Target position for the boss to move towards
    private bool movingToPoint1 = true;

    void Start()
    {
        platformPoint = point1.position; // Start moving towards point1
    }

    void FixedUpdate()
    {
        // Move the platform towards the target point
        platform.MovePosition(Vector2.MoveTowards(platform.position, platformPoint, enemySpeed * Time.fixedDeltaTime));

        // Check if the platform is close enough to switch direction
        if (Vector2.Distance(platform.position, platformPoint) < 0.1f)
        {
            SwitchTarget(); // Change direction
        }
    }

    void SwitchTarget()
    {
        movingToPoint1 = !movingToPoint1; // Toggle direction
        platformPoint = movingToPoint1 ? point1.position : point2.position;
    }
}
