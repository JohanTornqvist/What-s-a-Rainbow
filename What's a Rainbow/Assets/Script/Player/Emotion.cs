using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Emotion : MonoBehaviour
{


    public playerMovement playerMove;
    public Hunt_Audio_Controler huntAudio;
    public int playerState = 0;

    public Volume globalVolume;
    public VolumeProfile normVolume;
    public VolumeProfile huntVolume;
    public VolumeProfile sadVolume;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<playerMovement>();
        huntAudio = player.GetComponent<Hunt_Audio_Controler>();
    }
    void Update()
    {
        switch (playerState)
        {
            case 0:
                normal();
                break;
            case 1:
                hunting();
                break;
            case 2:
                sad();
                break;
        }
    }

    void normal()
    {
        globalVolume.profile = normVolume;
        playerMove.playerState = playerState;
    }

    void hunting()
    {
        globalVolume.profile = huntVolume;
        playerMove.playerState = playerState;
        huntAudio.playerState = playerState;

        if (globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.postExposure.value = -1.8f; // Instantly darken
            StartCoroutine(RestoreExposure(colorAdjustments, 2f)); // Restore after 1 second
        }
    }

    IEnumerator RestoreExposure(ColorAdjustments colorAdjustments, float delay)
    {
        yield return new WaitForSeconds(delay);
        colorAdjustments.postExposure.value = 4.35f; // Restore exposure

    }



    void sad()
    {
        globalVolume.profile = sadVolume;
        playerMove.playerState = playerState;
    }

    public void SetSaturation(float value)
    {
            colorAdjustments.saturation.value = value;

    }
}


