using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public static CoinUI instance;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        if (coinText != null)
            coinText.text = "Coins: " + coins;
    }
}
