using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveInput;
    public Rigidbody2D rb;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Collider2D playerJumpBox;
    public int playerState = 0;
    public Emotion emotionControler;

    [Header("Movement Settings:")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] int jumpAmount = 1;
    public int jumpsLeft;
    [SerializeField] AudioClip jumpSfx;
    [SerializeField] AudioSource audioSource;

    [Header("Ground Check:")]
    [SerializeField] ContactFilter2D groundFilter;

    [Header("Movement Toggles:")]
    public bool canMove = true;
    public bool canJump = false;
    public bool hasDash = false;
    public bool hasDoubleJump = false;
    public bool hasWallJump = false;
    [SerializeField] bool noClip;

    [Header("State saves:")]
    public float normMoveSpeedSave;
    public float huntMoveSpeedSave = 0;
    public float sadMoveSpeedSave = 0;

    private Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normMoveSpeedSave = moveSpeed;
        ani = GetComponent<Animator>();

        GameObject emotionObject = GameObject.FindWithTag("EmotionControle");
        if (emotionObject != null)
        {
            emotionControler = emotionObject.GetComponent<Emotion>();
        }
        jumpsLeft = jumpAmount;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // Apply jump force
            audioSource.PlayOneShot(jumpSfx);
            jumpsLeft = hasDoubleJump ? 1 : 0; // Allow double jump if enabled
            ani.SetTrigger("isJumping"); // Trigger jump animation
        }
        else if (canJump == false && jumpsLeft > 0 && hasDoubleJump == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // Apply jump force for double jump
            audioSource.PlayOneShot(jumpSfx);
            jumpsLeft = 0; // No more jumps left after double jump
        }
    }

    private void FixedUpdate()
    {
        // Update the player movement and check if on the ground
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
            if (moveInput.x != 0)
            {
                ani.SetBool("isWalking", true);
                transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1); // Flip player sprite
            }
            else
            {
                ani.SetBool("isWalking", false);
            }
        }

        // Ground check logic to allow jumps
        canJump = playerJumpBox.IsTouching(groundFilter);
        if (canJump) jumpsLeft = jumpAmount; // Reset jumps if grounded
    }

    void Update()
    {
        ani.SetBool("isJumping", !canJump); // Update jump animation status

        // Handle state-based movement speeds
        switch (playerState)
        {
            case 0:
                Normal();
                break;
            case 1:
                Hunting();
                break;
            case 2:
                Sad();
                break;
        }


    }

    public void OnNoClip(InputValue value)
    {
        noClip = !noClip;

        if (noClip)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Disable physics
            rb.velocity = Vector2.zero; // Stop movement
            col.enabled = false; // Disable collisions
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics
            col.enabled = true; // Enable collisions
        }
    }

    void Normal() => moveSpeed = normMoveSpeedSave;
    void Hunting() => moveSpeed = huntMoveSpeedSave;
    void Sad() => moveSpeed = sadMoveSpeedSave;
}
