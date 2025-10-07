using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public float spawnInterval = 2f;
    public float yOffset = 8f;
    public int maxCoinsInGroup = 4;
    public float verticalSpacing = 0.6f; // space between coins in the same group
    public float groupSpacing = 3f;      // NEW: extra vertical space between groups

    private Transform monkey;
    private float lastSpawnY = 0f;       // to track where last group was spawned

    private void Start()
    {
        monkey = GameObject.FindWithTag("Player")?.transform;
        if (monkey != null)
        {
            lastSpawnY = monkey.position.y;
            InvokeRepeating(nameof(SpawnCoinGroup), 1f, spawnInterval);
        }
    }

    private void SpawnCoinGroup()
    {
        if (monkey == null || coinPrefab == null) return;

        // Only spawn if the monkey has moved enough upward
        if (monkey.position.y < lastSpawnY + groupSpacing) return;

        // Randomly choose left or right side
        float xPos = Random.value < 0.5f ? -0.52f : 0.52f;

        // Randomly decide how many coins to spawn (1 to max)
        int coinCount = Random.Range(1, maxCoinsInGroup + 1);

        // Spawn them in a vertical line
        for (int i = 0; i < coinCount; i++)
        {
            float yPos = monkey.position.y + yOffset + (i * verticalSpacing);
            Vector3 spawnPos = new Vector3(xPos, yPos, 0);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }

        // Update the last spawn position
        lastSpawnY = monkey.position.y + yOffset;
    }
}
