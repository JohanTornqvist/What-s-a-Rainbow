using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementChaser : MonoBehaviour
{
    /* 
     * There was once a script whose purpose was to make the player move. This is not that script.
     * Instead, its purpose is to make the player's opponent move, and its name is Enemy.
     * TLDR Its the Enemys movement Script
     */

    [SerializeField] float moveSpeed = 17f;
    [SerializeField] float moveDuration = 3f; // Time before the enemy destroys itself

    Rigidbody2D rb;
    GameObject player;
    bool shouldMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (shouldMove && player != null)
        {
            // Move towards the player
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shouldMove = true;
            StopAllCoroutines(); // Stop any previous stop timer (prevents conflicts)
            StartCoroutine(DestroyAfterDelay()); // Start countdown to destroy
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(moveDuration);
        Destroy(gameObject); // Destroy the enemy after the delay
    }
}