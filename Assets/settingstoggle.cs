using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public Button settingsButton;   // Assign your Settings button here
    public GameObject soundButton;  // Assign your Sound button here
    public GameObject Shopbutton;

    private void Start()
    {
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(ToggleSoundButton);
        }

        // Make sure the sound button is initially inactive
        if (soundButton != null)
        {
            soundButton.SetActive(false);
        }
        if (Shopbutton != null)
        {
            Shopbutton.SetActive(false);
        }
    }

    private void ToggleSoundButton()
    {
        if (soundButton != null)
        {
            soundButton.SetActive(!soundButton.activeSelf);
        }
        if (Shopbutton != null)
        {
            Shopbutton.SetActive(!Shopbutton.activeSelf);
        }

    }
}
