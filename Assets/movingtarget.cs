using TMPro;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("References")]
    public TextMeshPro targetText;

    [Header("Movement Settings")]
    public float moveSpeed = 1f; // Speed of movement to the left
    public float disableX = -10f; // X position where it disables

    private bool shouldMove = false;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScore > 0)
        {
            if (targetText != null)
                targetText.text = $"Target: {highScore}+";

            shouldMove = true; // enable movement
        }
        else
        {
            // Hide the object if no high score yet
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!shouldMove) return;

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x <= disableX)
        {
            gameObject.SetActive(false);
        }
    }
}
