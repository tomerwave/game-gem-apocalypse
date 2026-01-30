using UnityEngine;
using EasyPeasyFirstPersonController;

public class SwitchScript : MonoBehaviour
{
    public bool switchValue;
    public GameObject[] interacteWith;
    public float interactionRange = 2f;

    private Transform player;
    private InputManagerOld input;

    private Camera playerCamera;
    private bool canInteract;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCamera = playerObj.GetComponentInChildren<Camera>();
        }
        input = player.GetComponent<InputManagerOld>();
    }

    void Update()
    {
        if (player == null || playerCamera == null) return;

        canInteract = false;

        // Check if player is in range
        bool interactPressed = input.interact;
            
        
        float distance = Vector3.Distance(transform.position, player.position);
        bool playerInRange = distance <= interactionRange;

        // Check if player is looking at this object
        if (playerInRange)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
            {
                Debug.Log($"[Switch] Raycast hit: {hit.transform.name}");
                if (hit.transform == transform)
                {
                    canInteract = true;
                }
            }
            else
            {
                Debug.Log("[Switch] Raycast hit nothing");
            }
        
        }
        

        if (canInteract && interactPressed)
        {
            if (InteractionPromptUI.Instance != null)
            {
                string action = switchValue ? "Turn Off" : "Turn On";
                InteractionPromptUI.Instance.ShowPrompt($"Press E to {action}");
            }

            switchValue = !switchValue;
            foreach (GameObject item in interacteWith)
                item?.GetComponent<SwitchInteractive>()?.DoStuff(switchValue);
            GetComponent<Renderer>().material.color = switchValue ? Color.green : Color.blue;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
