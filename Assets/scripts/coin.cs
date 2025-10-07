using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip collectSound;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Destroy coin if it's far below the player
        if (player != null && transform.position.y < player.position.y - 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collectSound)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Update the coin count
            int coins = PlayerPrefs.GetInt("Coins", 0);
            coins += coinValue;
            PlayerPrefs.SetInt("Coins", coins);

            // Update in-game UI
            CoinUI.instance?.UpdateCoinText();

            Destroy(gameObject);
        }
    }
}
