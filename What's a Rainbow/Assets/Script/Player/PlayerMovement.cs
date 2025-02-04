using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class playerMovement : MonoBehaviour
{
    Vector2 moveInput;
    public Rigidbody2D rb;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] GameObject player;
    public int playerState = 0;
    public Emotion emotionControler;

    [Header("Movement Settings:")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] float airTime = 1f;
    [SerializeField] float linDrag = 1f;
    [SerializeField] float angleDrag = 1f;
    [SerializeField] bool inAir = false;
    public float airTimeTimer = 0f;
    public float gravityTranstion = 1f;
    [SerializeField] float playerGravity = 10f;

    [Header("Ground Check:")]
    [SerializeField] ContactFilter2D groundFilter;

    [Header("Movment Toggles:")]
    public bool canMove = true;
    public bool canJump = false;

    [Header("State saves:")]
    public float normMoveSpeedSave;
    public float huntMoveSpeedSave = 0;
    public float sadMoveSpeedSave = 0;
    public Volume globalVolume;
    public VolumeProfile normVolume;
    public VolumeProfile huntVolume;
    public VolumeProfile sadVolume;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        airTimeTimer = airTime;
        normMoveSpeedSave = moveSpeed;
        rb.gravityScale = playerGravity;

        GameObject Emotion = GameObject.FindWithTag("EmotionControle");
        emotionControler = Emotion.GetComponent<Emotion>();
    }
    void OnMove(InputValue value)
    {
        if (canMove == true)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (canJump == true)
        {
            rb.velocity += new Vector2(0, jumpPower);
            rb.gravityScale = 0f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            inAir = true;
        }
    }

    private void FixedUpdate()
    {
        canJump = rb.IsTouching(groundFilter);
    }

    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }

        if(inAir == true)
        {
            airTimeTimer -= 0.1f * Time.deltaTime;
            airTimeTimer = Mathf.Clamp(airTimeTimer, 0f, airTime);
        }

        if (airTimeTimer <= 0f)
        {   
            inAir = false;
            airTimeTimer = airTime;
            rb.gravityScale = playerGravity;
            rb.drag = linDrag;
            rb.angularDrag = angleDrag;
        }

        switch (playerState)
        {
            case 0:
                normal();
                break;
            case 1:
                hunting();
                break;
            case 2:
                sad();
                break;
        }
    }

    void normal()
    {
        moveSpeed = normMoveSpeedSave;
    }

    void hunting()
    {
        moveSpeed = huntMoveSpeedSave;
    }

    void sad()
    {
        moveSpeed = sadMoveSpeedSave;
    }
}
