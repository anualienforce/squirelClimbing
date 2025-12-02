using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip collectSound;
    public GameObject collectVFX;
    private Transform player;

    // Distance for auto collect (adjust as needed)
    public float collectDistance = 0.01f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        // Auto collect if player gets close
        // Auto collect only if player collider is OFF
        Collider2D playerCol = player.GetComponent<Collider2D>();

        if (playerCol != null && playerCol.enabled == false)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance <= collectDistance)
            {
                CollectCoin();
            }
        }


        // Destroy coin if far below player
        if (transform.position.y < player.position.y - 10f)
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
        if (collision.CompareTag("Player") || collision.CompareTag("Invincible"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        // Sound
        if (collectSound)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // VFX
        if (collectVFX != null)
        {
            GameObject vfx = Instantiate(collectVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

        // Coins add
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coins += coinValue;
        PlayerPrefs.SetInt("Coins", coins);

        // UI update
        CoinUI.instance?.UpdateCoinText();

        // Destroy coin
        Destroy(gameObject);
    }
}
