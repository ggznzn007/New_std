using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


#if UNITY_ANDROID
public class GPGSManager : MonoBehaviour
{
    public Text myName;
    public Text myLog;
    public RawImage myImage;
    public Button start, gsLogin;

    private bool bWaitingForAuth = false;

    private void Awake()
    {
        myLog.text = "구글 로그인 해주세요";
        myImage.gameObject.SetActive(false);
        // 구글 게임서비스 활성화 (초기화)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        //게임시작시 자동로그인
        //doAutoLogin();
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
    public void doAutoLogin()
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
    }

    // 수동로그인 
    public void OnBtnLoginClicked()
    {
        //이미 인증된 사용자는 바로 로그인 성공된다. 
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            myLog.text = "name : " + "\n" + Social.localUser.userName + "\n";
            StartCoroutine(UserPictureLoad());
            start.transform.LeanScale(Vector3.one, 0.2f);
            gsLogin.gameObject.SetActive(true);
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    myLog.text = "name : " + "\n" + Social.localUser.userName + "\n";
                    StartCoroutine(UserPictureLoad());
                    start.transform.LeanScale(Vector3.one, 0.2f);
                    gsLogin.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Login Fail");
                    myLog.text = "로그인 실패! 다시 시도해주세요.\n";

                    start.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
                    gsLogin.gameObject.SetActive(false);
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
        if (success)
        {
            // 사용자 이름을 띄어줌 
            myLog.text = "환영합니다!!!" + "\n" + Social.localUser.userName + "\n";

            StartCoroutine(UserPictureLoad());
        }
        else
        {
            myLog.text = "로그인이 필요합니다.\n";
        }
    }

    // 유저 이미지 받아오기 
    IEnumerator UserPictureLoad()
    {
        myLog.text = "유저 로그인 정보 로딩중 ...";
        // 최초 유저 이미지 가져오기
        myImage.gameObject.SetActive(true);
        Texture2D pic = Social.localUser.image;

        // 구글 아바타 이미지 여부를 확인 후 이미지 생성 
        while (pic == null)
        {
            pic = Social.localUser.image;

            yield return null;
        }

        myImage.texture = pic;
        myName.text = Social.localUser.userName;
        myLog.text = "로그인 완료되었습니다!!!";
    }



    public void ClickEnter()
    {
        if (Social.localUser.authenticated)
        {
            LoadingUIController.Instance.LoadScene("Main");
        }
    }

    public void ClickGSLogin()
    {
        StartCoroutine(ClickStart());
        /*if (Social.localUser.authenticated)
        {
            
        }*/
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