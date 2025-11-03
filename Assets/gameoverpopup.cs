using UnityEngine;
using System.Collections;

public class GameOverPopup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animDuration = 0.3f;

    [Header("Sound Settings")]
    public AudioClip gameOverSound;   // assign in Inspector
    [Range(0f, 1f)] public float volume = 1f;

    private static AudioSource uiAudioSource;

    private void Awake()
    {
        // Create a shared AudioSource for UI if not already present
        if (uiAudioSource == null)
        {
            GameObject audioObj = new GameObject("UIAudioSource");
            uiAudioSource = audioObj.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.spatialBlend = 0f; // 2D sound
            DontDestroyOnLoad(audioObj);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ShowPanel());

        // Play game-over sound
        if (gameOverSound != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(gameOverSound, volume);
    }

    private IEnumerator ShowPanel()
    {
        transform.localScale = Vector3.zero;
        float timer = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (timer < animDuration)
        {
            timer += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, timer / animDuration);
            yield return null;
        }

        transform.localScale = endScale;
    }
}
