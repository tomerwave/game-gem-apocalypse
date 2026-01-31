using UnityEngine;
using EasyPeasyFirstPersonController;

public class SwitchScript : MonoBehaviour
{
    public bool switchValue;
    public GameObject interacteWith;
    public float interactionRange = 2f;

    private Transform player;
    private InputManagerOld input;

    private Camera playerCamera;
    private bool canInteract;
    private float interactCooldown = 1.6f;
    private float lastInteractTime = 0f;


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
        

        if (canInteract && interactPressed && Time.time>lastInteractTime+interactCooldown)
        {
            lastInteractTime = Time.time;
            string action = switchValue ? "Turn Off" : "Turn On";

            if(gameObject.tag == "Key")
            {
                action = "Take";
            }
            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.ShowPrompt($"Press E to {action}");
            }

            switchValue = !switchValue;
            interacteWith?.GetComponent<SwitchInteractive>()?.DoStuff(switchValue);
            if(gameObject.tag == "Key")
            {
                Destroy(gameObject);
            }
            else{
                GetComponent<Renderer>().material.color = switchValue ? Color.green : Color.blue;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
