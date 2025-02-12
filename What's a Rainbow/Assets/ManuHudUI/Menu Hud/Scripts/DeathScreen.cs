using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private UIDocument deathScreenDocument;
    private VisualElement deathScreenRoot;
    private Button quitButton;
    private Button restartButton;

    void Start()
    {
        if (deathScreenDocument == null)
        {
            Debug.LogError("DeathScreen: UIDocument is not assigned.");
            return;
        }

        var root = deathScreenDocument.rootVisualElement;

        // Hide Death Screen
        deathScreenRoot = root.Q<VisualElement>("Background");
        if (deathScreenRoot != null)
        {
            deathScreenRoot.style.display = DisplayStyle.None;
        }
        else
        {
            Debug.LogError("DeathScreenRoot element not found.");
        }

        // Initialize Buttons
        quitButton = root.Q<Button>("QuitButton");
        if (quitButton != null)
        {
            quitButton.clicked += OnQuitButtonPressed;
        }
        else
        {
            Debug.LogError("Quit button not found.");
        }

        restartButton = root.Q<Button>("RestartButton");
        if (restartButton != null)
        {
            restartButton.clicked += OnRestartButtonPressed;
        }
        else
        {
            Debug.LogError("Restart button not found.");
        }
    }

    // Public method to open the death screen
    public void OpenDeathScreen()
    {
        if (deathScreenRoot != null)
        {
            deathScreenRoot.style.display = DisplayStyle.Flex;  // Show the death screen
        }
        else
        {
            Debug.LogError("DeathScreenRoot not found.");
        }
    }

    private void OnQuitButtonPressed()
    {
        Time.timeScale = 1;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnRestartButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
