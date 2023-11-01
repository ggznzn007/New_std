using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Control_Ante : PlayerControl
{
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sk = GetComponent<SpriteSkin>();

        waitTime = Setting.startWaitTime;
    }
    private void Start()
    {
        mySp = Instantiate(mySpot).transform;
        moveSpot = mySp.GetComponent<Transform>();
        anim = GetComponent<Animator>();


        moveSpot.position = new Vector3(Random.Range(Setting.minX, Setting.maxX), Random.Range(Setting.minY, Setting.maxY)
          , Random.Range(Setting.minZ, Setting.maxZ));

    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, Setting.speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < Setting.distance)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector3(Random.Range(Setting.minX, Setting.maxX), Random.Range(Setting.minY, Setting.maxY)
                  , Random.Range(Setting.minZ, Setting.maxZ));
                waitTime = Setting.startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
                // anim.SetBool("IsRight", false);
                // anim.SetBool("IsLeft", false);
            }
        }

        if (moveSpot.position.x < transform.position.x)
        {
            sr.flipY = false;
            //sr.flipY = false;
            // anim.SetBool("IsRight", true);
            // anim.SetBool("IsLeft", false);
        }
        else if (moveSpot.position.x > transform.position.x)
        {
            sr.flipY = true;
            // sr.flipY = true;
            //  anim.SetBool("IsRight", false);
            // anim.SetBool("IsLeft", true);
        }

        //StartCoroutine(DeActive_Ante());
    }

    public IEnumerator DeActive_Ante()
    {
        yield return new WaitForSeconds(Setting.DelTime);
        ObjectPool.OP.ReturnToPool_Ante(gameObject);
    }
}
