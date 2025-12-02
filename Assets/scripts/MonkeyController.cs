using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MonkeyController : MonoBehaviour
{
    public float climbSpeed = 3f;
    private bool climbingRight = true;
    private bool isGameOver = false;
    public float maxClimbSpeed = 7f;      // Maximum speed cap
    public float speedIncreaseRate = 0.01f; // Units per second to increase
    public GameObject gameOverPanel;
    public TextMeshProUGUI currentScoreText;   // Reference to score UI during gameplay
    public TextMeshProUGUI gameOverCurrentScoreText;  // TMP text in Game Over Panel for current score
    public TextMeshProUGUI gameOverHighScoreText;     // TMP text in Game Over Panel for high score

    private int score = 0;
    private float scoreTimer = 0f;
    private float scoreMultiplier = 1f;
    private float baseScoreInterval = 1f; // normal 1 second per score increment

    public AudioSource audioSource;
    public AudioClip flipSound;

    [SerializeField] private AudioClip powerUpSound;
    [SerializeField][Range(0f, 1f)] private float powerUpVolume = 1f;
    private static AudioSource uiAudioSource;


    [SerializeField] Animator animator;
    [SerializeField] GameObject dizzyEffect;
    [SerializeField] GameObject pandaSadface;
    public AudioClip dieSound;
    public AudioClip taptoplaySound;

    public static bool checkTap;
    private void Awake()
    {
        if (uiAudioSource == null)
        {
            GameObject audioObj = new GameObject("UIAudioSource");
            uiAudioSource = audioObj.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.spatialBlend = 0f; // 2D sound
            DontDestroyOnLoad(audioObj);
            animator = GetComponent<Animator>();


        }
    }

    private void HandlePowerUpPickup(GameObject powerUp, string type)
    {
        Debug.Log($"{type} power-up collected");
        Destroy(powerUp);

        // Play sound
        if (powerUpSound != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(powerUpSound, powerUpVolume);
    }
    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }

    void Start()
    {
        checkTap = false;
        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        if (musicOn)
        {
            BackgroundMusic.Instance?.PlayMusic();
        }
        else
        {
            BackgroundMusic.Instance?.StopMusic();
        }
        
        animator.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isGameOver) return;
        // Move monkey upward constantly

        if (checkTap)
        {
            scoreTimer += Time.deltaTime * scoreMultiplier;  // multiply deltaTime
            if (scoreTimer >= baseScoreInterval)
            {
                score++;
                scoreTimer -= baseScoreInterval;
                UpdateScoreUI();
            }
        }


        if (checkTap)
        {
            climbSpeed = Mathf.Min(climbSpeed + speedIncreaseRate * Time.deltaTime, maxClimbSpeed);

            transform.Translate(Vector2.up * climbSpeed * Time.deltaTime);

            climbSpeed = Mathf.Min(climbSpeed + speedIncreaseRate * Time.deltaTime, maxClimbSpeed);
            animator.speed = 1f + (climbSpeed - 3f) / 280f;  // start normal, only increase later
            //  Debug.Log(animator.speed);

        }



    }
    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "" + score.ToString();
        }
    }

    public void Flip()
    {
        if (checkTap)
        {
            climbingRight = !climbingRight;
            Vector3 scale = transform.localScale;
            scale.x = climbingRight ? 0.7f : -0.7f;
            transform.localScale = scale;


            // Shift monkey left or right on flip for climbing sides
            float xPos = climbingRight ? 0.37f : -0.37f;
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

            // 👇 Play flip sound
            if (audioSource != null && flipSound != null)
                audioSource.PlayOneShot(flipSound);

            // 👇 Subtle camera shake when flip happens
            if (CameraShake.Instance != null)
                CameraShake.Instance.TriggerShake(0.02f, 0.01f);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Branch"))
        {
            isGameOver = true;
            Debug.Log("Game Over!");
            dizzyEffect.SetActive(true);
            pandaSadface.SetActive(true);
            animator.SetBool("IsGoUp", false);
            uiAudioSource.PlayOneShot(dieSound, powerUpVolume);
            // Save current score to PlayerPrefs
            PlayerPrefs.SetInt("LastScore", score);

            // Update high score if necessary
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
                highScore = score;
            }
            PlayerPrefs.Save();

            // Update Game Over panel UI texts
            if (gameOverCurrentScoreText != null)
                gameOverCurrentScoreText.text = "Score: " + score.ToString();
            if (gameOverHighScoreText != null)
                gameOverHighScoreText.text = "High Score: " + highScore.ToString();

            if (gameOverPanel != null)
                StartCoroutine(gameoverwait());

        }
        if (collision.CompareTag("SlowPowerUp"))
        {
            HandlePowerUpPickup(collision.gameObject, "slow");
            PowerUpManager.Instance.AddSlowPowerUp();
        }
        else if (collision.CompareTag("Invincible"))
        {
            HandlePowerUpPickup(collision.gameObject, "invincible");
            PowerUpManager.Instance.AddInvinciblePowerUp();
        }
        else if (collision.CompareTag("ScoreMultiplier"))
        {
            HandlePowerUpPickup(collision.gameObject, "multiplier");
            PowerUpManager.Instance.AddMultiplierPowerUp();
        }

    }

    IEnumerator gameoverwait()
    {
        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.StopMusic();
        yield return new WaitForSeconds(1.5f);
        gameOverPanel.SetActive(true);

    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Taptoplay()
    {
        checkTap = true;
        uiAudioSource.PlayOneShot(taptoplaySound, powerUpVolume);
        animator.SetBool("IsGoUp", true);
    }
}
