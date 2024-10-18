using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{   
    public GameObject[] catsPrefab;
    public bool isBlack;
    public bool isYellow;
    public bool isWhite;   
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isBlack = !isBlack;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isYellow = !isYellow;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isWhite = !isWhite;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }

        if (isBlack)
        {
            catsPrefab[0].SetActive(true);
        }

        else
        {
            catsPrefab[0].SetActive(false);
        }

        if(isYellow)
        {
            catsPrefab[1].SetActive(true);
        }
        else
        {
            catsPrefab[1].SetActive(false);
        }
        if (isWhite)
        {
            catsPrefab[2].SetActive(true);
        }
        else
        {
            catsPrefab[2].SetActive(false);
        }
    }
}
