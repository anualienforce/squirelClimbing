using TMPro;
using UnityEngine;
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
   

    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }

    
    private void Update()
    {
        if (isGameOver) return;
        // Move monkey upward constantly
        scoreTimer += Time.deltaTime * scoreMultiplier;  // multiply deltaTime
        if (scoreTimer >= baseScoreInterval)
        {
            score++;
            scoreTimer -= baseScoreInterval;
            UpdateScoreUI();
        }

        climbSpeed = Mathf.Min(climbSpeed + speedIncreaseRate * Time.deltaTime, maxClimbSpeed);

        transform.Translate(Vector2.up * climbSpeed * Time.deltaTime);

        // Flip direction on screen tap or mouse click
       
    }
    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + score.ToString();
        }
    }

    public void Flip()
    {
        climbingRight = !climbingRight;
        Vector3 scale = transform.localScale;
        scale.x = climbingRight ? 0.5f : -0.5f;
        transform.localScale = scale;

        // Shift monkey left or right on flip for climbing sides
        float xPos = climbingRight ? .5f : -.5f;
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Branch"))
        {
            isGameOver = true;
            Debug.Log("Game Over!");

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
                gameOverPanel.SetActive(true);
        }
        if (collision.CompareTag("SlowPowerUp"))
        {
            Debug.Log("slowpup");
            Destroy(collision.gameObject);
            PowerUpManager.Instance.AddSlowPowerUp();
        }
        if (collision.CompareTag("Invincible"))
        {
            Destroy(collision.gameObject);
            PowerUpManager.Instance.AddInvinciblePowerUp();
        }
        if (collision.CompareTag("ScoreMultiplier"))
        {
            Destroy(collision.gameObject);
            PowerUpManager.Instance.AddMultiplierPowerUp();
        }

    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
