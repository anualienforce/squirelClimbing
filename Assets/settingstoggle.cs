using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public Button settingsButton;   // Assign your Settings button here
    public GameObject soundButton;  // Assign your Sound button here
    public GameObject Shopbutton;
    public GameObject ratebtn;
    public GameObject privacybtn;

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
        if (ratebtn != null)
        {
            ratebtn.SetActive(false);
        }
        if (privacybtn != null)
        {
            privacybtn.SetActive(false);
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
        if (ratebtn != null)
        {
            ratebtn.SetActive(!ratebtn.activeSelf);
        }
        if (privacybtn != null)
        {
            privacybtn.SetActive(!privacybtn.activeSelf);
        }
    }
}
