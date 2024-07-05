using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Control_Ante : PlayerControl
{   
    public float speed =1;

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
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        
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
            }
        }

        if (moveSpot.position.x < transform.position.x)
        {
            sr.flipY = false;          
        }
        else if (moveSpot.position.x > transform.position.x)
        {
            sr.flipY = true;           
        }
    }
}
