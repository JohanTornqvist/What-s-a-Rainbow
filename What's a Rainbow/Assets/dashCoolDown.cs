using UnityEngine;
using UnityEngine.UIElements;

public class ProgressBarController : MonoBehaviour
{
    public UIDocument uiDocument;
    private ProgressBar progressBar;

    private float progressValue = 0f; // Example value (0-100)

    void OnEnable()
    {
        // Get the root visual element
        VisualElement root = uiDocument.rootVisualElement;

        // Find the Progress Bar by name
        progressBar = root.Q<ProgressBar>("progress-bar");

        // Initialize progress
        UpdateProgressBar();
    }

    void Update()
    {
        // Simulating progress increase (replace with your logic)
        progressValue = Mathf.PingPong(Time.time * 10, 100);

        // Update the UI
        UpdateProgressBar();
    }

    public void SetProgress(float value)
    {
        progressValue = Mathf.Clamp(value, 0, 100);
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = progressValue;
        }
    }
}
