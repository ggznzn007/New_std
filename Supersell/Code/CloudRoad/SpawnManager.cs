using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SM;
    public GameObject Foot;
    public bool isHold;
    private Vector3 m_Offset;
    private float m_ZCoord;
    private GameObject myFoot;

    void Start()
    {
        SM = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHold = true;
            //Vector3 mouseScreenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                SpawnBombAtPos(hitInfo.point);
                FallenLeavesMove.FL.mSpeed = 1f;
            }
        }

        if (Input.GetMouseButton(0))
        {
            isHold = true;
            myFoot.transform.position = GetMouseWorldPosition() + m_Offset;
            Collider[] coll = Physics.OverlapSphere(myFoot.transform.position, 10);
            foreach (Collider collider in coll)
            {
                if (collider.gameObject != null)
                {
                    collider.GetComponent<Rigidbody>().AddForce(collider.transform.position, ForceMode.Impulse);
                    StartCoroutine(FallenLeavesMove.FL.BacktoSpeed());
                    StartCoroutine(FallenLeavesMove.FL.RotStop());
                }
            }
            /*int i = 0;
            while (i < coll.Length)
            {
                coll[i].GetComponent<Rigidbody>().AddForce(coll[i].transform.position);
                    i++;
                StartCoroutine(DelayFalse());
                
            }*/

            //Debug.Log(coll[i]);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(myFoot);
            //StartCoroutine(DelayFalse());
        }
    }

    private void SpawnBombAtPos(Vector3 spawnPosition)
    {
        spawnPosition.y = 1;
        myFoot = Instantiate(Foot, spawnPosition, Quaternion.identity);
    }

    Vector3 GetMouseWorldPosition()                                        // 마우스 위치값 함수
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    IEnumerator DelayFalse()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        //isHold = false;
        FallenLeavesMove.FL.mSpeed = 0f;
    }
}
