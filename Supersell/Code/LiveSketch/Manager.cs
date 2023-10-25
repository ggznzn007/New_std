using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject spotPrefab;

    GameObject myChar;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            myChar = Instantiate(playerPrefab,playerPrefab.transform.position,Quaternion.identity);
           // StartCoroutine(DelayNull());
         

        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(myChar);
        }
    }

    IEnumerator DelayNull()
    {
        myChar = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        yield return null;
        myChar = null;
    }

    public void SpawnChar()
    {
       myChar = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
    }

    public void DestroyChar()
    {
        Destroy(myChar);
    }
}
