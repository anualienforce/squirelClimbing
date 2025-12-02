using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public GameObject inshopPanel;
    public TextMeshProUGUI coinsText;

    [Header("Power-Up Costs")]
    public int slowPowerUpCost = 10;
    public int invinciblePowerUpCost = 15;
    public int multiplierPowerUpCost = 20;

    private int coins;

    public TMPro.TextMeshProUGUI notEnoughCoinsText;


    private void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinsUI();
        notEnoughCoinsText.gameObject.SetActive(false);
        if (shopPanel != null)
            shopPanel.SetActive(false); // Hide by default

        inshopPanel.transform.localScale = new Vector3(0, 0, 0);
    }



    private void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = "" + coins.ToString();
    }

    // ---------- SHOP PANEL TOGGLE ----------
    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);

            StartCoroutine(AnimShopPanel());
            UpdateCoinsUI(); // refresh coins when opened
        }
    }
    IEnumerator AnimShopPanel()
    {
        inshopPanel.transform.localScale = Vector3.zero;

        float duration = 0.2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            // normalize 0 → 1
            float t = timer / duration;

            // Ease-out effect (optional, better look)
            t = Mathf.SmoothStep(0, 1, t);

            inshopPanel.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            yield return null;
        }

        inshopPanel.transform.localScale = Vector3.one;
        // Time.timeScale = 0f;
    }


    public void CloseShop()
    {
        if (shopPanel != null)
        {
            StartCoroutine(AnimCloseShop());
        }
    }

    IEnumerator AnimCloseShop()
    {
        //  Time.timeScale = 1f; // unpause game

        float duration = 0.2f;
        float timer = 0f;

        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / duration;
            t = Mathf.SmoothStep(0, 1, t); // smooth animation

            inshopPanel.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        inshopPanel.transform.localScale = Vector3.zero;

        shopPanel.SetActive(false); // finally hide panel
    }


    // ---------- BUY FUNCTIONS ----------
    public void BuySlowPowerUp()
    {
        TryBuyPowerUp(slowPowerUpCost, PowerUpType.Slow);
    }

    public void BuyInvinciblePowerUp()
    {
        TryBuyPowerUp(invinciblePowerUpCost, PowerUpType.Invincible);
    }

    public void BuyMultiplierPowerUp()
    {
        TryBuyPowerUp(multiplierPowerUpCost, PowerUpType.Multiplier);
    }

    // ---------- CORE PURCHASE LOGIC ----------
    private void TryBuyPowerUp(int cost, PowerUpType type)
    {
        if (coins < cost)
        {
            Debug.Log("Not enough coins!");
            notEnoughCoinsText.text = "Not enough coins to buy";
            notEnoughCoinsText.gameObject.SetActive(true);
            StartCoroutine(FadeInAndOut());
            return;
        }

        coins -= cost;
        PlayerPrefs.SetInt("Coins", coins);

        switch (type)
        {
            case PowerUpType.Slow:
                int slowCount = PlayerPrefs.GetInt("SlowPowerUp", 0);
                PlayerPrefs.SetInt("SlowPowerUp", slowCount + 1);
                break;
            case PowerUpType.Invincible:
                int invCount = PlayerPrefs.GetInt("InvinciblePowerUp", 0);
                PlayerPrefs.SetInt("InvinciblePowerUp", invCount + 1);
                break;
            case PowerUpType.Multiplier:
                int multCount = PlayerPrefs.GetInt("MultiplierPowerUp", 0);
                PlayerPrefs.SetInt("MultiplierPowerUp", multCount + 1);
                break;
        }

        PlayerPrefs.Save();
        UpdateCoinsUI();
        Debug.Log($"{type} Power-Up Purchased!");
    }

    private IEnumerator FadeInAndOut()
    {
        // Fade IN (0 → 1)
        for (float a = 0f; a <= 1f; a += Time.deltaTime * 2f)  // ~0.5 sec fade
        {
            Color c = notEnoughCoinsText.color;
            notEnoughCoinsText.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        // Hold for 1 second
        yield return new WaitForSeconds(1f);

        // Fade OUT (1 → 0)
        for (float a = 1f; a >= 0f; a -= Time.deltaTime * 2f)
        {
            Color c = notEnoughCoinsText.color;
            notEnoughCoinsText.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        // Turn OFF after fade
        notEnoughCoinsText.gameObject.SetActive(false);
    }



    private enum PowerUpType
    {
        Slow,
        Invincible,
        Multiplier
    }
}
