using UnityEngine;

public class ToggleTimeScript : MonoBehaviour
{
    public Texture futureTexture,pastTexture;
    private bool timeToggle=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnGlobalToggleChanged(bool value)
    {
        timeToggle = value;
        Debug.Log(gameObject.name + " has been called with global toggle.");
        Renderer rend = GetComponent<Renderer>();
        if (value)
        {
            rend.material.color = Color.red;
        }
        else
        {
            rend.material.color = Color.white;
        }
    }
}
