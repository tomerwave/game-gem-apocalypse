using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    public bool switchValue;
    public GameObject[] interacteWith;    

    private Camera cam;
    private float maxDistance = 20f;

    void Start()
    {
        cam ??= FindObjectOfType<Camera>();
        Debug.Log(cam.name);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                float distance = Vector3.Distance(cam.transform.position, hit.point);

                if (distance <= 2f)
                {
                    switchValue = !switchValue;
                    foreach (GameObject item in interacteWith)
                        item?.GetComponent<SwitchInteractive>()?.DoStuff(switchValue);
                    GetComponent<Renderer>().material.color = switchValue?Color.green:Color.blue;
                }
            }
        }
    }
}
