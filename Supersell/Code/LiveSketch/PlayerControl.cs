using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerControl : MonoBehaviour
{
    public GameObject mySpot;
    public Transform moveSpot;
   
    float waitTime;
    Animator anim;
    SpriteRenderer sr;
    SpriteSkin sk;
    Transform mySp;
    List<Transform> spots = new();

    public int setPoint;
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
       
        /*  mySpot = GameObject.Find(Setting.movingSpots);
          if (mySpot != null)
          {
              moveSpots = mySpot.GetComponentsInChildren<Transform>();
          }
          setPoint = Random.Range(0,moveSpots.Length);
          moveSpot = moveSpots[setPoint];*/
        moveSpot.position = new Vector3(Random.Range(Setting.minX, Setting.maxX), Random.Range(Setting.minY, Setting.maxY)
            ,Random.Range(Setting.minZ,Setting.maxZ));
      //  Destroy(gameObject, 10f);
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
            sr.flipX = true;
            //sr.flipY = false;
           // anim.SetBool("IsRight", true);
           // anim.SetBool("IsLeft", false);
        }
        else if (moveSpot.position.x > transform.position.x)
        {
            sr.flipX = false;
           // sr.flipY = true;
          //  anim.SetBool("IsRight", false);
           // anim.SetBool("IsLeft", true);
        }
    }


    /*public int speed = 10;
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime; //x축으로 이동할 양
        float yMove = Input.GetAxis("Vertical") * speed * Time.deltaTime; //y축으로 이동할양
        this.transform.Translate(new Vector3(xMove, yMove, 0));  //이동
    }*/ // 키보드 이동
}
