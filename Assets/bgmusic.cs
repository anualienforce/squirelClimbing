using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip bgMusic;    // Assign your background music clip here
    public float volume = 0.5f;  // Set volume

    private static BackgroundMusic instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = bgMusic;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
