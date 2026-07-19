using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float turnSpeed = 12f;
    public float gravity = -20f;
    public Transform cameraTransform;

    CharacterController controller;
    Vector3 verticalVelocity;
    CheckpointGameManager gameManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<CheckpointGameManager>();
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (gameManager != null && gameManager.IsGameEnded)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 move = Vector3.zero;
        if (input.magnitude > 0.1f)
        {
            Vector3 cameraForward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
            Vector3 cameraRight = cameraTransform != null ? cameraTransform.right : Vector3.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            move = cameraForward * input.z + cameraRight * input.x;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move((move * moveSpeed + verticalVelocity) * Time.deltaTime);
    }
}
