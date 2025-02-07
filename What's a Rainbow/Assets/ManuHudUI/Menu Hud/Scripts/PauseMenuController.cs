using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument pauseMenuDocument;
    [SerializeField] private List<ButtonEvent> pauseButtonEvents;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string volumeName = "MasterVolume";

    private VisualElement pauseMenu;
    private VisualElement blurredBackground;
    private VisualElement background; // Updated for new background visual element
    private Label pauseLabel; // Updated to reference the "Pause" label
    private Button quitButton;
    private bool isPaused = false;

    private void Start()
    {
        var root = pauseMenuDocument.rootVisualElement;
        pauseMenu = root.Q<VisualElement>("popupWindow");
        blurredBackground = root.Q<VisualElement>("blurredBackground");
        background = root.Q<VisualElement>("background"); // Get background
        pauseLabel = root.Q<Label>("Pause"); // Get the "Pause" label
        quitButton = root.Q<Button>("quitButton");

        if (quitButton != null)
        {
            quitButton.clicked += QuitGame; // Attach Quit Function
        }
        else
        {
            Debug.LogError("❌ Quit Button not found in UI Document!");
        }

        // Ensure the pause menu and other UI elements are hidden initially
        pauseMenu.style.display = DisplayStyle.None;
        blurredBackground.style.display = DisplayStyle.None;
        background.style.display = DisplayStyle.None;
        pauseLabel.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0;
        isPaused = true;

        // Show the pause menu and related UI elements
        pauseMenu.style.display = DisplayStyle.Flex;
        blurredBackground.style.display = DisplayStyle.Flex;
        background.style.display = DisplayStyle.Flex;
        pauseLabel.style.display = DisplayStyle.Flex; // Show the Pause label

        // Activate button events for Pause Menu
        pauseButtonEvents.ForEach(button => button.Activate(pauseMenuDocument));
    }

    private void ClosePauseMenu()
    {
        Time.timeScale = 1;
        isPaused = false;

        // Hide the pause menu and related UI elements
        pauseMenu.style.display = DisplayStyle.None;
        blurredBackground.style.display = DisplayStyle.None;
        background.style.display = DisplayStyle.None;
        pauseLabel.style.display = DisplayStyle.None; // Hide the Pause label

        // Deactivate button events
        pauseButtonEvents.ForEach(button => button.Inactivate(pauseMenuDocument));
    }

    public void QuitGame()
    {
        Debug.Log("✅ Quit button clicked! Loading Scene 0...");
        StartCoroutine(LoadSceneAsync(0)); // Load Scene Asynchronously
    }

    private IEnumerator LoadSceneAsync(int buildIndex)
    {
        // 🔹 Set flag to skip fade
        PlayerPrefs.SetInt("SkipFade", 1);
        PlayerPrefs.Save();

        Time.timeScale = 1; // Ensure time is resumed before loading

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }
    }

    private void OnEnable()
    {
        pauseButtonEvents.ForEach(button => button.Activate(pauseMenuDocument));
    }

    private void OnDisable()
    {
        pauseButtonEvents.ForEach(button => button.Inactivate(pauseMenuDocument));
    }
}
