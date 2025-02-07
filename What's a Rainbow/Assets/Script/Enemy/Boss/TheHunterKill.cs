using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheHunterKill : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Destroy(collision.gameObject); 
        }
        else if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }
}
