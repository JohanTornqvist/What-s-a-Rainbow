using UnityEngine;
using UnityEngine.UI; // Make sure this is included for UI components
using System.Collections;

public class CooldownButtonScript : MonoBehaviour
{
    public Button myButton;           // Reference to the button
    public Text cooldownText;         // Reference to the text showing the countdown
    public float cooldownTime = 5f;   // Total time for cooldown (in seconds)

    private bool isOnCooldown = false; // Flag to track cooldown status

    void Start()
    {
        // Set up the button's onClick listener to trigger the cooldown function
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
    }

    // Make sure this method is marked as public
    public void OnButtonClick()
    {
        // If not on cooldown, start the countdown
        if (!isOnCooldown)
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    // Coroutine for handling the cooldown
    IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true; // Set the flag to true, indicating that cooldown is active
        float remainingTime = cooldownTime;

        // Update the countdown every frame until it reaches 0
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Decrease time based on frame rate
            cooldownText.text = " " + Mathf.Ceil(remainingTime).ToString(); // Update text
            yield return null; // Wait for the next frame
        }

        cooldownText.text = ""; // Once cooldown ends, set text to 0
        isOnCooldown = false; // Reset cooldown flag
    }
}
