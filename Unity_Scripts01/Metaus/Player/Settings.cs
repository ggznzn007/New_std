using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool isJoyStick;
    public Image touchBtn, joyStickBtn;
    public Color black;
    public PlayerCtrl playerCtrl_script;
    GameObject mainView, playView;

    private void Start()
    {
        mainView = playerCtrl_script.mainView;
        playView = playerCtrl_script.playView;
    }
    // 설정 버튼 누르면 호출
    public void ClickSetting()
    {
        gameObject.SetActive(true);
        playerCtrl_script.isCantMove = true;
    }
    // 게임으로 돌아가기 버튼 누르면 호출
    public void ClickBack()
    {
        gameObject.SetActive(false);
        playerCtrl_script.isCantMove = false;
    }

    // 터치이동을 누르면 호출
    public void ClickTouch()
    {
        isJoyStick = false;
        touchBtn.color = black;
        joyStickBtn.color = Color.white;
    }

    // 조이스틱을 누르면 호출
    public void ClickJoyStick()
    {
        isJoyStick = true;
        touchBtn.color = Color.white; 
        joyStickBtn.color = black;
    }

    // 게임 종료 누르면 호출
    public void ClickQuit()
    {
        /*mainView.SetActive(true);
        playView.SetActive(false);*/
        LoadingUIController.Instance.LoadScene("Main");
        //캐릭터 삭제
        playerCtrl_script.DestoyPlayer();
    }
}
