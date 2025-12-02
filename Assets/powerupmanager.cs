using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("Slow Power-Up")]
    public TextMeshProUGUI slowPowerUpText;
    public Button slowPowerUpButton;
    public GameObject timeSlowEffPrefab;

    [Header("Invincible Power-Up")]
    public TextMeshProUGUI invinciblePowerUpText;
    public Button invinciblePowerUpButton;
    public GameObject InvincibleEffPrefab;

    [Header("Score Multiplier Power-Up")]
    public TextMeshProUGUI multiplierPowerUpText;
    public Button multiplierPowerUpButton;
    public GameObject ScoreSpeedUpEffPrefab;


    private int slowPowerUpCount = 0;
    private int invinciblePowerUpCount = 0;
    private int multiplierPowerUpCount = 0;

    private const string SlowKey = "SlowPowerUp";
    private const string InvincibleKey = "InvinciblePowerUp";
    private const string MultiplierKey = "MultiplierPowerUp";

    private MonkeyController monkeyController;

    [Header("tap to play")]
    public GameObject taptoPlayBtn;
    public Image taptoPlayBack;
    public TMP_Text taptoPlayText;
    public Transform PandaPos;

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


    void Update()
    {

        float t = Mathf.PingPong(Time.time * 2, 1f); // 0→1→0
        float scale = Mathf.Lerp(3.2f, 3.5f, t);
        taptoPlayBtn.transform.localScale = new Vector3(scale, scale, scale);

        if (MonkeyController.checkTap)
        {
            taptoPlayBack.raycastTarget = false;
            Color c = taptoPlayBack.color;
            c.a = Mathf.Lerp(c.a, 0f, 2 * Time.deltaTime);
            taptoPlayBack.color = c;
            if (c.a <= 0.05f)
            {
                taptoPlayBack.gameObject.SetActive(false);

            }
        }


        if (MonkeyController.checkTap)
        {
            taptoPlayText.raycastTarget = false;

            Color c = taptoPlayText.color;
            c.a = Mathf.Lerp(c.a, 0f, 4 * Time.deltaTime);
            taptoPlayText.color = c;

            if (c.a <= 0.05f)
            {
                taptoPlayText.gameObject.SetActive(false);
            }
        }
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

        if (timeSlowEffPrefab != null)
        {
            GameObject vfx = Instantiate(timeSlowEffPrefab, PandaPos.position, Quaternion.identity);
            Destroy(vfx, 2f); // optional: destroy after 2 seconds

            vfx.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        }
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

        if (InvincibleEffPrefab != null)
        {
            GameObject vfx = Instantiate(InvincibleEffPrefab, PandaPos.position, Quaternion.identity);
            Destroy(vfx, 2f); // optional: destroy after 2 seconds
            vfx.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        }

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

        if (ScoreSpeedUpEffPrefab != null)
        {
            GameObject vfx = Instantiate(ScoreSpeedUpEffPrefab, PandaPos.position, Quaternion.identity);
            Destroy(vfx, 2f); // optional: destroy after 2 seconds
            vfx.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
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
