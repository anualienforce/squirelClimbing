using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button homeButton;
    public TextMeshProUGUI highScoreText; // Add this reference for high score display

    [Header("Animation Settings")]
    public float animDuration = 0.3f;
    private bool isPaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);

        pauseButton.onClick.AddListener(OnPauseButton);
        resumeButton.onClick.AddListener(OnResumeButton);
        homeButton.onClick.AddListener(OnHomeButton);
    }

    private void OnPauseButton()
    {
        if (isPaused) return;
        StartCoroutine(ShowPausePanel());
    }

    private void OnResumeButton()
    {
        if (!isPaused) return;
        StartCoroutine(HidePausePanel());
    }

    public void OnHomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu"); // change to your scene
    }

    IEnumerator ShowPausePanel()
    {
        isPaused = true;
        pausePanel.SetActive(true);

        // Display high score
        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "High Score: " + highScore.ToString();
        }

        // Animate panel scale
        float timer = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        pausePanel.transform.localScale = startScale;

        while (timer < animDuration)
        {
            timer += Time.unscaledDeltaTime;
            pausePanel.transform.localScale = Vector3.Lerp(startScale, endScale, timer / animDuration);
            yield return null;
        }

        pausePanel.transform.localScale = endScale;

        // Freeze game
        Time.timeScale = 0f;
    }

    IEnumerator HidePausePanel()
    {
        Time.timeScale = 1f;

        // Animate panel scale
        float timer = 0f;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        while (timer < animDuration)
        {
            timer += Time.unscaledDeltaTime;
            pausePanel.transform.localScale = Vector3.Lerp(startScale, endScale, timer / animDuration);
            yield return null;
        }

        pausePanel.transform.localScale = endScale;
        pausePanel.SetActive(false);
        isPaused = false;
    }
}
