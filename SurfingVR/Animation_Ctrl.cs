using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animation_Ctrl : MonoBehaviour
{
    public Animator ani;
    public void Animation_ctrl(string str)
    {
        ani = GetComponent<Animator>();

        // ani.SetInteger("motion", ani_num);
        ani.SetTrigger(str);
        
    }
  
}
