using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBreathingWithSound : MonoBehaviour
{
    [Header("Breathing Settings")]
    public float scaleAmount = 1.07f;   // subtle expansion
    public float speed = 1.2f;          // breathing speed
    public bool randomOffset = true;

    [Header("Sound Settings")]
    public AudioClip clickSound;        // UI click sound
    [Range(0f, 1f)] public float volume = 1f;

    private Vector3 originalScale;
    private float offset;
    private Button button;
    private static AudioSource uiAudioSource;

    void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        offset = randomOffset ? Random.Range(0f, Mathf.PI * 2f) : 0f;

        // Create a shared AudioSource for all UI sounds (2D)
        if (uiAudioSource == null)
        {
            GameObject audioObj = new GameObject("UIAudioSource");
            uiAudioSource = audioObj.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.spatialBlend = 0f; // 2D sound
            DontDestroyOnLoad(audioObj);
        }

        button.onClick.AddListener(PlayClickSound);
    }

    void Update()
    {
        float scale = 1 + (Mathf.Sin(Time.time * speed + offset) * (scaleAmount - 1));
        transform.localScale = originalScale * scale;
    }

  public  void PlayClickSound()
    {
        if (clickSound != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(clickSound, volume);
    }

    void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PlayClickSound);
    }
}
