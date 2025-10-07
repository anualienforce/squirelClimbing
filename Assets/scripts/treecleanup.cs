using UnityEngine;

public class TreeSegmentCleanup : MonoBehaviour
{
    private Camera mainCamera;
    public float extraBuffer = 2f; // extra distance before deletion

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;

        if (transform.position.y < cameraBottom - extraBuffer)
        {
            Destroy(gameObject);
        }
    }
}
