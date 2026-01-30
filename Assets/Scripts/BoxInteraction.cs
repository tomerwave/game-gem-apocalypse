using UnityEngine;
using UnityEngine.UI;
using EasyPeasyFirstPersonController;

public class BoxInteraction : MonoBehaviour
{
    [Header("Detection Settings")]
    public float interactionRange = 2.5f;
    public LayerMask interactableLayer;
    public string interactableTag = "Interactable";

    [Header("UI Reference")]
    public Text interactionPrompt;

    [Header("Player Movement Modifier")]
    public float holdingSpeedMultiplier = 0.5f;

    private InputManagerOld input;
    private FirstPersonController playerController;
    private InteractableBox currentBox;
    private InteractableBox nearbyBox;
    private float originalWalkSpeed;
    private float originalSprintSpeed;

    void Start()
    {
        input = GetComponent<InputManagerOld>();
        playerController = GetComponent<FirstPersonController>();

        if (playerController != null)
        {
            originalWalkSpeed = playerController.walkSpeed;
            originalSprintSpeed = playerController.sprintSpeed;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        DetectNearbyBox();
        HandleInteraction();
        UpdateUI();
    }

    void DetectNearbyBox()
    {
        if (currentBox != null) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

        InteractableBox closest = null;
        float closestDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!col.CompareTag(interactableTag)) continue;

            InteractableBox box = col.GetComponent<InteractableBox>();
            if (box == null) continue;

            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = box;
            }
        }

        nearbyBox = closest;
    }

    void HandleInteraction()
    {
        if (input.interact)
        {
            if (currentBox == null && nearbyBox != null)
            {
                StartHolding(nearbyBox);
            }

            if (currentBox != null)
            {
                // Get current speed based on whether player is sprinting
                bool isSprinting = input.sprint && input.moveInput.y > 0;
                float currentSpeed = isSprinting ? playerController.sprintSpeed : playerController.walkSpeed;

                currentBox.MoveWithPlayer(input.moveInput, transform, currentSpeed);

                float distance = Vector3.Distance(transform.position, currentBox.transform.position);

                Debug.Log($"[BoxInteraction] Distance: {distance:F2} | MaxDist: {currentBox.maxDistance} | Speed: {currentSpeed:F2} | Sprinting: {isSprinting}");

                if (distance > currentBox.maxDistance)
                {
                    Debug.LogWarning($"[BoxInteraction] MAX DISTANCE EXCEEDED! Dropping box. Distance: {distance:F2}");
                    StopHolding();
                }
            }
        }
        else
        {
            if (currentBox != null)
            {
                StopHolding();
            }
        }
    }

    void StartHolding(InteractableBox box)
    {
        currentBox = box;

        if (playerController != null)
        {
            playerController.walkSpeed = originalWalkSpeed * holdingSpeedMultiplier;
            playerController.sprintSpeed = originalSprintSpeed * holdingSpeedMultiplier;
        }

        // Pass the player's current speed to the box so they move together
        float currentSpeed = playerController != null ? playerController.walkSpeed : 1.5f;
        currentBox.StartHolding(transform, currentSpeed);
    }

    void StopHolding()
    {
        if (currentBox != null)
        {
            currentBox.StopHolding();
            currentBox = null;
        }

        if (playerController != null)
        {
            playerController.walkSpeed = originalWalkSpeed;
            playerController.sprintSpeed = originalSprintSpeed;
        }
    }

    void UpdateUI()
    {
        if (interactionPrompt == null) return;

        if (currentBox != null)
        {
            interactionPrompt.text = "Release E to drop";
            interactionPrompt.gameObject.SetActive(true);
        }
        else if (nearbyBox != null)
        {
            interactionPrompt.text = "Hold E to push/pull";
            interactionPrompt.gameObject.SetActive(true);
        }
        else
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
