using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csLookEnemy : MonoBehaviour
{
    public Transform enemy;
    private Transform spPoint;
    public float rayLength = 4f;
    RaycastHit hit;
    Vector3 fwd = Vector3.forward;
    void Start()
    {
        spPoint = transform.Find("/Turret/Tower/SpawnPoint");
    }
    void Update()
    {
        transform.LookAt(enemy);
        Debug.DrawRay(spPoint.position,
            spPoint.forward * rayLength, Color.red);
        if (Physics.Raycast(spPoint.position, fwd, out hit, rayLength))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
