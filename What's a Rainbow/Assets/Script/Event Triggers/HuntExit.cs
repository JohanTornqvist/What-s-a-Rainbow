using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HuntExitScript : MonoBehaviour
{
    [SerializeField] private Collider2D trigger;
    public Volume globalVolume;
    public Emotion emotionControler;
    public PlayerMovement playerMove;
    public Hunt_Audio_Controler huntAudio;
    [SerializeField] SpriteRenderer sprite;

    private void Start()
    {
        sprite.enabled = false;
        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
        huntAudio = player.GetComponent<Hunt_Audio_Controler>();

        GameObject Emotion = GameObject.FindWithTag("EmotionControle");
        emotionControler = Emotion.GetComponent<Emotion>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the correct tag
        {
            emotionControler.playerState = 0;  // Set the player state
            huntAudio.playerState = 0;
        }
    }

    public void Update()
    {
        if (playerMove.playerState == 0)
        {
            trigger.enabled = false;
        }
        else trigger.enabled = true;
    }
}
