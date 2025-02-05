using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    public float offset = 10f;
    public float delay = 5f;
    public float tolerance = 1.5f;

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
                // Calculate a new position for the player:
                // - The x position is offset from the enemy's position.
                // - The y position is set equal to the enemy's y to remove any vertical hiding.
                // - The z position is preserved (if you're using 3D; for 2D you might ignore z).
                Vector3 newPosition = new Vector3(transform.position.x - offset, transform.position.y, player.transform.position.z);
                player.transform.position = newPosition;
                Debug.Log("Player teleported to: " + newPosition);
            }
            else
            {
                Debug.Log("Player and enemy are not aligned on the X axis; teleportation skipped.");
            }
        }
    }
}
