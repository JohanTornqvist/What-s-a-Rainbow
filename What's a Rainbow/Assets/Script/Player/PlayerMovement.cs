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

    [Header("Ground Check:")]
    [SerializeField] ContactFilter2D groundFilter;

    [Header("Movement Toggles:")]
    public bool canMove = true;
    public bool canJump = false;
    public bool hasDash = false;

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
    }

    void OnMove(InputValue value)
    {
        if (canMove)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    //void OnJump(InputValue value)
   // {
     //   if (canJump == true)
       // {
       //     rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //    ani.SetBool("isJumping", true);
        //}
        //}

    void OnJump(InputValue value)
    {
        if (canJump == true)
        {
            rb.velocity += new Vector2(0, jumpPower);
        }
    }

    void OnDash(InputValue value)
    {
        if (!hasDash) return; // Ensure dash is unlocked

        if (Mathf.Abs(moveInput.x) > 0.1f) // Avoid floating-point errors
        {
            rb.velocity = new Vector2(moveInput.x * 40, rb.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        // Constantly check if player is on the ground
        canJump = playerJumpBox.IsTouching(groundFilter);
        MovePlayer();
    }

    void MovePlayer()
    {
        if (!canMove) return;

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

    void Update()
    {
        ani.SetBool("isJumping", !canJump); // Automatically update jump animation

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

    void Normal() => moveSpeed = normMoveSpeedSave;
    void Hunting() => moveSpeed = huntMoveSpeedSave;
    void Sad() => moveSpeed = sadMoveSpeedSave;
}
