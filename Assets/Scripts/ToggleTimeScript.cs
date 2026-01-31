using UnityEngine;

public class ToggleTimeScript : MonoBehaviour
{
    public Texture futureTexture,pastTexture;
    private bool timeToggle = false;
    

    public void OnGlobalToggleChanged(bool value)
    {
        Debug.Log(gameObject.tag);
        switch (gameObject.tag)
        {
            case "Wall":
                gameObject.GetComponent<Renderer>().enabled = !value;
                gameObject.GetComponent<Collider>().enabled = !value;
                break;
            default:
                timeToggle = value;
                Debug.Log(gameObject.name + " has been called with global toggle.");
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = value?Color.red:Color.white;

                break;
            
        }
    }
}
