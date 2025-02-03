using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    Vector2 moveInput;
    public Rigidbody2D rb;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Transform player;
    [SerializeField] Transform ground;

    [Header("Movement Settings:")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpPower = 10f;

    [Header("Ground Check:")]
    [SerializeField] ContactFilter2D groundFilter;
    [SerializeField] ContactFilter2D platformFilter;
    [SerializeField] LayerMask groundLayers;

    [Header("Movment Toggles:")]
    public bool canMove = true;
    public bool canJump = false;


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        groundFilter.SetLayerMask(groundLayers);

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

    }
}
