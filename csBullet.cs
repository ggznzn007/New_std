using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        if(this.transform.position.y>10f)
        {
            Destroy(this.gameObject);
        }
    }
}
