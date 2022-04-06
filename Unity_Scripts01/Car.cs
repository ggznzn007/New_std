using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    public float carSpeed;
    public Transform target;
    int nextTarget;
    public bool player;


    public void StartAI()
    {
        if (!player)
        {
            target = GameManager.instance.target[nextTarget];
            GetComponent<NavMeshAgent>().speed = carSpeed;
            StartCoroutine("AI_Move");
            StartCoroutine("AI_Animation");
        }
    }

    IEnumerator AI_Move()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.position);
        while (true)
        {
            float dis = (target.position - transform.position).magnitude;

            if (dis <= 1)
            {
                nextTarget += 1;
                if (nextTarget >= GameManager.instance.target.Length)
                {
                    nextTarget = 0;
                }

                target = GameManager.instance.target[nextTarget];
                GetComponent<NavMeshAgent>().SetDestination(target.position);
            }

            yield return null;
        }
    }

    IEnumerator AI_Animation()
    {
        Vector3 lastPosition;

        while (true)
        {
            lastPosition = transform.position;
            yield return new WaitForSecondsRealtime(0.03f);

            if ((lastPosition - transform.position).magnitude > 0)
            {
                Vector3 dir = transform.InverseTransformPoint(lastPosition);
                if (dir.x >= -0.01f && dir.x <= 0.01f)
                {
                    GetComponent<Animator>().Play("Ani_Forward");
                }
                if (dir.x < -0.01f)
                {
                    GetComponent<Animator>().Play("Ani_Right");
                }
                if (dir.x > 0.01f)
                {
                    GetComponent<Animator>().Play("Ani_Left");
                }
            }

            if ((lastPosition - transform.position).magnitude <= 0)
            {
                GetComponent<Animator>().Play("Ani_Idle");
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            if (other.gameObject.tag == "Finish")
            {
                if (GameManager.instance.check)
                {
                    GameManager.instance.check = false;

                    if (GameManager.instance.lap > 0)
                    {
                        SE_Manager.instance.Playsound(SE_Manager.instance.lap);
                        GameManager.instance.LapTime();
                    }
                    GameManager.instance.lap += 1;
                }
            }

            if(other.gameObject.tag =="CheckPoint")
            {
                GameManager.instance.check = true;
            }
        }
    }
}
