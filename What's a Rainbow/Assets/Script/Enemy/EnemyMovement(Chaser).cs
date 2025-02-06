using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementChaser : MonoBehaviour
{
    [SerializeField] float moveSpeed = 17f;
    [SerializeField] float moveDuration = 2f; // Time before the enemy destroys itself

    Rigidbody2D rb;
    GameObject player;
    bool shouldMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        rb.gravityScale = 1; // Ensure gravity is enabled
    }

    void Update()
    {
        if (shouldMove && player != null)
        {
            // Move towards the player using velocity
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y); // Keep gravity effect
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
