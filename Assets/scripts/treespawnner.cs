using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treeSegmentPrefab;

    public Transform monkeyTransform;

    private float segmentHeight;
    private float lastSpawnY;

  

   


    private void Start()
    {
        if (treeSegmentPrefab == null || monkeyTransform == null)
        {
            Debug.LogError("Assign treeSegmentPrefab and monkeyTransform in inspector.");
            enabled = false;
            return;
        }


        // Assume the tree segment sprite pivot is at center; get height from sprite bounds
        SpriteRenderer sr = treeSegmentPrefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            segmentHeight = sr.bounds.size.y;
        }
        else
        {
            segmentHeight = 2f; // Default height if no sprite renderer found
        }


        lastSpawnY = monkeyTransform.position.y - segmentHeight;
        // Start spawning segments slightly below monkey start position
        SpawnSegment();




    }

    private void Update()
    {
        // Spawn new segments ahead of the monkey to fill the vertical space
        float cameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize;

        while (lastSpawnY < cameraTop + segmentHeight)
        {
            SpawnSegment();
            // CloudsSpawnSegment();
        }

      

    }

    private void SpawnSegment()
    {
        lastSpawnY += segmentHeight;
        Vector3 spawnPos = new Vector3(0f, lastSpawnY, 1f); // Z=1 to appear behind branches and monkey
        Instantiate(treeSegmentPrefab, spawnPos, Quaternion.identity, transform);
    }

}
