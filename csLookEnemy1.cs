using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csLookEnemy1 : MonoBehaviour
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
        viewer.LookAt(enemy);
        transform.rotation = viewer.rotation;

        Debug.DrawRay(spPoint.position,
            spPoint.forward * rayLength, Color.red);
        if (Physics.Raycast(spPoint.position, fwd, out hit, rayLength))
            Debug.Log(hit.collider.gameObject.name);
    }
}
