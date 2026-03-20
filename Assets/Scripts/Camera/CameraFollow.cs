using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // The object to follow
    public Vector3 offset;       // Offset from the target
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Apply position
        transform.position = smoothedPosition;

        // Always look at target
        transform.LookAt(target);
    }
}