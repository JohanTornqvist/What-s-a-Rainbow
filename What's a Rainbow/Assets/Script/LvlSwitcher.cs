using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LvlSwitcher : MonoBehaviour
{
    [SerializeField] private SceneReference sceneToLoad; // Scene asset reference

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the correct tag
        {
            if (!string.IsNullOrEmpty(sceneToLoad.ScenePath)) // Ensure a valid scene
            {
                SceneManager.LoadScene(sceneToLoad.SceneName);
            }
            else
            {
                Debug.LogWarning("No scene assigned in LvlSwitcher! Please assign a scene in the Inspector.");
            }
        }
    }
}
