using TMPro;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("References")]
    public TextMeshPro targetText;

    [Header("Horizontal Movement")]
    public float moveSpeed = 1f;       // left movement speed
    public float disableX = -10f;      // X position where it disables

    [Header("Vertical Movement (Balloon)")]
    public float upwardSpeed = 1f;        // starting upward speed
    public float speedIncreaseRate = .1f; // how fast it increases per second
    public float maxUpwardSpeed = 10f;    // optional cap

    private bool shouldMove = false;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Always start from 5, then +3 steps: 5+, 8+, 11+, ...
        int target = 50;

        while (highScore > target)
        {
            target += 30;
        }

        if (targetText != null)
            targetText.text = $"Target: {target}+";

        shouldMove = true;
    }



    private void Update()
    {
        if (!MonkeyController.checkTap || !shouldMove)
            return;

        // Increase balloon's upward speed gradually
        upwardSpeed += speedIncreaseRate * Time.deltaTime;
        upwardSpeed = Mathf.Min(upwardSpeed, maxUpwardSpeed);

        // Move left
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Move upward with its own speed
        transform.position += Vector3.up * upwardSpeed * Time.deltaTime;

        // Disable when it goes off screen
        if (transform.position.x <= disableX)
        {
            gameObject.SetActive(false);
        }
    }
}
