using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Reference to the fade image (black screen)
    public float fadeDuration = 2f; // Duration of the fade-out effect
    public string sceneToLoad = "Main Menu"; // Name of the scene to load after fade (updated)
    public float stayDuration = 10f; // Duration the black screen stays before fading out

    void Start()
    {
        // Start the fade-out process after staying on screen for a while
        StartCoroutine(WaitAndFade());
    }

    IEnumerator WaitAndFade()
    {
        // Wait for the desired amount of time before starting the fade-out
        yield return new WaitForSeconds(stayDuration);

        // Start the fade-out process
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // Ensure the fade image starts fully opaque (black screen)
        fadeImage.canvasRenderer.SetAlpha(1f);

        // Fade out over time (using CrossFadeAlpha)
        fadeImage.CrossFadeAlpha(0f, fadeDuration, false);

        // Wait for the fade-out to complete
        yield return new WaitForSeconds(fadeDuration);

        // Disable the image after the fade-out is complete (make sure it's hidden)
        fadeImage.gameObject.SetActive(false);

        // Check if the current scene is not already the Main Menu
        if (SceneManager.GetActiveScene().name != sceneToLoad)
        {
            // Load the "Main Menu" scene after the fade-out
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
