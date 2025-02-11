using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Jump_Pill : MonoBehaviour
{
    PlayerMovement playerMove;
    GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the correct tag
        {
            Destroy(gameObject);
            playerMove.hasWallJump = true;
        }
    }
}
