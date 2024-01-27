using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float distanceFromPlayer = 2.1f;
    public float heightAbovePlayer = 4.5f;
    public float cameraFollowSpeed = 5.0f;
    public float cameraRotateSpeed = 5.0f;
    public float PlayerYOffset = 2.0f;

    private Vector3 cameraOffset;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned to CameraFollow script.");
            return;
        }

        // Calculate the initial offset at start
        cameraOffset = new Vector3(0, heightAbovePlayer, - distanceFromPlayer);
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate the desired position based on the player's position and the offset
            Vector3 desiredPosition = playerTransform.position + cameraOffset;

            // Smoothly move the camera to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraFollowSpeed * Time.deltaTime);

            // Calculate target rotation to look at the player
            Vector3 AdjustedPlayerTransform = new Vector3(playerTransform.position.x, playerTransform.position.y + PlayerYOffset, playerTransform.position.z + 1);
            Quaternion targetRotation = Quaternion.LookRotation(AdjustedPlayerTransform - transform.position);

            // Adjust target rotation based on xRotationAdjustment
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            // Smoothly rotate the camera to the adjusted target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotateSpeed * Time.deltaTime);
        }
    }
}
