using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Toy : MonoBehaviour
{
    public Animation anim;
    public GameObject[] meshes;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play();
        StartCoroutine(OffObj());
    }

    IEnumerator OffObj()
    {
        yield return new WaitForSeconds(1.4f);
        gameObject.isStatic = true;
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].isStatic = true;
        }
    }
}
