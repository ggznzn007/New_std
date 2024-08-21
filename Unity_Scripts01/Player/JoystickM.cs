using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickM : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform rect;
    Vector2 touch = Vector2.zero;
    [SerializeField] RectTransform handle;
    float widethHalf;
    [SerializeField] JoystickValue value;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        widethHalf = rect.sizeDelta.x * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
       touch = (eventData.position - rect.anchoredPosition)/ widethHalf;
        if(touch.magnitude>1)
        {
            touch = touch.normalized;
        }
        value.joyTouch = touch;
        handle.anchoredPosition = touch * widethHalf;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        value.joyTouch = Vector2.zero; 
        handle.anchoredPosition = Vector2.zero;
    }
}
