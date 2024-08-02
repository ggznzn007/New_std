using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Settings3 : MonoBehaviour
{
    public bool isTouch;
    public Image touchBtn, joyStickBtn;
    public Color black;
    public PlayerController2 playerController2_script;
    bool settingSwich = true;

    GameObject joyStick;

    private void Start()
    {        
        joyStick = playerController2_script.joyStick;
    }

    // 설정 버튼 누르면 호출
    public void ClickSetting()
    {
        if (settingSwich)
        {
            joyStick.SetActive(false);
            StartCoroutine(SettingOpen());
            playerController2_script.StartCoroutine("FriendsPanelClose");
            playerController2_script.StartCoroutine("ChattingClose");
            playerController2_script.StartCoroutine("EmoticonPanelClose");
        }
        else
        {
            joyStick.SetActive(true);
            StartCoroutine(SettingClose());
        }

    }
    IEnumerator SettingOpen()
    {
        this.gameObject.transform.LeanMoveLocal(new Vector3(0f, 100f), 0.2f);
        this.gameObject.transform.LeanScale(Vector3.one, 0.2f);
        settingSwich = false;

        playerController2_script.isCantMove = true;
        yield return null;
    }
    IEnumerator SettingClose()
    {
        this.gameObject.transform.LeanMoveLocal(new Vector3(448f, 1020f), 0.2f);
        this.gameObject.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        settingSwich = true;
        playerController2_script.isCantMove = false;
        yield return null;
    }

    // 터치이동을 누르면 호출
    public void ClickTouch()
    {
        isTouch = true;
        joyStick.SetActive(false);
        playerController2_script.isCantMove = false;
    }

    // 조이스틱을 누르면 호출
    public void ClickJoyStick()
    {
        isTouch = false;
        joyStick.SetActive(true);
        playerController2_script.isCantMove = true;
    }

    // 게임 종료 누르면 호출
    public void ClickQuit()
    {
        LoadingUIController.Instance.LoadScene("Main");
        //캐릭터 삭제
        playerController2_script.DestoyPlayer();
    }
}
