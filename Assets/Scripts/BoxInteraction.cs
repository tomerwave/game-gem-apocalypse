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
    private Camera playerCamera;
    private InteractableBox currentBox;
    private InteractableBox lookingAtBox;
    private float originalWalkSpeed;
    private float originalSprintSpeed;
    private float timeCooldown=0.5f;
    private float timeLatest=0;

    void Start()
    {
        input = GetComponent<InputManagerOld>();
        playerController = GetComponent<FirstPersonController>();
        playerCamera = GetComponentInChildren<Camera>();

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
        lookingAtBox = null;

        if (playerCamera == null) return;

        // Raycast from camera to detect what player is looking at
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                InteractableBox box = hit.collider.GetComponent<InteractableBox>();
                if (box != null)
                {
                    lookingAtBox = box;
                }
            }
        }
    }

    bool IsLookingAtCurrentBox()
    {
        if (currentBox == null || playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange + currentBox.maxDistance, interactableLayer))
        {
            return hit.collider.GetComponent<InteractableBox>() == currentBox;
        }
        return false;
    }

    void HandleInteraction()
    {
        // Toggle interaction on E press (not hold)
        if (input.interact && Time.time >= timeLatest+timeCooldown)
        {
            
            if (currentBox == null && lookingAtBox != null)
            {
                // Start pushing
                timeLatest = Time.time;

                StartHolding(lookingAtBox);
                input.hold = true;
            }
            else if (currentBox != null)
            {
                timeLatest = Time.time;
                // Stop pushing
                StopHolding();
                input.hold = false;
            }
        }

        // Continue moving box if holding one
        if (currentBox != null)
        {
            // Check if still looking at the box
            if (!IsLookingAtCurrentBox())
            {
                StopHolding();
                input.hold = false;
                return;
            }

            // Get current speed based on whether player is sprinting
            bool isSprinting = input.sprint && input.moveInput.y > 0;
            float currentSpeed = isSprinting ? playerController.sprintSpeed : playerController.walkSpeed;

            currentBox.MoveWithPlayer(input.moveInput, transform, currentSpeed);

            float distance = Vector3.Distance(transform.position, currentBox.transform.position);

            if (distance > currentBox.maxDistance)
            {
                StopHolding();
                input.hold = false;
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
        // Use the centralized interaction prompt system
        if (InteractionPromptUI.Instance != null)
        {
            if (currentBox != null)
            {
                InteractionPromptUI.Instance.ShowPrompt("Press E to Release");
            }
            else if (lookingAtBox != null)
            {
                InteractionPromptUI.Instance.ShowPrompt("Press E to Push");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
