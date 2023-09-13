using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovement2 : MonoBehaviour
{
    Vector3 worldPosition;
    public GameObject windObject;
    new Rigidbody rigidbody;
    public float lifeTime = 10f;
    //public float Hspeed = 4f;
    //public float HspeedBack = 5f;
    public float Hspeed;
    public float HspeedBack;
    float temp = 0;
    float time;
    float timeStop;
    //Vector3 pointerLoc;
    float timeDelay;
    public float delta = 8f;
   // float eulerAngZ;
    public float direction = 10f;
    private Quaternion startPos;
    Quaternion startRotation = Quaternion.Euler(0, 0, 70);
    Quaternion endRotation = Quaternion.Euler(0, 0, -70);
    //float speed = 1.0f;
    Vector3 initialLoc;
    Vector3 maxLoc, minLoc;
    bool swt = true;
    bool held = false;
    bool left = false;
    bool once = true;
    bool beingHandled = false;
    Vector3 tempLoc;
    Vector3 newVector;

    void Start()
    {
        time = 0f;
        timeStop = 0f;
        timeDelay = 5f;
        // Hspeed = Random.Range(1.1f,2f);
        Hspeed = 1.5f;
        temp = Random.value > 0.5f ? -30f : 30f;
        startPos = transform.rotation;
        direction = Random.Range(0.1f, 1.0f);
        delta = Random.Range(0.5f, 2.0f);
        initialLoc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        maxLoc = new Vector3(initialLoc.x + 2.0f, initialLoc.y + 2.0f, initialLoc.z + 2.0f);
        minLoc = new Vector3(initialLoc.x - 2.0f, initialLoc.y - 2.0f, initialLoc.z - 2.0f);
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if ( /*some case  */ !beingHandled)
            StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.

        beingHandled = true;
        //yield on a new YieldInstruction that waits for 5 seconds.
        Vector2 pos = this.transform.position;

        time += 0.1f;
        // time =time+0.002f;
        if (time >= timeDelay)
        {
            Hspeed = 2f;
            time = 0f;
            held = false;

        }

        //After we have waited 5 seconds print the time again.
        if (transform.position.x >= maxLoc.x)
        {
            swt = true;
        }
        if (transform.position.x <= initialLoc.x)
        {
            swt = false;
        }

        newVector.y = 0;
        newVector.Normalize();

        if (held)
        {
            this.transform.Translate(Hspeed * Time.deltaTime * newVector);
            Hspeed -= 0.01f;
            // this.transform.position += newVector*Hspeed*Time.deltaTime;
            // this.transform.position = Vector3.MoveTowards(this.transform.position ,newVector,Hspeed*Time.deltaTime);
            // this.transform.Rotate(Vector3.up, Time.deltaTime * 400f);
        }
        else
        {
            // this.transform.position = Vector3.MoveTowards(this.transform.position,initialLoc,3f*Time.deltaTime);
            if (initialLoc != this.transform.position)
            {
                if (once)
                {
                    yield return new WaitForSeconds(1);
                    Debug.Log("CEK!");
                    once = false;
                }
                HspeedBack = Vector3.Distance(initialLoc, this.transform.position);
                if (Vector3.Distance(initialLoc, this.transform.position) < 1)
                {
                    HspeedBack = Vector3.Distance(initialLoc, this.transform.position);
                }
                if (Vector3.Distance(initialLoc, this.transform.position) < 0.01)
                {
                    HspeedBack = 1f;
                }
                this.transform.position = Vector3.MoveTowards(this.transform.position, initialLoc, HspeedBack * 1f * Time.deltaTime);
            }
        }

        if (transform.position.y <= -5)
        {
            Destruction();
        }
        beingHandled = false;

    }
    void OnMouseDown()
    {
        Debug.Log("masukkeneternytDestroyer");
    }

    private void OnCollisionEnter(Collision other)
    {
        time = 0f;
        held = true;
        Hspeed = 1.7f;
        HspeedBack = 6f;
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x >= 960)
            left = false;
        else
        {
            left = true;
        }
        left = (Random.value > 0.5f);
        once = true;
        time = 0f;
        Vector3 ballPoint = this.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Vector3 clickPoint = ray.GetPoint (10) ; 
        Vector3 clickPoint;
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            clickPoint = hitInfo.point;
        }
        else
        {
            clickPoint = new Vector3(0f, 0f, 0f);
        }
        // clickPoint.z = 1;
        clickPoint.y = 1;
        ballPoint.y = 1;
        newVector = ballPoint - clickPoint;
        Vector3 newVector2 = clickPoint - ballPoint;

        // this.transform.Translate(newVector*1f);

    }

    void OnMouseUp()
    {
        // held=false;
        // once=false;
    }
    void Destruction()
    {
        Destroy(this.gameObject);
    }
}
