using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Western : MonoBehaviour
{
    public Animation anim;
    public GameObject fakePlane;
    public GameObject[] meshes;
    

    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play();
       StartCoroutine(OffObj());        
    }

    IEnumerator OffObj()
    {
        yield return new WaitForSeconds(1.4f);
        fakePlane.SetActive(false);
        gameObject.isStatic = true;
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].isStatic = true;            
        }        
    }
}
