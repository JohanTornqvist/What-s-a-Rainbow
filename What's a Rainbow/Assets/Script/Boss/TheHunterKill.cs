using UnityEngine;

public class TheHunterKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Killscript killScript = collision.GetComponent<Killscript>();

            if (killScript != null)
            {
                killScript.StartDeathSequence();
            }
            else
            {
                Debug.LogError("Killscript component not found on the player.");
            }
        }
    }
}
