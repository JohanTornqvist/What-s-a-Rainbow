using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    public float offset = 10f;
    public float delay = 5f;
    public float tolerance = 1.5f;
    public ParticleSystem teleportParticals; // Keep original variable name

    void Start()
    {
        StartCoroutine(TeleportAfterDelay());
    }

    IEnumerator TeleportAfterDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Player not found!");
                continue;
            }

            if (Mathf.Abs(player.transform.position.x - transform.position.x) < tolerance)
            {
                Vector3 oldPosition = player.transform.position;
                Vector3 newPosition = new Vector3(transform.position.x - offset, transform.position.y, player.transform.position.z);

                // Spawn particles at the old position
                SpawnParticles(oldPosition);

                // Move the player
                player.transform.position = newPosition;
                Debug.Log("Player teleported to: " + newPosition);

                // Spawn particles at the new position
                SpawnParticles(newPosition);
            }
            else
            {
                Debug.Log("Player and enemy are not aligned on the X axis; teleportation skipped.");
            }
        }
    }

    void SpawnParticles(Vector3 position)
    {
        if (teleportParticals != null)
        {
            ParticleSystem particles = Instantiate(teleportParticals, position, Quaternion.identity);

            // Ensure particles render behind everything
            Renderer particleRenderer = particles.GetComponent<Renderer>();
            if (particleRenderer != null)
            {
                particleRenderer.sortingLayerName = "Background"; // Ensure this layer exists in Unity
                particleRenderer.sortingOrder = -10; // Renders below all objects
            }

            Destroy(particles.gameObject, 1f); // Destroy the particles after 1 second
        }
    }
}
