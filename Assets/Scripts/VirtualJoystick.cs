using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform handle;
    private bool lookInput;
    public float moveRange = 75f;

    private Vector2 inputVector;
    void Start()
    {
        handle = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        lookInput = transform.gameObject.name.Contains("Look");

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        pos = Vector2.ClampMagnitude(pos, moveRange);
        handle.anchoredPosition = pos;

        inputVector = pos / moveRange;

        if (!lookInput)
        {
            MobileInput.horizontal = inputVector.x;
            MobileInput.vertical = inputVector.y;
        }
        else
        {
            MobileInput.MouseX = inputVector.x;
            MobileInput.MouseY = inputVector.y;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        
        if (!lookInput)
        {
            MobileInput.horizontal = 0;
            MobileInput.vertical = 0;
        }
        else
        {
            MobileInput.MouseX = 0;
            MobileInput.MouseY = 0;
        }
    }

    public float Horizontal() => inputVector.x;
    public float Vertical() => inputVector.y;
    
}