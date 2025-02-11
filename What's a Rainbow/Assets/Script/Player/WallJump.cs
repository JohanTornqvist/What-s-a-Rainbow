using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallJump : MonoBehaviour
{
    public Rigidbody2D rb;  // Rigidbody for the player
    public PlayerMovement playerMove;  // Player movement script for input
    public Vector2 direction;  // Direction the player is facing
    public float length = 0.5f;  // Length of the Raycast
    public LayerMask collisionMask;  // LayerMask to specify what layers the Raycast should collide with
    public bool onWall = false;
    public float wallJumpPower = 10f;
    public float sideJumpPower = 10f;
    public float gravitySave;
    public float wallGravity;

   void Start()
    {
        gravitySave = rb.gravityScale;
        wallGravity = rb.gravityScale / 2;
    }

 

    private void Update()
    {
        // Update the direction based on player input
        if (playerMove.moveInput == new Vector2(-1, 0))
        {
            direction = Vector2.left;
        }
        if (playerMove.moveInput == new Vector2(1, 0))
        {
            direction = Vector2.right;
        }

        // Perform a raycast in the direction the player is facing, with the selected LayerMask
        RaycastHit2D hit = Physics2D.Raycast(rb.position + new Vector2(0, 0.1f), direction, length, collisionMask);

        // Check if the raycast hit something
        if (hit.collider != null)
        {
            // Log the name of the object hit by the raycast
            Debug.Log("Raycast Hit: " + hit.collider.gameObject.name);
            playerMove.canMove = false;
            playerMove.jumpsLeft = 1;
            onWall = true;
            rb.gravityScale = wallGravity;

        }
        else 
        {
            playerMove.canMove = true;
            onWall = false;
            rb.gravityScale = gravitySave;
        }
    }

    // To visualize the raycast in the scene view (for debugging purposes)
    private void OnDrawGizmos()
    {
        // Set color for the Gizmos (Raycast visualizer)
        Gizmos.color = Color.red;

        // Draw the ray in the direction the player is facing
        Gizmos.DrawRay(rb.position, direction * length);
    }
    
    void OnJump()
    {
        if(playerMove.hasWallJumo == true)
        {
        if(onWall == true && playerMove.moveInput == new Vector2(1, 0))
        {
            rb.velocity = new Vector2(-sideJumpPower, wallJumpPower);
        }
        if (onWall == true && playerMove.moveInput == new Vector2(-1, 0))
        {
            rb.velocity = new Vector2(sideJumpPower, wallJumpPower);
        }
        }
       
    }
}
