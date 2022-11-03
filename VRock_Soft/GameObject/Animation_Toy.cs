using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Toy : MonoBehaviour
{
    public Animation anim;
   
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play();

    }
}
