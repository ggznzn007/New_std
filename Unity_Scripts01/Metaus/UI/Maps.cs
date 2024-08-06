using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maps : MonoBehaviour
{
    public GameObject selectView;

     // 공간 선택 창 호출
     public void ClickBack()
     {
         this.gameObject.SetActive(false);
         selectView.SetActive(true);
     }
  
    // 채팅창 활성화
    public void ClickChat()
    {
       // chatting.SetActive(true);
    }

    // 채팅차 비활성화
    public void ClickQuit()
    {
       // chatting.SetActive(false);
    }
}
