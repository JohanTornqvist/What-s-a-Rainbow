using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LvlSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneName; // Stores the scene name

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only the Player triggers it
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                Debug.Log("Switching to scene: " + sceneName);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("No scene assigned in LvlSwitcher! Please assign a scene.");
            }
        }
    }
}
