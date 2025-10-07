using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-Up Prefabs List")]
    public List<GameObject> powerUpPrefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;
    public float yOffset = 8f;
    public float xPositionLeft = -0.52f;
    public float xPositionRight = 0.52f;

    private Transform monkey;

    private void Start()
    {
        monkey = GameObject.FindWithTag("Player")?.transform;
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        // Random wait before the first spawn
        float initialWait = Random.Range(minSpawnInterval, maxSpawnInterval);
        yield return new WaitForSeconds(initialWait);

        while (true)
        {
            if (monkey != null && powerUpPrefabs.Count > 0)
            {
                SpawnRandomPowerUp();
            }

            // Wait random interval for next spawn
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnRandomPowerUp()
    {
        // Randomly choose x position (left or right)
        float xPos = Random.value < 0.5f ? xPositionLeft : xPositionRight;
        Vector3 spawnPos = new Vector3(xPos, monkey.position.y + yOffset, 0);

        // Randomly choose a prefab from the list
        int randomIndex = Random.Range(0, powerUpPrefabs.Count);
        GameObject prefabToSpawn = powerUpPrefabs[randomIndex];

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }
}
