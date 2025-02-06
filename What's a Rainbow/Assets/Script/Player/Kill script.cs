using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            // add tag death to anything that needs to kill player. Only player should have this script.
            Destroy(gameObject);
            Debug.Log("Compare tag");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
