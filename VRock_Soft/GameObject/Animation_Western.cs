using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Western : MonoBehaviour
{
    public Animation anim;
    public GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play();
        StartCoroutine(FakePlaneHide());

    }

    IEnumerator FakePlaneHide()
    {
        yield return new WaitForSeconds(1.4f);
        plane.SetActive(false);
    }
}
