using System.Collections;
using UnityEngine;

public class Hunt_Audio_Controler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public AudioClip activeAudio;
    public int playerState = 0;
    private int oldRnd = -1; // Store the last played sound

    public AudioClip[] audioClips; // Array to store all audio clips

    private void Start()
    {
        StartCoroutine(PlayAudioLoop());
    }

    private IEnumerator PlayAudioLoop()
    {
        while (true)
        {
            if (playerState == 1)
            {
                yield return new WaitForSeconds(3f); // Wait 2 seconds
                ChangeAudio();
            }
            else
            {
                yield return new WaitForSeconds(1f); // Avoid unnecessary CPU usage
            }
        }
    }

    void ChangeAudio()
    {
        int rand;

        // Keep generating a new random number until it's different from the last one
        do
        {
            rand = Random.Range(0, audioClips.Length);
        }
        while (rand == oldRnd);

        oldRnd = rand; // Store the last played sound
        activeAudio = audioClips[rand];
        if(playerState == 1)
        audioSource.PlayOneShot(activeAudio);
    }
}
