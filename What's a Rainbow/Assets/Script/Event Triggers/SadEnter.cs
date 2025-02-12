using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadEnter : MonoBehaviour
{
    [SerializeField] private Collider2D trigger;
    public Emotion emotionControler;
    [SerializeField] SpriteRenderer sprite;


    void Start()
    {
        sprite.enabled = false;
        GameObject Emotion = GameObject.FindWithTag("EmotionControle");
        emotionControler = Emotion.GetComponent<Emotion>();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the Player has the correct tag
        {
            emotionControler.playerState = 2;
        }
    }

    private void Update()
    {
        if (emotionControler.playerState == 2)
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
