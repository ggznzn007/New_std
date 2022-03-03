using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public GameObject selectView;
    
    

    // 공간 선택 창 호출
    public void ClickLogin()
    {
        this.gameObject.SetActive(false);
        selectView.SetActive(true);        
    }
}
