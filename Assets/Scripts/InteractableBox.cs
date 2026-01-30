using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableBox : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How smoothly the box accelerates/decelerates")]
    public float smoothing = 10f;
    public float maxDistance = 2f;

    private Rigidbody rb;
    private Transform player;
    private bool isBeingHeld;
    private Vector3 currentVelocity;
    private float playerSpeed;

    public bool IsBeingHeld => isBeingHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (!gameObject.CompareTag("Interactable"))
        {
            Debug.LogWarning($"InteractableBox on {gameObject.name} should have the 'Interactable' tag!");
        }
    }

    public void StartHolding(Transform playerTransform, float speed)
    {
        player = playerTransform;
        playerSpeed = speed;
        isBeingHeld = true;
        currentVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    public void StopHolding()
    {
        isBeingHeld = false;
        player = null;
        currentVelocity = Vector3.zero;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.linearVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!isBeingHeld) return;

        // Apply smooth velocity
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }

    public void MoveWithPlayer(Vector2 moveInput, Transform playerTransform, float currentPlayerSpeed)
    {
        if (!isBeingHeld) return;

        Vector3 moveDirection = playerTransform.forward * moveInput.y + playerTransform.right * moveInput.x;
        moveDirection.y = 0;

        // No input = stop immediately, with input = move at player speed
        if (moveDirection.magnitude > 0.1f)
        {
            currentVelocity = moveDirection.normalized * currentPlayerSpeed;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }

        Debug.Log($"Box Position: {transform.position} | Velocity: {currentVelocity} | Speed: {currentPlayerSpeed}");
    }
}
