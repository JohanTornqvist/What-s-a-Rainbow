using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HuntExitScript : MonoBehaviour
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
            emotionControler.playerState = 0;  // Set the player state
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
