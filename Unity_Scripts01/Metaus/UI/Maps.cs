using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maps : MonoBehaviour
{
    public GameObject selectView;

     // ���� ���� â ȣ��
     public void ClickBack()
     {
         this.gameObject.SetActive(false);
         selectView.SetActive(true);
     }
  
    // ä��â Ȱ��ȭ
    public void ClickChat()
    {
       // chatting.SetActive(true);
    }

    // ä���� ��Ȱ��ȭ
    public void ClickQuit()
    {
       // chatting.SetActive(false);
    }
}
