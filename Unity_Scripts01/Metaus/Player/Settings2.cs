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
    // ���� ��ư ������ ȣ��
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
    // �������� ���ư��� ��ư ������ ȣ��
    public void ClickBack()
    {
        gameObject.SetActive(false);
        playerController_script.isCantMove = false;

    }

    // ��ġ�̵��� ������ ȣ��
    public void ClickTouch()
    {
        isTouch = true;
        joyStick.SetActive(false);
        touchBtn.color = black;
        joyStickBtn.color = Color.white;
    }

    // ���̽�ƽ�� ������ ȣ��
    public void ClickJoyStick()
    {
        isTouch = false;
        joyStick.SetActive(true);
        touchBtn.color = Color.white;
        joyStickBtn.color = black;
        
    }

    // ���� ���� ������ ȣ��
    public void ClickQuit()
    {
        /*mainView.SetActive(true);
        playView.SetActive(false);*/
        // LoadingSceneCtrl.LoadScene("Main");
        LoadingUIController.Instance.LoadScene("Main");

        //ĳ���� ����
        playerController_script.DestoyPlayer();
    }


  
  
   
}
