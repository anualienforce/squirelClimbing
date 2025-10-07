using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public TextMeshProUGUI coinsText;

    [Header("Power-Up Costs")]
    public int slowPowerUpCost = 10;
    public int invinciblePowerUpCost = 15;
    public int multiplierPowerUpCost = 20;

    private int coins;

    private void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinsUI();

        if (shopPanel != null)
            shopPanel.SetActive(false); // Hide by default
    }

    private void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + coins.ToString();
    }

    // ---------- SHOP PANEL TOGGLE ----------
    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            UpdateCoinsUI(); // refresh coins when opened
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
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


    private enum PowerUpType
    {
        Slow,
        Invincible,
        Multiplier
    }
}
