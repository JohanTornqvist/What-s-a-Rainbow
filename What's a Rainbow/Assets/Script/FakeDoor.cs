using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDoor : MonoBehaviour
{
    public GameObject key;
    public Collider2D colliderDoor;
    public SpriteRenderer sprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (key == null)
        {
            colliderDoor.enabled = true;
            sprite.enabled = true;
        }
    }
}
