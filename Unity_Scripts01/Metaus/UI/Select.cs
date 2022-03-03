using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class Select : MonoBehaviour
{
    public GameObject[] popUp;
    public RawImage userImage;
    public Text userName;    
    ArrayList popUplist;


    private bool bWaitingForAuth = false;
    public void Start()
    {
        Camera.main.transform.parent = this.transform.parent;

        if(Social.localUser.authenticated)
        {
            Texture2D pic = Social.localUser.image;
            userImage.texture = pic;
            userName.text = Social.localUser.userName;
        }
        else
        {            
            userImage.texture = userImage.texture;
            userName.text = userName.text;
        }
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
            LoadingUIController.Instance.LoadScene("GPGSLogin2");
        }
    }

    // 공간 선택 창 호출
    public void ClickBack()
    {
        // this.gameObject.SetActive(false);
        // ((PlayGamesPlatform)Social.Active).SignOut();
        // LoadingSceneCtrl.LoadScene("GPGSLogin");
        // LoadingUIController.Instance.LoadScene("GPGSLogin");


    }


    // 공간 1 호출
    public void ClickSelect1()
    {
        LoadingUIController_Night.Instance.LoadScene("NightTime");
    }

    // 공간 2 호출
    public void ClickSelect2()
    {
        LoadingUIController_Day.Instance.LoadScene("DayTime");
    }

    public void PopUp1()
    {
        StartCoroutine(GetPopUp1());
    }
    public void PopUp2()
    {
        StartCoroutine(GetPopUp2());
    }
    public void PopUp3()
    {
        StartCoroutine(GetPopUp3());
    }
    public void PopUp4()
    {
        StartCoroutine(GetPopUp4());
    }
    public void PopUp5()
    {
        StartCoroutine(GetPopUp5());
    }
    public void PopUp6()
    {
        StartCoroutine(GetPopUp6());
    }


    IEnumerator GetPopUp1()
    {
        popUp[0].transform.LeanScale(Vector2.one, 0.2f);
        yield return new WaitForSeconds(1.5f);
        popUp[0].transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }

    IEnumerator GetPopUp2()
    {
        popUp[1].transform.LeanScale(Vector2.one, 0.2f);
        yield return new WaitForSeconds(1.5f);
        popUp[1].transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }

    IEnumerator GetPopUp3()
    {
        popUp[2].transform.LeanScale(Vector2.one, 0.2f);
        yield return new WaitForSeconds(1.5f);
        popUp[2].transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }

    IEnumerator GetPopUp4()
    {
        popUp[3].transform.LeanScale(Vector2.one, 0.2f);
        yield return new WaitForSeconds(1.5f);
        popUp[3].transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }

    IEnumerator GetPopUp5()
    {
        popUp[4].transform.LeanScale(Vector2.one, 0.2f);
        yield return new WaitForSeconds(1.5f);
        popUp[4].transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }

    IEnumerator GetPopUp6()
    {
        popUp[5].transform.LeanScale(Vector2.one, 0.2f);
        yield return new WaitForSeconds(1.5f);
        popUp[5].transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }


}
