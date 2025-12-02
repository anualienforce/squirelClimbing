using UnityEngine;

public class BranchSpawner : MonoBehaviour
{
    [Header("Branch Settings")]
    public GameObject branchLeftPrefab;
    public GameObject branchRightPrefab;
    public float spawnInterval = 0.5f;

    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public int minCoinGroups = 2;
    public int maxCoinGroups = 4;
    public float verticalSpacing = 0.6f;
    public float coinXLeft = -0.52f;
    public float coinXRight = 0.52f;

    private float timer = 0f;
    private float lastSpawnY = 4f;
    private float prevBranchY = 0f;
    private bool prevBranchLeft;

    private void Start()
    {
        prevBranchLeft = Random.value > 0.5f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBranch();
            timer = 0f;
        }
    }

    void SpawnBranch()
    {
        prevBranchY = lastSpawnY;

        lastSpawnY += Random.Range(1.6f, 3.2f);
        bool spawnLeft = Random.value > 0.5f;

        Vector3 spawnPos = spawnLeft ? new Vector3(-1f, lastSpawnY, 0f) : new Vector3(1f, lastSpawnY, 0f);
        Instantiate(spawnLeft ? branchLeftPrefab : branchRightPrefab, spawnPos, Quaternion.identity);

        // 70% chance to spawn coins between this and the previous branch
        if (coinPrefab != null && Random.value < 0.8f)
        {
            // --- New logic: Most of the time coins match the *current* branch side ---
            bool matchCurrentBranch = Random.value > 0.5f; // 70% chance to match current side
            float coinX;

            if (matchCurrentBranch)
                coinX = spawnLeft ? coinXLeft : coinXRight;
            else
                coinX = spawnLeft ? coinXRight : coinXLeft;

            SpawnCoinsBetweenBranches(prevBranchY, lastSpawnY, coinX);
        }

        prevBranchLeft = spawnLeft;
    }

    void SpawnCoinsBetweenBranches(float lowerY, float upperY, float coinX)
    {
        if (upperY - lowerY < 1f) return;


        int groupCount = Random.Range(minCoinGroups, maxCoinGroups + 1);
        float sectionHeight = (upperY - lowerY) / (groupCount + 1);

        for (int i = 1; i <= groupCount; i++)
        {
            float groupCenterY = lowerY + (sectionHeight * i);
            int coinsInThisGroup = Mathf.Clamp(Mathf.FloorToInt((upperY - lowerY) / verticalSpacing), 2, 4);


            for (int j = 0; j < coinsInThisGroup; j++)
            {
                float yPos = groupCenterY + (j - (coinsInThisGroup - 1) / 2f) * verticalSpacing;
                Vector3 spawnPos = new Vector3(coinX, yPos, 0f);
                Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
