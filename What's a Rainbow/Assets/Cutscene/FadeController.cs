using UnityEngine;
using TMPro;  // For TextMeshPro
using UnityEngine.UI;  // For Image component

public class FadeImageAndText : MonoBehaviour
{
    public Image fadeImage;         // Reference to the Image (black screen)
    public TextMeshProUGUI myText;  // Reference to the TextMeshPro UI text (message text)
    public TextMeshProUGUI headerText; // Reference to the TextMeshPro UI header text
    public float fadeDuration = 2f; // Duration for fade effect (in seconds)
    private float targetAlpha = 0f; // Target alpha value (0 means fully transparent)
    private float startAlphaImage = 1f;  // Starting alpha value for the image
    private float startAlphaText = 1f;   // Starting alpha value for the text
    private float startAlphaHeader = 1f; // Starting alpha value for header text
    private float elapsedTime = 0f;

    void Start()
    {
        // Initialize the starting alpha values from the image, text, and header text's current color
        startAlphaImage = fadeImage.color.a;
        if (myText != null)
        {
            startAlphaText = myText.color.a;
        }
        if (headerText != null)
        {
            startAlphaHeader = headerText.color.a;
        }
    }

    void Update()
    {
        // Fade the image, text, and header text's alpha over time
        if (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alphaImage = Mathf.Lerp(startAlphaImage, targetAlpha, elapsedTime / fadeDuration);
            float alphaText = Mathf.Lerp(startAlphaText, targetAlpha, elapsedTime / fadeDuration);
            float alphaHeader = Mathf.Lerp(startAlphaHeader, targetAlpha, elapsedTime / fadeDuration);

            // Fade the image alpha
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alphaImage);

            // Fade the text alpha
            if (myText != null)
            {
                myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alphaText);
            }

            // Fade the header text alpha
            if (headerText != null)
            {
                headerText.color = new Color(headerText.color.r, headerText.color.g, headerText.color.b, alphaHeader);
            }
        }
        else
        {
            // Disable the image and text once they are fully transparent to allow button interactions
            if (fadeImage.color.a <= 0f)
            {
                fadeImage.gameObject.SetActive(false); // Disable the black screen
            }

            if (myText != null && myText.color.a <= 0f)
            {
                myText.gameObject.SetActive(false); // Disable the message text
            }

            if (headerText != null && headerText.color.a <= 0f)
            {
                headerText.gameObject.SetActive(false); // Disable the header text
            }
        }
    }

    // Optional: Start the fade effect
    public void StartFade()
    {
        elapsedTime = 0f;
        startAlphaImage = fadeImage.color.a;
        if (myText != null)
        {
            startAlphaText = myText.color.a;
        }
        if (headerText != null)
        {
            startAlphaHeader = headerText.color.a;
        }
    }

    // Set the header text during fade-in (Optional)
    public void SetHeaderText(string newText)
    {
        if (headerText != null)
        {
            headerText.text = newText;
        }
    }
}
