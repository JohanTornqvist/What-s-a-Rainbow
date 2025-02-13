using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public float dashDistance = 5f; // Maximum dash distance
    public LayerMask ignoreLayers; // Layers to ignore during dash collisions
    public LayerMask dashThroughLayer; // Layers to dash through
    public bool canDash = true;
    public float dashCoolDown = 3f;
    public float dashTimer;

    public float dashThroughExtraDistance = 1f; // Extra distance to dash through an object
    public PlayerMovement playerMove;

    [Header("SphereCast Settings")]
    public float sphereRadius = 0.2f;
    public float sphereHeightOffset = 0.5f; // Vertical distance between the two SphereCasts
    public float zeroPointOffset = -0.5f; // Adjust the height of the base "zero point"

    void Start()
    {
        dashTimer = dashCoolDown;
    }

    private void Update()
    {
        if (playerMove.moveInput == new Vector2(-1, 0))
        {
            moveInput = Vector2.left;
        }
        if (playerMove.moveInput == new Vector2(1, 0))
        {
            moveInput = Vector2.right;
        }

        if (!canDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                canDash = true;
            }
        }
    }

    void OnDash(InputValue value)
    {
        if (playerMove.hasDash && canDash)
        {
            if (moveInput != Vector2.zero) // Ensure the player is moving before dashing
            {
                Vector2 dashDirection = moveInput.normalized;
                Vector2 basePosition = rb.position + new Vector2(0, zeroPointOffset); // Apply zero point offset

                // Positions for the upper and lower SphereCasts
                Vector2 upperPosition = basePosition + new Vector2(0, sphereHeightOffset);
                Vector2 lowerPosition = basePosition - new Vector2(0, sphereHeightOffset);

                // Collision mask
                int collisionMask = ~ignoreLayers.value;

                // Perform SphereCasts
                RaycastHit2D upperHit = Physics2D.CircleCast(upperPosition, sphereRadius, dashDirection, dashDistance, collisionMask);
                RaycastHit2D lowerHit = Physics2D.CircleCast(lowerPosition, sphereRadius, dashDirection, dashDistance, collisionMask);

                Vector2 dashTarget = rb.position + (dashDirection * dashDistance - new Vector2(0, 0.1f));

                // Handling collisions
                if (upperHit.collider != null && lowerHit.collider != null)
                {
                    Debug.Log($"Both SphereCasts hit: {upperHit.collider.gameObject.name}");
                    dashTarget = upperHit.point - (dashDirection * 1f);
                }
                else if (upperHit.collider != null)
                {
                    Debug.Log($"Upper SphereCast hit: {upperHit.collider.gameObject.name}");
                    dashTarget = upperHit.point - (dashDirection * 1f);
                }
                else if (lowerHit.collider != null)
                {
                    Debug.Log($"Lower SphereCast hit: {lowerHit.collider.gameObject.name}");
                    dashTarget = lowerHit.point - (dashDirection * 1f);
                }

                // Handle Dash Through layer
                if ((upperHit.collider != null && ((1 << upperHit.collider.gameObject.layer) & dashThroughLayer) != 0) ||
                    (lowerHit.collider != null && ((1 << lowerHit.collider.gameObject.layer) & dashThroughLayer) != 0))
                {
                    dashTarget += dashDirection * dashThroughExtraDistance;
                    Debug.Log("Dash through triggered!");
                }

                // Move player instantly
                rb.position = dashTarget;
                dashTimer = dashCoolDown;
                canDash = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            Gizmos.color = Color.blue;

            Vector2 basePosition = rb.position + new Vector2(0, zeroPointOffset);
            Vector2 upperPosition = basePosition + new Vector2(0, sphereHeightOffset);
            Vector2 lowerPosition = basePosition - new Vector2(0, sphereHeightOffset);

            Gizmos.DrawWireSphere(upperPosition + (moveInput.normalized * dashDistance), sphereRadius);
            Gizmos.DrawWireSphere(lowerPosition + (moveInput.normalized * dashDistance), sphereRadius);
        }
    }
}
