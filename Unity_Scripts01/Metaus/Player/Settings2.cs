using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings2 : MonoBehaviour
{
    public bool isTouch;
    public Image touchBtn, joyStickBtn;
    public Color black;
    public PlayerController playerController_script;
    bool settingSwich = true;
   
   
    GameObject mainView, playView, joyStick;
    
    private void Start()
    {

       mainView = playerController_script.mainView;
        playView = playerController_script.playView;
        joyStick = playerController_script.joyStick;
    }
    // 설정 버튼 누르면 호출
    public void ClickSetting()
    {
        if(settingSwich)
        {
            this.gameObject.transform.LeanScale(Vector3.one, 0.2f);
            settingSwich = false;
            playerController_script.isCantMove = true;
        }
        else
        {
            this.gameObject.transform.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
            settingSwich = true;
            playerController_script.isCantMove = false;
        }
        
    }
    // 게임으로 돌아가기 버튼 누르면 호출
    public void ClickBack()
    {
        gameObject.SetActive(false);
        playerController_script.isCantMove = false;

    }

    // 터치이동을 누르면 호출
    public void ClickTouch()
    {
        isTouch = true;
        joyStick.SetActive(false);
        touchBtn.color = black;
        joyStickBtn.color = Color.white;
    }

    // 조이스틱을 누르면 호출
    public void ClickJoyStick()
    {
        isTouch = false;
        joyStick.SetActive(true);
        touchBtn.color = Color.white;
        joyStickBtn.color = black;
        
    }

    // 게임 종료 누르면 호출
    public void ClickQuit()
    {
        /*mainView.SetActive(true);
        playView.SetActive(false);*/
        // LoadingSceneCtrl.LoadScene("Main");
        LoadingUIController.Instance.LoadScene("Main");

        //캐릭터 삭제
        playerController_script.DestoyPlayer();
    }


  
  
   
}
