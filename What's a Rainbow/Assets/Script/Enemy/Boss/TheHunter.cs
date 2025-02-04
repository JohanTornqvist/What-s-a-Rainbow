using UnityEngine;
using System.Collections;

public class TheHunter : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float forwardJumpForce = 10f;
    [SerializeField] float chaseInterval = 0.1f;
    [SerializeField] float jumpCooldown = 2f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

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
                MoveTowardsPlayer();

                // Only allow jumping if the cooldown is passed and we are grounded
                if (Time.time > lastJumpTime + jumpCooldown && isGrounded && Mathf.Abs(rb.velocity.y) < 0.1f)
                {
                    PerformJumpBehavior();
                }
            }

            yield return new WaitForSeconds(chaseInterval);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void PerformJumpBehavior()
    {
        // If we're not grounded and moving, initiate jump
        if (!isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        // Apply both forward and upward forces at the same time
        float jumpDirection = Mathf.Sign(rb.velocity.x) * forwardJumpForce;
        rb.velocity = new Vector2(jumpDirection, jumpForce); // Combine forward and upward movement
        lastJumpTime = Time.time;
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Automatically jump forward and up when leaving a collider
    private void OnTriggerExit2D(Collider2D other)
    {
        // Automatically jump forward and up when exiting a trigger collider
        Jump();
    }
}
