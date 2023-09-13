using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBomb : MonoBehaviour
{
    public GameObject exp;
    public float expForce , radius;
    public Renderer rend;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other){
        // GameObject _exp = Instantiate(exp,transform.position,transform.rotation);
        // Destroy(_exp,3);
        knockBack();
        Destroy(exp);
    }
    void knockBack(){
        Collider[] colliders = Physics.OverlapSphere(transform.position,radius);
        Vector3 newVector = transform.position-transform.position ;
        foreach (Collider nearby in colliders){
            Rigidbody rigg = nearby.GetComponent<Rigidbody>();
            if (rigg != null){
                Debug.Log(rigg);
                //  rigg.WakeUp();
                rigg.isKinematic = false;
                // rigg.velocity = Vector3.zero;
                // rigg.AddExplosionForce(expForce,transform.position,radius,0f, ForceMode.Force);
                newVector.y=1;
                rigg.AddForce(newVector*5.0f*Time.deltaTime,ForceMode.Impulse);
                // this.transform.position = Vector3.MoveTowards(this.transform.position ,initialLoc,20.0f*Time.deltaTime);
            }
        }
    }
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
