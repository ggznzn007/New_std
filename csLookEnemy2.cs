using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csLookEnemy2 : MonoBehaviour
{
    public Transform enemy;
    private Transform viewer;
    private Transform spPoint;

    RaycastHit hit;
    Vector3 fwd = Vector3.forward;
    public float rayLength = 4f;
    void Start()
    {
        viewer = transform.Find("/Turret/Viewer");
        spPoint = transform.Find("/Turret/Tower/SpawnPoint");
    }
    void Update()
    {
        // viewer가 주시하고 회전각도를 동일하게 적용해준다
        viewer.LookAt(enemy);
        transform.rotation = viewer.rotation;

        Vector3 newSpPoint = new Vector3(spPoint.position.x,
                                   viewer.transform.position.y,
                                   spPoint.position.z);

        Debug.DrawRay(newSpPoint,
            spPoint.forward * rayLength, Color.red);
        fwd = transform.TransformDirection(viewer.transform.forward);
        if (Physics.Raycast(newSpPoint, fwd, out hit, rayLength))
            Debug.Log(hit.collider.gameObject.name);
    }
}
