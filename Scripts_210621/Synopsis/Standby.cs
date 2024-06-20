using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Standby : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(3);
        }
    }
}
