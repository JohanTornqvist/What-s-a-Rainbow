using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI myText;
    public TextMeshProUGUI headerText;
    public float fadeDuration = 2f;
    public float stayDuration = 3f;
    private float targetAlpha = 0f;
    private float startAlphaImage = 1f;
    private float startAlphaText = 1f;
    private float startAlphaHeader = 1f;
    private float elapsedTime = 0f;
    private bool isFading = false;

    void Start()
    {
        // 🔹 Check if fade should be skipped (Scene loaded from Pause Menu)
        if (PlayerPrefs.GetInt("SkipFade", 0) == 1)
        {
            Debug.Log("Skipping fade effect...");
            fadeImage.gameObject.SetActive(false);
            if (myText != null) myText.gameObject.SetActive(false);
            if (headerText != null) headerText.gameObject.SetActive(false);

            // Reset flag so future scene loads have fade again
            PlayerPrefs.SetInt("SkipFade", 0);
            PlayerPrefs.Save();
            return;
        }

        Cursor.visible = false;
        startAlphaImage = fadeImage.color.a;
        if (myText != null) startAlphaText = myText.color.a;
        if (headerText != null) startAlphaHeader = headerText.color.a;
    }

    void Update()
    {
        if (!isFading)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stayDuration)
            {
                isFading = true;
                elapsedTime = 0f;
            }
        }
        else
        {
            if (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alphaImage = Mathf.Lerp(startAlphaImage, targetAlpha, elapsedTime / fadeDuration);
                float alphaText = Mathf.Lerp(startAlphaText, targetAlpha, elapsedTime / fadeDuration);
                float alphaHeader = Mathf.Lerp(startAlphaHeader, targetAlpha, elapsedTime / fadeDuration);

                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alphaImage);
                if (myText != null) myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alphaText);
                if (headerText != null) headerText.color = new Color(headerText.color.r, headerText.color.g, headerText.color.b, alphaHeader);
            }
            else
            {
                Cursor.visible = true;
                if (fadeImage.color.a <= 0f) fadeImage.gameObject.SetActive(false);
                if (myText != null && myText.color.a <= 0f) myText.gameObject.SetActive(false);
                if (headerText != null && headerText.color.a <= 0f) headerText.gameObject.SetActive(false);
            }
        }
    }

    public void StartFade()
    {
        elapsedTime = 0f;
        isFading = false;
        startAlphaImage = fadeImage.color.a;
        if (myText != null) startAlphaText = myText.color.a;
        if (headerText != null) startAlphaHeader = headerText.color.a;
        Cursor.visible = false;
    }

    public void SetHeaderText(string newText)
    {
        if (headerText != null) headerText.text = newText;
    }
}
