using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("Slow Power-Up")]
    public TextMeshProUGUI slowPowerUpText;
    public Button slowPowerUpButton;

    [Header("Invincible Power-Up")]
    public TextMeshProUGUI invinciblePowerUpText;
    public Button invinciblePowerUpButton;

    [Header("Score Multiplier Power-Up")]
    public TextMeshProUGUI multiplierPowerUpText;
    public Button multiplierPowerUpButton;

    private int slowPowerUpCount = 0;
    private int invinciblePowerUpCount = 0;
    private int multiplierPowerUpCount = 0;

    private const string SlowKey = "SlowPowerUp";
    private const string InvincibleKey = "InvinciblePowerUp";
    private const string MultiplierKey = "MultiplierPowerUp";

    private MonkeyController monkeyController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        slowPowerUpCount = PlayerPrefs.GetInt(SlowKey, 0);
        invinciblePowerUpCount = PlayerPrefs.GetInt(InvincibleKey, 0);
        multiplierPowerUpCount = PlayerPrefs.GetInt(MultiplierKey, 0);
        monkeyController = FindObjectOfType<MonkeyController>();
        UpdateAllUI();
    }

    // ---------- SLOW POWER-UP ----------
    public void AddSlowPowerUp()
    {
        slowPowerUpCount++;
        PlayerPrefs.SetInt(SlowKey, slowPowerUpCount);
        PlayerPrefs.Save();
        UpdateAllUI();
    }

    public void UseSlowPowerUp()
    {
        if (slowPowerUpCount <= 0 || (slowPowerUpButton != null && !slowPowerUpButton.interactable)) return;

        slowPowerUpCount--;
        PlayerPrefs.SetInt(SlowKey, slowPowerUpCount);
        PlayerPrefs.Save();
        UpdateAllUI();
        if (monkeyController == null) monkeyController = FindObjectOfType<MonkeyController>();
        if (monkeyController != null) StartCoroutine(ApplySlowEffect());
    }

    private IEnumerator ApplySlowEffect()
    {
        if (slowPowerUpButton != null)
            slowPowerUpButton.interactable = false;

        float originalSpeed = monkeyController.climbSpeed;
        monkeyController.climbSpeed /= 2f;

        yield return new WaitForSeconds(5f);

        monkeyController.climbSpeed = originalSpeed;
        if (slowPowerUpButton != null)
            slowPowerUpButton.interactable = true;
    }

    // ---------- INVINCIBLE POWER-UP ----------
    public void AddInvinciblePowerUp()
    {
        invinciblePowerUpCount++;
        PlayerPrefs.SetInt(InvincibleKey, invinciblePowerUpCount);
        PlayerPrefs.Save();
        UpdateAllUI();
    }

    public void UseInvinciblePowerUp()
    {
        if (invinciblePowerUpCount <= 0 || (invinciblePowerUpButton != null && !invinciblePowerUpButton.interactable)) return;

        invinciblePowerUpCount--;
        PlayerPrefs.SetInt(InvincibleKey, invinciblePowerUpCount);
        PlayerPrefs.Save();
        UpdateAllUI();

        if (monkeyController == null) monkeyController = FindObjectOfType<MonkeyController>();
        if (monkeyController != null) StartCoroutine(ApplyInvincibleEffect());
    }

    private IEnumerator ApplyInvincibleEffect()
    {
        if (invinciblePowerUpButton != null)
            invinciblePowerUpButton.interactable = false;

        var collider = monkeyController.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(5f);
            collider.enabled = true;
        }

        if (invinciblePowerUpButton != null)
            invinciblePowerUpButton.interactable = true;
    }

    // ---------- MULTIPLIER POWER-UP ----------
    public void AddMultiplierPowerUp()
    {
        multiplierPowerUpCount++;
        PlayerPrefs.SetInt(MultiplierKey, multiplierPowerUpCount);
        PlayerPrefs.Save();
        UpdateAllUI();
    }

    public void UseMultiplierPowerUp()
    {
        if (multiplierPowerUpCount <= 0 || (multiplierPowerUpButton != null && !multiplierPowerUpButton.interactable)) return;

        multiplierPowerUpCount--;
        PlayerPrefs.SetInt(MultiplierKey, multiplierPowerUpCount);
        PlayerPrefs.Save();
        UpdateAllUI();

        if (monkeyController == null) monkeyController = FindObjectOfType<MonkeyController>();
        if (monkeyController != null) StartCoroutine(ApplyMultiplierEffect());
    }

    private IEnumerator ApplyMultiplierEffect()
    {
        if (multiplierPowerUpButton != null)
            multiplierPowerUpButton.interactable = false;

        monkeyController.SetScoreMultiplier(2f); // activate 2x
        yield return new WaitForSeconds(5f);
        monkeyController.SetScoreMultiplier(1f); // revert to normal

        if (multiplierPowerUpButton != null)
            multiplierPowerUpButton.interactable = true;
    }

    // ---------- HELPERS ----------
    private void UpdateAllUI()
    {
        if (slowPowerUpText != null)
            slowPowerUpText.text = slowPowerUpCount.ToString();

        if (invinciblePowerUpText != null)
            invinciblePowerUpText.text = invinciblePowerUpCount.ToString();

        if (multiplierPowerUpText != null)
            multiplierPowerUpText.text = multiplierPowerUpCount.ToString();
    }
}
