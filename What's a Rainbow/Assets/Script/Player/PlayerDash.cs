using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public Vector2 dashInput;
    public Vector2 moveInput;
    public float dashSpeed = 20f;

    public PlayerMovement playerMovement; // Reference to PlayerMovement

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>(); // Get PlayerMovement from the same GameObject
    }

    private void Update()
    {
        if (playerMovement != null)
        {
            moveInput = playerMovement.moveInput; // Get moveInput from PlayerMovement
        }
    }

    void OnDash(InputValue value)
    {
        rb.velocity = new Vector2(moveInput.x * dashSpeed, rb.velocity.y);
    }
}
