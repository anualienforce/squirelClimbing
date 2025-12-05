using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsButton : MonoBehaviour
{
    public Button settingsButton;

    public RectTransform soundButton;
    public RectTransform shopButton;
    public RectTransform rateButton;
    public RectTransform privacyButton;

    public float animationTime = 0.25f;

    RectTransform settingsRect;
    Vector2 soundFinalPos, shopFinalPos, rateFinalPos, privacyFinalPos;
    bool isOpen = false;
    bool isAnimating = false;

    void Awake()
    {
        settingsRect = settingsButton.GetComponent<RectTransform>();

        // Cache final positions (where you placed them)
        soundFinalPos = soundButton.anchoredPosition;
        shopFinalPos = shopButton.anchoredPosition;
        rateFinalPos = rateButton.anchoredPosition;
        privacyFinalPos = privacyButton.anchoredPosition;

        // Compute center as settings button position converted to each child's parent space
        Vector3 worldCenter = settingsRect.position;

        soundButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        rateButton.gameObject.SetActive(false);
        privacyButton.gameObject.SetActive(false);

        soundButton.anchoredPosition =
            WorldToAnchored(soundButton.parent as RectTransform, worldCenter);
        shopButton.anchoredPosition =
            WorldToAnchored(shopButton.parent as RectTransform, worldCenter);
        rateButton.anchoredPosition =
            WorldToAnchored(rateButton.parent as RectTransform, worldCenter);
        privacyButton.anchoredPosition =
            WorldToAnchored(privacyButton.parent as RectTransform, worldCenter);
    }

    void Start()
    {
        settingsButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        if (isAnimating) return;
        StartCoroutine(isOpen ? AnimateClose() : AnimateOpen());
    }

    IEnumerator AnimateOpen()
    {
        isAnimating = true;
        isOpen = true;

        Vector3 worldCenter = settingsRect.position;

        soundButton.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(true);
        rateButton.gameObject.SetActive(true);
        privacyButton.gameObject.SetActive(true);

        Vector2 soundCenter = WorldToAnchored(soundButton.parent as RectTransform, worldCenter);
        Vector2 shopCenter = WorldToAnchored(shopButton.parent as RectTransform, worldCenter);
        Vector2 rateCenter = WorldToAnchored(rateButton.parent as RectTransform, worldCenter);
        Vector2 privacyCenter = WorldToAnchored(privacyButton.parent as RectTransform, worldCenter);

        soundButton.anchoredPosition = soundCenter;
        shopButton.anchoredPosition = shopCenter;
        rateButton.anchoredPosition = rateCenter;
        privacyButton.anchoredPosition = privacyCenter;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animationTime;
            float k = Mathf.SmoothStep(0f, 1f, t);

            soundButton.anchoredPosition = Vector2.Lerp(soundCenter, soundFinalPos, k);
            shopButton.anchoredPosition = Vector2.Lerp(shopCenter, shopFinalPos, k);
            rateButton.anchoredPosition = Vector2.Lerp(rateCenter, rateFinalPos, k);
            privacyButton.anchoredPosition = Vector2.Lerp(privacyCenter, privacyFinalPos, k);

            yield return null;
        }

        isAnimating = false;
    }

    IEnumerator AnimateClose()
    {
        isAnimating = true;
        isOpen = false;

        Vector3 worldCenter = settingsRect.position;

        Vector2 soundCenter = WorldToAnchored(soundButton.parent as RectTransform, worldCenter);
        Vector2 shopCenter = WorldToAnchored(shopButton.parent as RectTransform, worldCenter);
        Vector2 rateCenter = WorldToAnchored(rateButton.parent as RectTransform, worldCenter);
        Vector2 privacyCenter = WorldToAnchored(privacyButton.parent as RectTransform, worldCenter);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animationTime;
            float k = Mathf.SmoothStep(0f, 1f, t);

            soundButton.anchoredPosition = Vector2.Lerp(soundFinalPos, soundCenter, k);
            shopButton.anchoredPosition = Vector2.Lerp(shopFinalPos, shopCenter, k);
            rateButton.anchoredPosition = Vector2.Lerp(rateFinalPos, rateCenter, k);
            privacyButton.anchoredPosition = Vector2.Lerp(privacyFinalPos, privacyCenter, k);

            yield return null;
        }

        soundButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        rateButton.gameObject.SetActive(false);
        privacyButton.gameObject.SetActive(false);

        isAnimating = false;
    }

    // Helper: convert world position to anchoredPosition in a RectTransform parent
    Vector2 WorldToAnchored(RectTransform parent, Vector3 worldPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            RectTransformUtility.WorldToScreenPoint(null, worldPos),
            null,
            out localPoint);
        return localPoint;
    }
}
