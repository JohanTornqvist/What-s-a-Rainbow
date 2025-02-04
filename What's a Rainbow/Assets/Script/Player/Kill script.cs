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
            Destroy(gameObject);
            Debug.Log("Compare tag");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        
    }
}
