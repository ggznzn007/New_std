using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
            Debug.Log("태그됨");
        }
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         if (collision.collider.gameObject.tag=="Player")
         {
             player.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
             Debug.Log("플레이어 태그됨");
         }

     }*/
}
