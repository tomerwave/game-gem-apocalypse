using UnityEngine;

public class ToggleTimeScript : MonoBehaviour
{
    public Texture futureTexture,pastTexture;
    public GameObject linkedGameObject;
    private bool timeToggle = false;
    

    public void OnGlobalToggleChanged(bool value)
    {
        if(linkedGameObject!=null){
            linkedGameObject.transform.position = transform.position;
            linkedGameObject.SetActive(true);
            this.gameObject.SetActive(false);
            return;
        }

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
