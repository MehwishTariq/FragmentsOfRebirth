using UnityEngine;

[ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;// {get;set;}

    [Header("Camera Settings")]
    public Vector3 baseOffset = new Vector3(0f, 3f, -10f);
    public float smoothTime = 0.2f;
    public bool followY = false;

    [Header("Cinematic Look Ahead")]
    public float lookAheadDistance = 3f;   // how far to look ahead
    public float lookAheadSpeed = 2f;      // how fast to transition
    public float lookAheadReturnSpeed = 2f;

    private Vector3 velocity = Vector3.zero;
    private float currentLookAhead = 0f;
    private float lookAheadTarget = 0f;
    private Vector3 lastTargetPosition;

    private void Start()
    {
        if (target)
            lastTargetPosition = target.position;
    }

    private void LateUpdate()
    {
        if (!target) return;

        // Compute movement direction
        float deltaX = target.position.x - lastTargetPosition.x;
        lastTargetPosition = target.position;

        // Determine look-ahead target based on direction
        if (Mathf.Abs(deltaX) > 0.05f)
        {
            lookAheadTarget = Mathf.Sign(deltaX) * lookAheadDistance;
        }
        else
        {
            lookAheadTarget = 0f;
        }

        // Smooth transition of look-ahead offset
        currentLookAhead = Mathf.Lerp(currentLookAhead, lookAheadTarget, 
            Time.deltaTime * (lookAheadTarget == 0 ? lookAheadReturnSpeed : lookAheadSpeed));

        // Final desired camera position
        Vector3 desiredPosition = new Vector3(
            target.position.x + baseOffset.x + currentLookAhead,
            followY ? target.position.y + baseOffset.y : transform.position.y,
            baseOffset.z
        );

        // Smooth move
        transform.position = Application.isPlaying
            ? Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime)
            : desiredPosition;

        transform.rotation = Quaternion.Euler(10f, 0f, 0f);
    }
}
