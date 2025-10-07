using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Reference to the monkey
    public float smoothSpeed = 5f;
    public float yOffset = 2f;  // Camera is slightly above the monkey

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(0f, target.position.y + yOffset, -10f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
