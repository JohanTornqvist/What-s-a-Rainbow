using UnityEngine;
using System.Collections;

public class TheHunter : MonoBehaviour
{
    [Header("Movement & Jump Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float forwardJumpForce = 10f;
    [SerializeField] private float chaseInterval = 0.1f;
    [SerializeField] private float jumpCooldown = 1.5f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float ledgeCheckDistance = 1.5f;

    [Header("Layers & Transforms")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ledgeCheck;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastJumpTime;
    private bool isChasing = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            StartCoroutine(ChasePlayer());
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
        while (isChasing)
        {
            if (playerTransform != null)
            {
                DecideAction();
            }
            yield return new WaitForSeconds(chaseInterval);
        }
    }

    void DecideAction()
    {
        if (!isGrounded) return; // Avoid decisions mid-air

        Vector2 diff = playerTransform.position - transform.position;

        if (diff.y > 1f && CanJump())
        {
            JumpUp();  // Jump up if the player is above
        }
        else if (diff.y < -1f && CanJump())
        {
            MoveToEdgeAndJumpOff(); // Jump down if the player is below
        }
        else if (IsApproachingLedge())
        {
            JumpForward(); // Jump forward over gaps
        }
        else
        {
            MoveTowardsPlayer(); // Chase the player normally
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    bool CanJump()
    {
        return Time.time > lastJumpTime + jumpCooldown && isGrounded;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        lastJumpTime = Time.time;
    }

    void JumpUp()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        lastJumpTime = Time.time;
    }

    void JumpForward()
    {
        rb.velocity = new Vector2(Mathf.Sign(playerTransform.position.x - transform.position.x) * forwardJumpForce, jumpForce * 0.8f);
        lastJumpTime = Time.time;
    }

    void MoveToEdgeAndJumpOff()
    {
        Vector2 checkPos = (Vector2)transform.position + new Vector2(Mathf.Sign(rb.velocity.x) * 1f, 0);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, groundCheckRadius, groundLayer);

        if (hit.collider == null)
        {
            Jump(); // Jump off the ledge if there's no ground ahead
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    bool IsApproachingLedge()
    {
        Vector2 checkPos = (Vector2)ledgeCheck.position + new Vector2(Mathf.Sign(rb.velocity.x) * ledgeCheckDistance, 0);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, groundCheckRadius, groundLayer);
        return hit.collider == null;
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Jump(); // Automatically jump when leaving a collider
        }
    }
}
