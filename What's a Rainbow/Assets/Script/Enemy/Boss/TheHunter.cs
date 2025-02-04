using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheHunter : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float forwardJumpForce = 10f;
    [SerializeField] float chaseInterval = 0.1f;
    [SerializeField] float jumpCooldown = 2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float maxJumpHeightCheck = 5f;
    [SerializeField] float ledgeCheckDistance = 1f;

    private PositionTracker playerTracker;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerTracker = player.GetComponent<PositionTracker>();

            if (playerTracker != null)
            {
                StartCoroutine(ChasePlayer());
            }
            else
            {
                Debug.LogWarning("Player does not have a PositionTracker component!");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject found with the 'Player' tag!");
        }
    }

    void Update()
    {
        CheckIfGrounded();
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

                if (Time.time > lastJumpTime + jumpCooldown && isGrounded && Mathf.Abs(rb.velocity.y) < 0.1f)
                {
                    if (IsLedgeAhead())
                    {
                        JumpForward();
                    }
                    else if (!IsCeilingAbove())
                    {
                        Jump();
                    }
                }
            }

            yield return new WaitForSeconds(chaseInterval);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        lastJumpTime = Time.time;
    }

    void JumpForward()
    {
        rb.velocity = new Vector2(moveSpeed * Mathf.Sign(rb.velocity.x), jumpForce);
        lastJumpTime = Time.time;
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    bool IsCeilingAbove()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, maxJumpHeightCheck, groundLayer);
        return hit.collider != null;
    }

    bool IsLedgeAhead()
    {
        Vector2 checkPosition = (Vector2)transform.position + new Vector2(Mathf.Sign(rb.velocity.x) * ledgeCheckDistance, 0);
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, groundCheckRadius, groundLayer);

        Debug.DrawRay(checkPosition, Vector2.down * groundCheckRadius, Color.red); // Debugging tool

        return hit.collider == null;
    }
}
