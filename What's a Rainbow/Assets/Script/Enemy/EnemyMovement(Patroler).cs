using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    /* 
     * There was once a script whose purpose was to make the player move. This is not that script.
     * Instead, its purpose is to make the player's opponent move, and its name is Enemy.
     * TLDR Its the Enemy's movement Script
     */

    [SerializeField] float moveSpeed = 3f;
    Rigidbody2D rb;
    Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        StartCoroutine(IdleRoutine()); // Start the Idle animation routine
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipSprite();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipSprite();
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    IEnumerator IdleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(50f); 

            ani.SetBool("isIdle", true); 
            Debug.Log("Enemy is now idle!");

            yield return new WaitForSeconds(7f);

            ani.SetBool("isIdle", false); 
            Debug.Log("Enemy is no longer idle!");
        }
    }
}
