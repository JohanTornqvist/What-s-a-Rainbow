using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Ground Check:")]
    [SerializeField] ContactFilter2D groundFilter;

    [Header("Movement Toggles:")]
    public bool canMove = true;
    public bool canJump = false;
    public bool hasDash = false;
    public bool hasDubbleJump = false;

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
        if (canMove)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (canJump == true || canJump == false && jumpsLeft >= jumpAmount)
        {
            rb.velocity += new Vector2(0, jumpPower);
            if(canJump == false && jumpsLeft >= jumpAmount)
            {
                jumpsLeft -= 1;
            }
        }
    }

    private void FixedUpdate()
    {
        // Constantly check if player is on the ground
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        canJump = playerJumpBox.IsTouching(groundFilter);
        //Debug.Log(canJump);
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

        if (canJump == true) jumpsLeft = jumpAmount;
    }

    void Normal() => moveSpeed = normMoveSpeedSave;
    void Hunting() => moveSpeed = huntMoveSpeedSave;
    void Sad() => moveSpeed = sadMoveSpeedSave;
}
