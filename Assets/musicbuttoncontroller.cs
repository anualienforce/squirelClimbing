using UnityEngine;
using UnityEngine.UI;

public class MusicButtonController : MonoBehaviour
{
    [Header("UI Images")]
    public GameObject musicOnImage;   // Child image for "music on"
    public GameObject musicOffImage;  // Child image for "music off"

    private AudioSource bgAudio;

    private void Start()
    {
        // Find the BackgroundMusic AudioSource
        BackgroundMusic bgMusicInstance = FindObjectOfType<BackgroundMusic>();
        if (bgMusicInstance != null)
        {
            bgAudio = bgMusicInstance.GetComponent<AudioSource>();
        }

        // Load saved music state
        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1; // default on

        if (musicOn && bgAudio != null)
        {
            bgAudio.Play();
            musicOnImage.SetActive(true);
            musicOffImage.SetActive(false);
        }
        else if (!musicOn && bgAudio != null)
        {
            bgAudio.Pause();
            musicOnImage.SetActive(false);
            musicOffImage.SetActive(true);
        }
    }

    public void ToggleMusic()
    {
        if (bgAudio == null) return;

        if (bgAudio.isPlaying)
        {
            // Turn music off
            bgAudio.Pause();
            musicOnImage.SetActive(false);
            musicOffImage.SetActive(true);
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            // Turn music on
            bgAudio.Play();
            musicOnImage.SetActive(true);
            musicOffImage.SetActive(false);
            PlayerPrefs.SetInt("MusicOn", 1);
        }

        PlayerPrefs.Save();
    }
}
