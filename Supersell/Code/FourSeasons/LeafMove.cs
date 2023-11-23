using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMove : MonoBehaviour
{
    Transform target;
    float moveSpeed = 10f;
    public Rigidbody rb;
    
    public bool isMoving;
    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        startRot = transform.rotation;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);        
        rb = GetComponent<Rigidbody>();
        rb.Sleep();        
    }


    void Update()
    {
        if (target != null)
        {               
            rb.WakeUp();
            if (Vector3.Distance(transform.position, target.position) > 100f)
            {
                isMoving = true;
                Vector3 dir = target.position + transform.position;
                // transform.Translate(moveSpeed * Time.deltaTime * dir.normalized);
                // transform.LeanMove(dir.normalized, moveSpeed*Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, dir.normalized, moveSpeed * Time.deltaTime);
                //transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
                //transform.LeanMoveLocal(dir.normalized, moveSpeed * Time.deltaTime);                
                StartCoroutine(DelayRidDisable());
            }
        }       
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Finish"))
        {
            StartCoroutine(DelayPos());
        }
    }

    private void UpdateTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 10f);//, 1 << 8);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Player"))
                {
                    target = cols[i].gameObject.transform;
                    Debug.Log("Physics Enemy : Target found");
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Physics Enemy : Target lost");
            target = null;
        }
    }

    IEnumerator DelayRidDisable()
    {
        yield return new WaitForSeconds(1);
        isMoving = false;
        rb.Sleep();       
    }

    IEnumerator DelayPos()
    {
        yield return new WaitForSecondsRealtime(10);
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
