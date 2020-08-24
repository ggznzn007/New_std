using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csFunctionCall : MonoBehaviour
{
    GameObject cube;
    void Start()
    {
        cube = GameObject.Find("Cube");
    }
    private void OnGUI()
    {
        if(GUI.Button(new Rect(30, 50, 180, 30), 
            "Function Call(Public)"))
        {
            csRotateCube script = cube.GetComponent<csRotateCube>();
            script.Rotate1();
        }
        if(GUI.Button(new Rect(30, 100, 180, 30), 
            "Function Call(Private)"))
        {
            cube.SendMessage("Rotate2",
                SendMessageOptions.DontRequireReceiver);
        }
        if(GUI.Button(new Rect(30, 150, 180, 30),
            "Static"))
        {
            Debug.Log("Call Variable : " + csRotateCube.numX);
            Debug.Log("Call Function : " + csRotateCube.AddTwoNum(3, 5));
        }
    }
}
