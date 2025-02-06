using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject key;
    public bool bossCanBreak = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && key == null || !collision.gameObject.CompareTag("Player") && bossCanBreak == true) // Ensure the Player has the correct tag
        {
            Destroy(gameObject);
        }
    }
}
