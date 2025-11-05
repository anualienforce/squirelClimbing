using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitPanelManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject exitPanel;
    public Button yesButton;
    public Button noButton;

    [Header("Other Panels (optional)")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Animation Settings")]
    public float animDuration = 0.25f;

    private bool isPaused = false;

    private void Start()
    {
        if (exitPanel != null)
            exitPanel.SetActive(false);

        if (yesButton != null)
            yesButton.onClick.AddListener(OnYesButton);

        if (noButton != null)
            noButton.onClick.AddListener(OnNoButton);
    }

    private void Update()
    {
        // Detect Android back button (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Prevent opening exit panel if pause or gameover is active
            if ((pausePanel != null && pausePanel.activeSelf) ||
                (gameOverPanel != null && gameOverPanel.activeSelf))
                return;

            if (exitPanel == null) return;

            if (exitPanel.activeSelf)
                StartCoroutine(HideExitPanel());
            else
                StartCoroutine(ShowExitPanel());
        }
    }

    IEnumerator ShowExitPanel()
    {
        isPaused = true;
        exitPanel.SetActive(true);

        // Animate scale up
        float timer = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        exitPanel.transform.localScale = startScale;

        while (timer < animDuration)
        {
            timer += Time.unscaledDeltaTime;
            exitPanel.transform.localScale = Vector3.Lerp(startScale, endScale, timer / animDuration);
            yield return null;
        }

        exitPanel.transform.localScale = endScale;
        Time.timeScale = 0f; // pause game
    }

    IEnumerator HideExitPanel()
    {
        Time.timeScale = 1f; // resume game

        float timer = 0f;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        while (timer < animDuration)
        {
            timer += Time.unscaledDeltaTime;
            exitPanel.transform.localScale = Vector3.Lerp(startScale, endScale, timer / animDuration);
            yield return null;
        }

        exitPanel.transform.localScale = endScale;
        exitPanel.SetActive(false);
        isPaused = false;
    }

    private void OnYesButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void OnNoButton()
    {
        StartCoroutine(HideExitPanel());
    }
}
