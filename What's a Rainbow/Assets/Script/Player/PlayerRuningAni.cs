using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRuningAni : MonoBehaviour
{
    private Animator ani;
    public Emotion emotionControler;

    // Start is called before the first frame update
    void Start()
    {
        GameObject emotionObject = GameObject.FindWithTag("EmotionControle");
        if (emotionObject != null)
        {
            emotionControler = emotionObject.GetComponent<Emotion>();
        }
        ani = GetComponent<Animator>();
        ani.SetBool("isPlayerRuning", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (emotionControler.playerState == 1)
        {
            ani.SetBool("isPlayerRuning", true);
            ani.SetBool("isWalking", false);
            ani.SetBool("isIdle2", false);
        }
        else
        {
            ani.SetBool("isPlayerRuning", false);
        }
    }
}
