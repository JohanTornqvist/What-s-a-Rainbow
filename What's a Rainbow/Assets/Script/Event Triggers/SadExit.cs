using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadExit : MonoBehaviour
{
    [SerializeField] private Collider2D trigger;
    public Emotion emotionControler;


    void Start()
    {
        GameObject Emotion = GameObject.FindWithTag("EmotionControle");
        emotionControler = Emotion.GetComponent<Emotion>();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        emotionControler.playerState = 0;

    }

    private void Update()
    {
        if (emotionControler.playerState == 0)
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
