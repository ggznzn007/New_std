using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMgr : MonoBehaviour
{
    Camera ARCam;
    Ray ray;
    RaycastHit hit;
    bool isActive = false;
    
    void Start()
    {
        ARCam = GameObject.Find("ARCamera").GetComponent<Camera>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            ray = ARCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 100f, 1 << 8))
            {
                if(isActive==false)
                    hit.transform.Find("Canvas").gameObject.SetActive(isActive = true);
                else
                    hit.transform.Find("Canvas").gameObject.SetActive(isActive = false);
            }
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ray = ARCam.ScreenPointToRay(Input.GetTouch(0).position);

            if(Physics.Raycast(ray, out hit, 100f, 1 << 8))
            {
                if (isActive == false)
                    hit.transform.Find("Canvas").gameObject.SetActive(isActive = true);
                else
                    hit.transform.Find("Canvas").gameObject.SetActive(isActive = false);
            }
        }
#endif
    }
}
