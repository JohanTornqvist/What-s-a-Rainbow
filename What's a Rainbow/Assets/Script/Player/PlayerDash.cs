using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public Vector2 moveInput;
    public float dashDistance = 5f; // How far the dash moves

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
        if (moveInput != Vector2.zero) // Ensure the player is moving before dashing
        {
            Vector2 dashTarget = rb.position + (moveInput.normalized * dashDistance); // Calculate new position
            rb.MovePosition(dashTarget); // Instantly move the player
        }
    }
}
