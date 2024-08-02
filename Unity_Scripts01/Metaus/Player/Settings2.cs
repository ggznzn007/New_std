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
    
    GameObject joyStick;
    
    private void Start()
    {      
       joyStick = playerController_script.joyStick;       
    }

    // ���� ��ư ������ ȣ��
    public void ClickSetting()
    {       
        if (settingSwich)
        {
            joyStick.SetActive(false);
            StartCoroutine(SettingOpen());
            playerController_script.StartCoroutine("FriendsPanelClose");
            playerController_script.StartCoroutine("ChattingClose");
            playerController_script.StartCoroutine("EmoticonPanelClose");
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
        
        playerController_script.isCantMove = true;
        yield return null;
    }
    IEnumerator SettingClose()
    {
        this.gameObject.transform.LeanMoveLocal(new Vector3(448f, 1020f), 0.2f);
        this.gameObject.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        settingSwich = true;       
        playerController_script.isCantMove = false;
        yield return null;
    }

    // ��ġ�̵��� ������ ȣ��
    public void ClickTouch()
    {
        isTouch = true;
        joyStick.SetActive(false);
        playerController_script.isCantMove = false;
    }

    // ���̽�ƽ�� ������ ȣ��
    public void ClickJoyStick()
    {
        isTouch = false;
        joyStick.SetActive(true);
        playerController_script.isCantMove = true;
    }

    // ���� ���� ������ ȣ��
    public void ClickQuit()
    {       
        LoadingUIController.Instance.LoadScene("Main");
        //ĳ���� ����
        playerController_script.DestoyPlayer();
    }   
}
