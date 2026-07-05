using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    const float Distance = 8f;
    const float Height = 4f;
    const float SmoothSpeed = 6f;
    const float DragSensitivity = 0.45f;

    float lastMouseX;
    float yaw;
    bool wasDragging;

    void Start()
    {
        if (FindAnyObjectByType<AudioListener>() == null)
        {
            gameObject.AddComponent<AudioListener>();
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        bool isDragging = Input.GetMouseButton(1) || Input.GetMouseButton(0);
        float mouseX = Input.mousePosition.x;

        if (isDragging && wasDragging)
        {
            yaw += (mouseX - lastMouseX) * DragSensitivity;
        }

        if (isDragging)
        {
            lastMouseX = mouseX;
        }

        wasDragging = isDragging;

        if (!isDragging)
        {
            lastMouseX = mouseX;
        }

        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, Height, -Distance);
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, SmoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
