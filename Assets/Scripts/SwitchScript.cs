using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    public bool switchValue;
    public GameObject[] interacteWith;
    public float interactionRange = 2f;

    private Transform player;
    private bool playerInRange;
    private bool wasInteractPressed;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Check if player is in range
        float distance = Vector3.Distance(transform.position, player.position);
        playerInRange = distance <= interactionRange;

        // E key pressed (single press, not hold)
        bool interactPressed = Input.GetKeyDown(KeyCode.E);

        if (playerInRange && interactPressed)
        {
            switchValue = !switchValue;
            foreach (GameObject item in interacteWith)
                item?.GetComponent<SwitchInteractive>()?.DoStuff(switchValue);
            GetComponent<Renderer>().material.color = switchValue ? Color.green : Color.blue;
            Debug.Log($"[Switch] Toggled to: {switchValue}");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
