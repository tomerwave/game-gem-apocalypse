using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractButton : MonoBehaviour
{
    public float delay = 0.5f; // seconds before interact becomes true
    private Coroutine holdRoutine;

    public void OnPointerDown()
    {
        holdRoutine = StartCoroutine(DelayedPress());
    }

    public void OnPointerUp()
    {
        if(gameObject.name.Contains("Interact"))
            MobileInput.interact = false;
        if(gameObject.name.Contains("Mask"))
            MobileInput.mask = false;
    }

    private IEnumerator DelayedPress()
    {
        if(gameObject.name.Contains("Interact"))
            MobileInput.interact = true;
        if(gameObject.name.Contains("Mask"))
            MobileInput.mask = true;
        yield return new WaitForSeconds(delay);
        OnPointerUp();
    }
}