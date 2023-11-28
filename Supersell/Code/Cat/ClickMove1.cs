using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Debug;

public enum CatState { WalkAround, WalkToMaster}

public class ClickMove1 : MonoBehaviour
{
    [Header("����� ��������Ʈ")]
    public Transform[] wayPos;
    [Header("����� �θ��� ����Ʈ")]
    public Transform callPos;
    [Header("����� ��������Ʈ�� �ִ� ����")]
    public bool isOnWay;
    private CatState catState;
    private int wayNum = 0;
    private Vector3 curPos;
    private Transform tr;
    private Animator anim;

    private void Awake()
    {
        ChangeState(CatState.WalkAround);
        isOnWay = true;
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();                                                  
        wayNum = 4;
    }   

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            ChangeState(CatState.WalkToMaster);
            isOnWay = false;
        }
        /* if (Input.GetMouseButton(0))
         {
             isOnWay = false;           
         }

         if (!isOnWay)
         {
             MoveToMaster();
         }

         MovetoWay();*/

        UpdateState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif   
        }
        /*  if (Input.GetMouseButtonDown(0))
          {
              Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

              if (Physics.Raycast(ray, out RaycastHit hit))
              {
                  agent.SetDestination(hit.point);

                  anim.SetBool("isMoving", true);
              }
          }

          if (agent.remainingDistance <= 0.2f && agent.velocity.magnitude >= 0.2f)
          {
              anim.SetBool("isMoving", false);
          }*/
    }

    private void UpdateState()
    {
        if (catState == CatState.WalkToMaster)
        {
            MoveToMaster();
        }
        if (catState==CatState.WalkAround)
        {
            MovetoWay();
        }       
    }

    private void ChangeState(CatState newState)
    {
        catState = newState;
        isOnWay = true;
    }

  

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Master"))
        {
            isOnWay = false;
        }

        if (coll.CompareTag("Waypoint"))
        {
            // isOnWay = true;
        }
    }

    public void MovetoWay()                       // ��������Ʈ�� ���ƴٴϴ� �޼���
    {
        if (!isOnWay) return;
        /*  anim.SetBool("isMoving", true);

          curPos = transform.position;
          // ���� ��ġ���� ���� ��������Ʈ�� ���ϴ� ���͸� ���
          Vector3 direction = wayPos[wayNum].position - tr.position;

          // ����� ������ ȸ�� ������ ���ʹϾ� Ÿ������ ����
          Quaternion rot = Quaternion.LookRotation(direction.normalized);

          // ���� �������� ȸ���ؾ� �� �������� �ε巴�� ȸ�� ó��
          tr.rotation = Quaternion.Slerp(tr.rotation, rot, Settings.damSpeed * Time.deltaTime);

          if (wayNum < wayPos.Length)
          {
              float step = Settings.moveSpeed * Time.deltaTime;
              //tr.Translate(step* Vector3.forward);
              transform.position = Vector3.MoveTowards(curPos, wayPos[wayNum].position, step);
              transform.LookAt(wayPos[wayNum].position);
          }*/
        anim.SetBool("isMoving", true);

        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, wayPos[wayNum].position, Settings.callSpeed * Time.deltaTime),
            Quaternion.Slerp(transform.rotation, transform.rotation, Settings.damSpeed));
        transform.LookAt(wayPos[wayNum].position);

        if (Vector3.Distance(wayPos[wayNum].position, transform.position) == 0f)
        {
            wayNum = Random.Range(1, wayPos.Length);
        }
    }

    public void MoveToMaster()                      // Ŭ������ �� ����̰� ���Բ� �θ��� �޼���
    {
        /*anim.SetBool("isMoving", true);

        curPos = transform.position;
        // ���� ��ġ���� ���� ��������Ʈ�� ���ϴ� ���͸� ���
        Vector3 direction = wayPos[wayNum].position - tr.position;

        // ����� ������ ȸ�� ������ ���ʹϾ� Ÿ������ ����
        Quaternion rot = Quaternion.LookRotation(direction.normalized);

        // ���� �������� ȸ���ؾ� �� �������� �ε巴�� ȸ�� ó��
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Settings.damSpeed * Time.deltaTime);

        if (wayNum < wayPos.Length)
        {
            float step = Settings.moveSpeed * Time.deltaTime;
            //tr.Translate(step* Vector3.forward);
            transform.position = Vector3.MoveTowards(curPos, wayPos[wayNum].position, step);
            transform.LookAt(wayPos[wayNum].position);
        }

        if (Vector3.Distance(wayPos[wayNum].position, transform.position) == 0f)
        {
            transform.LookAt(Camera.main.transform.position);
            anim.SetBool("isMoving", false);
            StartCoroutine(StandingCat());
        }*/


        anim.SetBool("isMoving", true);

        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, wayPos[0].position, Settings.callSpeed * Time.deltaTime),
            Quaternion.Slerp(transform.rotation, transform.rotation, Settings.damSpeed));
        transform.LookAt(wayPos[0].position);

        if (Vector3.Distance(wayPos[0].position, transform.position) == 0f)
        {
            transform.LookAt(Camera.main.transform.position);
            anim.SetBool("isMoving", false);
            StartCoroutine(StandingCat());
        }
    }


    IEnumerator StandingCat()
    {
        wayNum = Random.Range(1, wayPos.Length);
        yield return new WaitForSeconds(15);
        ChangeState(CatState.WalkAround);
    }

    /*   private void OnDrawGizmosSelected()
       {
           if (wayPos == null) { return; }
           for (int i = 0; i < wayPos.Length; i++)
           {
               Gizmos.color = Color.red;
               if (i == 0)
               {
                   Gizmos.DrawLine(transform.position, wayPos[i].position);
               }
               else
               {
                   Gizmos.DrawLine(wayPos[i - 1].position, wayPos[i].position);
                   Log(wayPos[i - 1].ToString() + " /to: " + wayPos[i].ToString());
               }
               // Gizmos.DrawIcon(wayPos[i].position, "wayPoint.png");
           }
       }*/


    /*  // ����ڵ�
      public void MovetoWay()
      {
          if (!isOnWaypoint) { return; }
          anim.SetBool("isMoving", true);

          Quaternion rot = Quaternion.Euler(transform.position);
          transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, wayPos[wayNum].position, Settings.moveSpeed * Time.deltaTime),
              Quaternion.Slerp(transform.rotation, rot, Settings.damSpeed));
          transform.LookAt(wayPos[wayNum].position);

          if (transform.position == wayPos[wayNum].transform.position)
          {
              wayNum = Random.Range(1, wayPos.Length);
          }
      }*/
    /* public void MoveToMaster()
     {
         anim.SetBool("isMoving", true);
         transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, wayPos[0].position, Settings.callSpeed * Time.deltaTime),
             Quaternion.Slerp(transform.rotation, transform.rotation, Settings.damSpeed));
         transform.LookAt(wayPos[0].position);

         if (Vector3.Distance(wayPos[0].position, transform.position) == 0f)
         {
             anim.SetBool("isMoving", false);
             StartCoroutine(StandingCat());
         }

        // if (transform.position == wayPos[0].transform.position)
        // {
         //    anim.SetBool("isMoving", false);
        //     StartCoroutine(StandingCat());
       //  }
     }*/
}
