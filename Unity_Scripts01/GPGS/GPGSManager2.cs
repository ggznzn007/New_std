using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

#if UNITY_ANDROID
public class GPGSManager2 : MonoBehaviour
{
    public Text myName;
    public Text myLog;
    public RawImage myImage;
    public Button fakeLogin, fakeStart;
    public Button realLogin, realStart, gsLogin;
   // private bool bWaitingForAuth = false;
    private void Awake()
    {
        myLog.text = "구글 로그인 해주세요";
        realLogin.interactable = true;
        fakeLogin.interactable = true;
        // 구글 게임서비스 활성화 (초기화)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ((PlayGamesPlatform)Social.Active).SignOut(); // 뒤로가기 누르면 로그아웃
            LoadingUIController.Instance.LoadScene("Enter"); // 인트로 씬 불러옴
        }
    }
    // 자동로그인
    /*public void doAutoLogin()
    {
        myLog.text = "...";
        if (bWaitingForAuth)
            return;

        //구글 로그인이 되어있지 않다면 
        if (!Social.localUser.authenticated)
        {
            //myLog.text = "Authenticating...";
            myLog.text = "로그인 중입니다...";
            bWaitingForAuth = true;
            //로그인 인증 처리과정 (콜백함수)      
            Social.localUser.Authenticate(AuthenticateCallback);

        }
        else
        {
            myLog.text = "로그인이 필요합니다.\n";
        }
    }*/

    // 수동로그인 
    public void OnBtnLoginClicked()
    {
        //이미 인증된 사용자는 바로 로그인 성공된다. 
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            myLog.text = "name : " + "\n" + Social.localUser.userName + "\n";            
            myLog.transform.LeanScale(Vector3.one, 0.2f);
            StartCoroutine(UserPictureLoad());
            realStart.transform.LeanScale(Vector3.one, 0.2f);
            gsLogin.transform.LeanScale(Vector3.one, 0.2f);
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    myLog.text = "name : " + "\n" + Social.localUser.userName + "\n";                    
                    myLog.transform.LeanScale(Vector3.one, 0.2f);
                    StartCoroutine(UserPictureLoad());
                    realStart.transform.LeanScale(Vector3.one, 0.2f);
                    gsLogin.transform.LeanScale(Vector3.one, 0.2f);
                }
                else
                {

                    Debug.Log("Login Fail");
                    myLog.text = "로그인 실패! 다시 시도해주세요.\n";                   
                    myLog.transform.LeanScale(Vector3.one, 0.2f);
                    realStart.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
                    gsLogin.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
                }
            });
    }
    // 수동 로그아웃 
    public void OnBtnLogoutClicked()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        myLog.text = "로그인이 필요합니다.";
    }
    // 인증 callback
    void AuthenticateCallback(bool success)
    {
        myLog.text = "Loading";
        myLog.transform.LeanScale(Vector3.one, 0.2f);        
        if (success)
        {
            // 사용자 이름을 띄어줌 
            myLog.text = "환영합니다!!!" + "\n" + Social.localUser.userName + "\n";
            myLog.transform.LeanScale(Vector3.one, 0.2f);            
            StartCoroutine(UserPictureLoad());
        }
        else
        {
            myLog.text = "로그인이 필요합니다.\n";
            myLog.transform.LeanScale(Vector3.one, 0.2f);            
        }
    }
    // 유저 이미지 받아오기 
    IEnumerator UserPictureLoad()
    {
        myLog.text = "유저 로그인 정보 로딩중 ...";        
        myLog.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        // 최초 유저 이미지 가져오기
        myImage.transform.LeanScale(Vector3.one, 0.2f);        
        Texture2D pic = Social.localUser.image;
        // 구글 아바타 이미지 여부를 확인 후 이미지 생성 
        while (pic == null)
        {
            pic = Social.localUser.image;
            yield return null;
        }
        myImage.texture = pic;
        myName.text = Social.localUser.userName;
        myLog.transform.LeanScale(Vector3.one, 0.2f);
        myLog.text = "로그인 완료되었습니다!!!";
        realLogin.interactable = false;
    }
    IEnumerator FakeUserInfoLoad()
    {
        myImage.texture = myImage.texture;
        myImage.transform.LeanScale(Vector3.one, 0.2f);
        myName.text = myName.text + "님";
        myName.transform.LeanScale(Vector3.one, 0.2f);
        myLog.text = "환영합니다!!!";       
        myLog.transform.LeanScale(Vector3.one, 0.2f);
        gsLogin.transform.LeanScale(Vector3.one, 0.2f);
        fakeStart.transform.LeanScale(Vector3.one, 0.2f);
        fakeLogin.interactable = false;
        yield return null;
    }
    public void FakeGoogleLogin()
    {
        StartCoroutine(FakeUserInfoLoad());        
    }
    public void ClickEnter()
    {
        if (Social.localUser.authenticated)
        {
            LoadingUIController.Instance.LoadScene("Main");
        }
    }
    public void ClickFakeEnter()
    {
        LoadingUIController.Instance.LoadScene("Main");
    }
    public void ClickGSLogin()
    {
        StartCoroutine(ClickStart());
    }
    public void ClickBack()
    {
        LoadingUIController.Instance.LoadScene("Enter");
    }
    private IEnumerator ClickStart()
    {
        this.transform.LeanScale(Vector2.zero, 3f).setEaseInBack();
        yield return new WaitForSeconds(0.5f);
        LoadingUIController.Instance.LoadScene("GSLogin");
    }
}
#endif