using UnityEngine;
using UnityEngine.UI;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [Header("UI References")]
    public Text promptText;
    public GameObject promptPanel;

    [Header("Settings")]
    public string defaultPrompt = "Press E to interact";

    private string currentPrompt;
    private bool shouldShow;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    void LateUpdate()
    {
        // Update UI at end of frame after all scripts have run
        if (promptPanel != null)
        {
            promptPanel.SetActive(shouldShow);
            if (shouldShow && promptText != null)
            {
                promptText.text = currentPrompt;
            }
        }

        // Reset for next frame
        shouldShow = false;
        currentPrompt = defaultPrompt;
    }

    /// <summary>
    /// Call this from any interactable script to show the prompt
    /// </summary>
    public void ShowPrompt(string message = null)
    {
        shouldShow = true;
        if (!string.IsNullOrEmpty(message))
            currentPrompt = message;
    }

    /// <summary>
    /// Hide the prompt (optional - it auto-hides if ShowPrompt isn't called)
    /// </summary>
    public void HidePrompt()
    {
        shouldShow = false;
    }
}
