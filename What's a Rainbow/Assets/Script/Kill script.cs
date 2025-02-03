using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killscript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Destroy(gameObject);
            Debug.Log("Compare tag");
        }
    }
}
