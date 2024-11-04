using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRotate : MonoBehaviour
{
    Vector3 startPos;
    float rotSpeed = 70f;
    LeafMove lM;
    //float newRotSpeed;

    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        lM = this.transform.parent.GetComponent<LeafMove>();
        //newRotSpeed = rotSpeed;
    }

    void Update()
    {
        if (startPos != transform.position)
        {
            if (Vector3.Distance(transform.position, startPos) < 10)
            {
                transform.parent.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
                rotSpeed = 70;
                StartCoroutine(DelRot());
            }
            else
            {
                this.transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
                rotSpeed = 1;
            }
        }
    }

    IEnumerator DelRot()
    {
        yield return new WaitForSeconds(0.8f);
        startPos = this.transform.position;
    }

    IEnumerator DelayStopRotate()
    {
        yield return new WaitForSeconds(2);
        rotSpeed = 90;
    }

    /*  private void OnTriggerEnter(Collider coll)
      {
          if(coll.CompareTag("Player"))
          {
              rotSpeed = 70;          
              Debug.Log("플레이어 태그 시작");
          }       
      }

      private void OnTriggerStay(Collider coll)
      {
          if (coll.CompareTag("Player"))
          {
              rotSpeed = 70;
              Debug.Log("플레이어 태그 중");
          }
      }

      private void OnTriggerExit(Collider coll)
      {
          if(coll.CompareTag("Player"))
          {
              rotSpeed = 0;
              Debug.Log("플레이어 아웃");
          }
      }*/
}
