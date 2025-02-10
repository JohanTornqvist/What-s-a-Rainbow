using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public float dashDistance = 5f; // Maximum dash distance
    public LayerMask ignoreLayers; // Layers to ignore during dash collisions (multiple layers)
    public LayerMask dashThroughLayer; // Layer to dash through if hit

    public float dashThroughExtraDistance = 1f; // Extra distance to dash through an object if it's on Dash Through layer

    private PlayerMovement playerMovement; // Reference to PlayerMovement

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // Get PlayerMovement from the same GameObject
    }

    private void Update()
    {
        if (playerMovement != null)
        {
            moveInput = playerMovement.moveInput; // Get moveInput from PlayerMovement
        }
    }

    void OnDash(InputValue value)
    {
        if (moveInput != Vector2.zero) // Ensure the player is moving before dashing
        {
            Vector2 dashDirection = moveInput.normalized;
            Vector2 dashTarget = rb.position + (dashDirection * dashDistance);

            // Correctly create the collision mask to ignore multiple layers
            int collisionMask = ~ignoreLayers.value;  // Invert mask to ignore specified layers

            // Perform a Raycast, ignoring the specified layers
            RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDirection, dashDistance, collisionMask);

            if (hit.collider != null)
            {
                // Log if the raycast hits something
                Debug.Log("Raycast hit: " + hit.collider.name); // Logs the name of the hit object

                // If the object hit is on the Dash Through layer, dash through it by extending the dash distance
                if (((1 << hit.collider.gameObject.layer) & dashThroughLayer) != 0)
                {
                    dashTarget = hit.point + (dashDirection * dashThroughExtraDistance); // Extend dash beyond hit point
                    Debug.Log("Dash through triggered!");
                }
                // If the object is not in Dash Ignore or Dash Through layer, stop before it
                else if (((1 << hit.collider.gameObject.layer) & ignoreLayers) == 0 &&
                         ((1 << hit.collider.gameObject.layer) & dashThroughLayer) == 0)
                {
                    dashTarget = hit.point - (dashDirection * 1f); // Stop just before the obstacle
                    Debug.Log("Dash stopped before obstacle!");
                }
            }

            // Instantly move player
            rb.position = dashTarget;
        }
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rb.position, rb.position + (moveInput.normalized * dashDistance));
        }
    }
}
