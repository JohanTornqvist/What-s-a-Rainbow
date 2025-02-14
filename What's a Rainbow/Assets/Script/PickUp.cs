using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the correct tag
        {
            audioSource.PlayOneShot(pickup);
            sprite.enabled = false;
            Destroy(gameObject);
        }
    }
}
