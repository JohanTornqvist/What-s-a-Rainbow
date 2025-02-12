using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private float stillTimeThreshold = 3f;

    [Header("Layers & Transforms")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ledgeCheck;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastJumpTime;
    private bool isChasing = true;
    private Animator ani;

    private Vector3 lastPosition;
    private float stillTime = 0f;
    private bool wasGrounded; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>(); 

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            StartCoroutine(ChasePlayer());
        }
        else
        {
            Debug.LogWarning("WHAT do you MEAN There are NO PREY?!!!");
        }

        lastPosition = transform.position;
    }

    void Update()
    {
        CheckIfGrounded();
        CheckIfStill();

        // Update the "isHunting" parameter based on the isChasing bool.
        ani.SetBool("isHunting", isChasing);

        // Detect when the hunter leaves the ground to start the jump animation.
        if (wasGrounded && !isGrounded)
        {
            ani.SetBool("isJumpingHunter", true);
        }

        // Stop the jump animation when the hunter lands.
        if (!wasGrounded && isGrounded)
        {
            ani.SetBool("isJumpingHunter", false);
        }

        wasGrounded = isGrounded; 
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
        if (!isGrounded) return;

        // Always prioritize ledge jumps first
        if (IsApproachingLedge())
        {
            JumpForward();
            return;
        }

        Vector2 diff = playerTransform.position - transform.position;

        // If the player is above
        if (diff.y > 1f)
        {
            if (Mathf.Abs(diff.x) < 0.5f) // Only stop if X positions are nearly the same
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                MoveTowardsPlayer();
            }
            return;
        }

        // If the player is below
        if (diff.y < -1f)
        {
            if (CanJump())
            {
                Jump();
            }

            if (Mathf.Abs(diff.x) < 0.5f) // Only stop if X positions are nearly the same
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                MoveTowardsPlayer();
            }
            return;
        }

        MoveTowardsPlayer();
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
        stillTime = 0f; 
    }

    void JumpForward()
    {
        rb.velocity = new Vector2(Mathf.Sign(playerTransform.position.x - transform.position.x) * forwardJumpForce, jumpForce * 0.8f);
        lastJumpTime = Time.time;
        stillTime = 0f;
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

    void CheckIfStill()
    {
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stillTime += Time.deltaTime;

            if (stillTime >= stillTimeThreshold)
            {
                Jump();
            }
        }
        else
        {
            stillTime = 0f;
        }

        lastPosition = transform.position;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Jump();
        }
    }
}