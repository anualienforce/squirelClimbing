using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Super subtle noise
            float x = Mathf.PerlinNoise(Time.time * 50f, 0f) * 2f - 1f;
            float y = Mathf.PerlinNoise(0f, Time.time * 50f) * 2f - 1f;

            transform.localPosition = originalPos + new Vector3(x, y, 0f) * magnitude * 0.5f;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Smoothly return to the original position
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, 0.2f);
        transform.localPosition = originalPos;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(duration, magnitude));
    }
}
