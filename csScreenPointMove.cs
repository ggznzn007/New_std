using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csScreenPointMove : MonoBehaviour
{
    public Transform cube;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = 
                // (Tag => MainCamera) == Camera.main
                Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Vector3 newPos = new Vector3(hit.point.x,
                                            cube.transform.position.y,
                                            hit.point.z);
                cube.transform.position = newPos;
            }
        }
    }
}
