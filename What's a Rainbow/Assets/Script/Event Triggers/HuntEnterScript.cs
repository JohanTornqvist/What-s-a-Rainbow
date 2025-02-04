using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HuntEnterScript : MonoBehaviour
{
    [SerializeField] private Collider2D trigger;
    public Volume globalVolume;
    public Emotion emotionControler;
    public playerMovement playerMove;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<playerMovement>();

        GameObject Emotion = GameObject.FindWithTag("EmotionControle");
        emotionControler = Emotion.GetComponent<Emotion>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the correct tag
        {
            emotionControler.playerState = 1;  // Set the player state
            playerMove.canMove = false;        // Disable movement

            // Start a coroutine to wait and enable movement after a delay
            StartCoroutine(EnableMovementAfterDelay(2f));
        }
    }
    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);

        // Re-enable movement
        playerMove.canMove = true;
    }

    public void Update()
    {
        if(playerMove.playerState == 1)
        {
            trigger.enabled = false;
        }
        else trigger.enabled = true;
    }
}
