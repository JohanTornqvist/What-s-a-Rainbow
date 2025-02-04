using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject key;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (key == null)
        {
            Destroy(gameObject);
        }
    }
}
