using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HuntEnterScript : MonoBehaviour
{
    [SerializeField] private Collider2D trigger;
    public Volume globalVolume;
    public Emotion emotionControler;
    public AudioSource audioSource;
    public AudioClip audio_hunt;
    public PlayerMovement playerMove;
    public GameObject hunterSpawnPoint;
    public GameObject hunter;
    public TheHunter theHunter;
    public Rigidbody2D theHunterRb;
    public TeleportPlayer teleport;
    public PlayerInput playerInput;
    [SerializeField] SpriteRenderer sprite;

    private void Start()
    {
        sprite.enabled = false;
        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
        theHunter.enabled = false;
        teleport.enabled = false;
        theHunterRb.gravityScale = 0;

        GameObject Emotion = GameObject.FindWithTag("EmotionControle");
        emotionControler = Emotion.GetComponent<Emotion>();

        // Ensure audioSource is assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the Player has the correct tag
        {
            emotionControler.playerState = 1;  // Set player state
            playerMove.canMove = false;
            playerInput.enabled = false;
            // Disable movement

            // Start coroutine to re-enable movement after delay
            StartCoroutine(EnableMovementAfterDelay(2f));
        }
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time

        playerMove.canMove = true;  // Re-enable movement
        playerInput.enabled = true;
        theHunter.enabled = true;
        teleport.enabled = true;
        theHunterRb.gravityScale = 5;

        hunter.transform.position = hunterSpawnPoint.transform.position;
        if (audioSource != null)
            audioSource.PlayOneShot(audio_hunt);
        
    }

    public void Update()
    {
        if (emotionControler.playerState == 1)
        {
            if (trigger != null)
                trigger.enabled = false;
        }
        else if (trigger != null)
        {
            trigger.enabled = true;
        }
    }
}
