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
        myLog.text = "���� �α��� ���ּ���";
        realLogin.interactable = true;
        fakeLogin.interactable = true;
        // ���� ���Ӽ��� Ȱ��ȭ (�ʱ�ȭ)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ((PlayGamesPlatform)Social.Active).SignOut(); // �ڷΰ��� ������ �α׾ƿ�
            LoadingUIController.Instance.LoadScene("Enter"); // ��Ʈ�� �� �ҷ���
        }
    }
    // �ڵ��α���
    /*public void doAutoLogin()
    {
        myLog.text = "...";
        if (bWaitingForAuth)
            return;

        //���� �α����� �Ǿ����� �ʴٸ� 
        if (!Social.localUser.authenticated)
        {
            //myLog.text = "Authenticating...";
            myLog.text = "�α��� ���Դϴ�...";
            bWaitingForAuth = true;
            //�α��� ���� ó������ (�ݹ��Լ�)      
            Social.localUser.Authenticate(AuthenticateCallback);

        }
        else
        {
            myLog.text = "�α����� �ʿ��մϴ�.\n";
        }
    }*/

    // �����α��� 
    public void OnBtnLoginClicked()
    {
        //�̹� ������ ����ڴ� �ٷ� �α��� �����ȴ�. 
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
                    myLog.text = "�α��� ����! �ٽ� �õ����ּ���.\n";                   
                    myLog.transform.LeanScale(Vector3.one, 0.2f);
                    realStart.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
                    gsLogin.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
                }
            });
    }
    // ���� �α׾ƿ� 
    public void OnBtnLogoutClicked()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        myLog.text = "�α����� �ʿ��մϴ�.";
    }
    // ���� callback
    void AuthenticateCallback(bool success)
    {
        myLog.text = "Loading";
        myLog.transform.LeanScale(Vector3.one, 0.2f);        
        if (success)
        {
            // ����� �̸��� ����� 
            myLog.text = "ȯ���մϴ�!!!" + "\n" + Social.localUser.userName + "\n";
            myLog.transform.LeanScale(Vector3.one, 0.2f);            
            StartCoroutine(UserPictureLoad());
        }
        else
        {
            myLog.text = "�α����� �ʿ��մϴ�.\n";
            myLog.transform.LeanScale(Vector3.one, 0.2f);            
        }
    }
    // ���� �̹��� �޾ƿ��� 
    IEnumerator UserPictureLoad()
    {
        myLog.text = "���� �α��� ���� �ε��� ...";        
        myLog.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        // ���� ���� �̹��� ��������
        myImage.transform.LeanScale(Vector3.one, 0.2f);        
        Texture2D pic = Social.localUser.image;
        // ���� �ƹ�Ÿ �̹��� ���θ� Ȯ�� �� �̹��� ���� 
        while (pic == null)
        {
            pic = Social.localUser.image;
            yield return null;
        }
        myImage.texture = pic;
        myName.text = Social.localUser.userName;
        myLog.transform.LeanScale(Vector3.one, 0.2f);
        myLog.text = "�α��� �Ϸ�Ǿ����ϴ�!!!";
        realLogin.interactable = false;
    }
    IEnumerator FakeUserInfoLoad()
    {
        myImage.texture = myImage.texture;
        myImage.transform.LeanScale(Vector3.one, 0.2f);
        myName.text = myName.text + "��";
        myName.transform.LeanScale(Vector3.one, 0.2f);
        myLog.text = "ȯ���մϴ�!!!";       
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