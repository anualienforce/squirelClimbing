using UnityEngine;

public class BranchSpawner : MonoBehaviour
{
    public GameObject branchLeftPrefab;
    public GameObject branchRightPrefab;
    public float spawnInterval = 0.5f;
    private float timer = 0f;
    private float lastSpawnY = 4f;  // Start spawning branches at Y = 4

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
        lastSpawnY += Random.Range(2f, 4f);

        bool spawnLeft = Random.value > 0.5f;

        Vector3 spawnPos = spawnLeft ?
            new Vector3(-1f, lastSpawnY, 0f) :
            new Vector3(1f, lastSpawnY, 0f);

        Instantiate(spawnLeft ? branchLeftPrefab : branchRightPrefab, spawnPos, Quaternion.identity);
    }
}
