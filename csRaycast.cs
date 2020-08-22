using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRaycast : MonoBehaviour
{
    public float speed = 20f;
    public float rayLength = 16f;
    void Update()
    {
        // 좌우 움직임
        float h = Input.GetAxis("Horizontal");
        h = h * speed * Time.deltaTime;
        this.transform.Translate(Vector3.right * h);

        // Scene창에 광선이 눈에 보이도록 한다(Debug목적으로)
        Debug.DrawRay(transform.position,
                      transform.forward * rayLength,
                      Color.red);
        // 실제 광선을 내보낸다
        RaycastHit hit;
        if(Physics.Raycast(transform.position, 
                        transform.forward,
                        out hit, rayLength))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
