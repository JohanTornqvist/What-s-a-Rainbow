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
    private Button quitButton;
    private bool isPaused = false;

    private void Start()
    {
        var root = pauseMenuDocument.rootVisualElement;
        pauseMenu = root.Q<VisualElement>("popupWindow");
        blurredBackground = root.Q<VisualElement>("blurredBackground");
        quitButton = root.Q<Button>("quitButton"); // Get Quit Button

        if (quitButton != null)
        {
            quitButton.clicked += QuitGame; // Attach Quit Function
        }
        else
        {
            Debug.LogError("❌ Quit Button not found in UI Document!");
        }

        // Ensure the pause menu is hidden when the game starts
        pauseMenu.style.display = DisplayStyle.None;
        blurredBackground.style.display = DisplayStyle.None;
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
        pauseMenu.style.display = DisplayStyle.Flex;
        blurredBackground.style.display = DisplayStyle.Flex;
        pauseButtonEvents.ForEach(button => button.Activate(pauseMenuDocument));
    }

    private void ClosePauseMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.style.display = DisplayStyle.None;
        blurredBackground.style.display = DisplayStyle.None;
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
