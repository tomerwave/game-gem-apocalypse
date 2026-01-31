using UnityEngine;

public class SwitchInteractive : MonoBehaviour
{
    public void DoStuff(bool value)
    {
        switch (gameObject.tag)
        {
            case "Door":
                transform.Rotate(new Vector3(0,90f,0));
                break;
        }   
    }
}
