using UnityEngine;

public class BranchCleanup : MonoBehaviour
{
    private Camera mainCamera;
    public float extraBuffer = 2f; // How far below the camera before deletion

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        // Get camera's bottom Y in world space
        float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;

        // If branch is far below the bottom of the camera view, destroy it
        if (transform.position.y < cameraBottom - extraBuffer)
        {
            Destroy(gameObject);
        }
    }
}
