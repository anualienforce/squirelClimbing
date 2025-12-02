using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip bgMusic;
    public float volume = 0.5f;

    private static BackgroundMusic instance;
    private AudioSource audioSource;

    public static BackgroundMusic Instance => instance;   // <-- Add this

    private void Awake()
    {
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
            Destroy(gameObject);
        }
    }

    // NEW: Turn OFF music
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // NEW: Turn ON music
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
