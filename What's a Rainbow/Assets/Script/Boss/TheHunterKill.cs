using System.Collections;
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
            Killscript killScript = collision.GetComponent<Killscript>(); // Get the Killscript on the player

            if (killScript != null)
            {
                killScript.StartDeathSequence(); // Call the method in Killscript
            }
        }
    }
}

