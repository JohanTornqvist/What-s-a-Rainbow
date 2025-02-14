using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallJump : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerMovement playerMove;

    [Header("Wall Detection")]
    public float sphereRadius = 0.2f;  // Radius of the SphereCasts
    public float checkDistance = 0.5f; // How far to check for walls
    public LayerMask wallLayers;       // Which layers count as a wall
    public float detectorHeight = 0.5f; // Vertical spacing between SphereCasts
    public float verticalOffset = -0.5f; // Moves the detection points downward

    [Header("Wall Jump Settings")]
    public float wallJumpPower = 10f;
    public float sideJumpPower = 10f;
    public float gravityNormal;
    public float wallGravity;

    private bool isTouchingWall = false;
    private Vector2 facingDirection = Vector2.right;
    [SerializeField] AudioClip jumpSfx;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        gravityNormal = rb.gravityScale;
        wallGravity = rb.gravityScale / 2;
    }

    private void Update()
    {
        // Update the facing direction based on input
        if (playerMove.moveInput.x < 0)
        {
            facingDirection = Vector2.left;
        }
        else if (playerMove.moveInput.x > 0)
        {
            facingDirection = Vector2.right;
        }

        // Adjust base position downward by verticalOffset
        Vector2 basePosition = rb.position + new Vector2(0, verticalOffset);

        // Define the positions of the two SphereCasts (upper and lower)
        Vector2 upperPosition = basePosition + new Vector2(0, detectorHeight);
        Vector2 lowerPosition = basePosition - new Vector2(0, detectorHeight);

        // Perform two SphereCasts to detect walls
        RaycastHit2D upperHit = Physics2D.CircleCast(upperPosition, sphereRadius, facingDirection, checkDistance, wallLayers);
        RaycastHit2D lowerHit = Physics2D.CircleCast(lowerPosition, sphereRadius, facingDirection, checkDistance, wallLayers);

        // Logging system
        if (upperHit.collider != null && lowerHit.collider != null)
        {
            Debug.Log($"Both upper and lower hits detected on: {upperHit.collider.gameObject.name}");
        }
        else if (upperHit.collider != null)
        {
            Debug.Log($"Upper hit detected on: {upperHit.collider.gameObject.name}");
        }
        else if (lowerHit.collider != null)
        {
            Debug.Log($"Lower hit detected on: {lowerHit.collider.gameObject.name}");
        }

        // If either of the two SphereCasts detects a wall, set wall jump variables
        if (upperHit.collider != null || lowerHit.collider != null)
        {
            playerMove.canMove = false;
            playerMove.jumpsLeft = 1;
            isTouchingWall = true;
            rb.gravityScale = wallGravity;
        }
        else
        {
            playerMove.canMove = true;
            isTouchingWall = false;
            rb.gravityScale = gravityNormal;
        }
    }

    void OnJump()
    {
        if (playerMove.hasWallJump && isTouchingWall)
        {
            float jumpDirection = facingDirection.x > 0 ? -1 : 1; // Flip jump direction away from the wall
            audioSource.PlayOneShot(jumpSfx);
            rb.velocity = new Vector2(jumpDirection * sideJumpPower, wallJumpPower);
        }
    }

    // Visualize the SphereCasts in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = isTouchingWall ? Color.green : Color.red;

        // Adjust base position downward by verticalOffset
        Vector2 basePosition = rb.position + new Vector2(0, verticalOffset);

        // Draw spheres at the detection points
        Vector2 upperPosition = basePosition + new Vector2(0, detectorHeight);
        Vector2 lowerPosition = basePosition - new Vector2(0, detectorHeight);

        Gizmos.DrawWireSphere(upperPosition + (Vector2)(facingDirection * checkDistance), sphereRadius);
        Gizmos.DrawWireSphere(lowerPosition + (Vector2)(facingDirection * checkDistance), sphereRadius);
    }
}
