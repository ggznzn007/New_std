using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public GameObject selectView;
    
    

    // ���� ���� â ȣ��
    public void ClickLogin()
    {
        this.gameObject.SetActive(false);
        selectView.SetActive(true);        
    }
}
