using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenLeavesMove : MonoBehaviour
{
    public static FallenLeavesMove FL;
    public float mSpeed = 1f;
    bool left;
    Vector3 newVector;
    


    void Start()
    {
        FL = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SpawnManager.SM.isHold)
        {
            if (!SpawnManager.SM.isHold) return;
            // transform.position = Vector3.Lerp(transform.position, newVector, 0.0001f);
          //  transform.GetComponent<Rigidbody>().AddForce(newVector);
        }
           // transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

        
        //transform.Translate(mSpeed * Time.deltaTime * newVector);
        /* mSpeed -= 0.1f;
         if(mSpeed <= 0f )
         {
             mSpeed = 0f;
         }*/
    }

    private void OnCollisionEnter(Collision coll)
    {
            
        if (coll.collider.CompareTag("Foot"))
        {
            StartCoroutine(DelayReduceSpeed());
            /* mSpeed -= 0.1f;
             if (mSpeed < 0f)
             {
                 mSpeed = 0f;
             }*/
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x >= 960)
                left = false;
            else
            {
                left = true;
            }
            left = (Random.value > 0.5f);
            Vector3 ballPoint = this.transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 clickPoint;
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                clickPoint = hitInfo.point;
            }
            else
            {
                clickPoint = new Vector3(0f, 0f, 0f);
            }
            clickPoint.y = 1;
            ballPoint.y = 1;
            newVector = ballPoint - clickPoint;
            Vector3 newVector2 = clickPoint - ballPoint;
            
        }
    }

    public IEnumerator DelayReduceSpeed()
    {
        yield return new WaitForSecondsRealtime(1);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // StartCoroutine(BacktoSpeed());
        /*  mSpeed-=0.1f;
          if (mSpeed <= 0)
          {
              mSpeed = 0;
          }*/
    }

    public IEnumerator BacktoSpeed()
    {
        yield return new WaitForSeconds(1);
        transform.GetComponent<Rigidbody>().velocity = newVector;
    }

    public IEnumerator RotStop()
    {
        yield return new WaitForSeconds(2);
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;        
        
        
    }
}
