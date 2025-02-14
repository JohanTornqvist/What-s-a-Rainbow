using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

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
    [SerializeField] TilemapRenderer tilemapRenderer;
    [SerializeField] SpriteRenderer sprite;

    private void Start()
    {
        sprite.enabled = false;
        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
        theHunter.enabled = false;
        teleport.enabled = false;
        theHunterRb.gravityScale = 0;
        tilemapRenderer.enabled = true;

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
            emotionControler.playerState = 1;  // Set player state when exiting the trigger zone

            // Disable player movement and input immediately
            playerMove.canMove = false;
            playerInput.enabled = false;
            tilemapRenderer.enabled = false;

            // Start coroutine to re-enable movement and other behaviors after a delay
            StartCoroutine(EnableMovementAfterDelay(2f)); // Delay before re-enabling
        }
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time before proceeding

        // Re-enable movement, hunter behavior, and audio after the delay
        playerMove.canMove = true;  // Re-enable player movement
        playerInput.enabled = true; // Re-enable player input
        theHunter.enabled = true;   // Enable the hunter
        teleport.enabled = true;    // Enable teleportation
        theHunterRb.gravityScale = 5; // Apply gravity to the hunter

        // Move the hunter to the spawn point and play the audio clip
        hunter.transform.position = hunterSpawnPoint.transform.position;
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio_hunt); // Play the "hunt" audio
        }
    }

    public void Update()
    {
        // Ensure the trigger is disabled when playerState is 1 (after exiting the zone)
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
