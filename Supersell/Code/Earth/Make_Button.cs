using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Make_Button : MonoBehaviour
{
    public UnityEvent unityEvent = new();
    public GameObject button;

    void Start()
    {
        button = this.gameObject;
    }

    void Update()
    {
        if (!DataManager.DM.isInfoOpen)
        {
            if (DataManager.DM.isInfoOpen) { return; }
            ButtonClick();
        }
    }

    public void ButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                unityEvent.Invoke();
            }
        }
    }
}
