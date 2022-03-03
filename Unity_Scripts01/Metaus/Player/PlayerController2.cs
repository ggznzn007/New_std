using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    public GameObject joyStick, chatting, panel, friendsPanel;
    public GameObject[] emoticons; // 이모티콘
    public Settings3 settings3_script; // 설정
    public RectTransform stick, backGround; // 조이스틱
    public Text userName;       // 구글 유저 이름
    //public RawImage userImage; // 구글 유저 이미지
    public SpriteRenderer userImg; // 구글 유저 이미지를 캐릭터의 이미지로 
    public GameObject voiceLockImage, voiceUnLockImage, voiceBtn;
    public Sprite[] voice_Btn;
    public GameObject nightTime, officeRoom;

    public float speed; // 캐릭터 이동 속도
    public bool isCantMove; // 캐릭터 움직임 여부                          

    bool isDrag; // 드래그 여부
    bool friendsSwich = true;
    bool emoticonSwich = true;
    bool chattingSwich = true;
    bool voiceSwich = true;

    float limit; // 조이스틱 영역제한

    public void Start()
    {
        nightTime = GameObject.FindGameObjectWithTag("NightTime");
        officeRoom = GameObject.FindGameObjectWithTag("OfficeRoom");
        nightTime.gameObject.SetActive(true);
        officeRoom.gameObject.SetActive(false);
        // Camera.main.transform.parent = transform;
        // Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        limit = backGround.rect.width * .27f;
        // anim = GetComponent<Animator>();        
    }
    private void Update()
    {
        GetGPGSImage();
    }
    public void GetGPGSImage()
    {
        if (Social.localUser.authenticated)
        {
            Texture2D pic = Social.localUser.image;
            Rect rect = new Rect(0, 0, pic.width, pic.height);
            Sprite sprite = Sprite.Create(pic, rect, new Vector2(.5f, .5f));
            userImg.size = new Vector2(5.12f, 5.12f);
            userImg.sprite = sprite;
            userName.text = Social.localUser.userName;
        }
        else
        {
            userImg.sprite = userImg.sprite;
            userName.text = userName.text;
        }
    }
    private void FixedUpdate()
    {

        if (settings3_script.isTouch == false)
        {
            JoyStickMove(); // 조이스틱
        }
        else
        {
            // 클릭 판단
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
#if UNITY_EDITOR // 유니티 에디터 일 경우                
                Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f)).normalized;
                transform.position += dir * speed * Time.deltaTime;
                //anim.SetBool("isWalk", true);
                // 왼쪽으로 이동
                if (dir.x < 0)
                {
                    transform.localScale = new Vector3(0.3f, 0.3f, 1);
                }
                // 오른쪽으로 이동  
                else
                {
                    transform.localScale = new Vector3(0.3f, 0.3f, 1);
                }

#else // 안드로이드 일 경우
            if((Input.touchCount > 0)&&(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
             {                
               Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f)).normalized;
               transform.position += dir * speed * Time.deltaTime;
                //anim.SetBool("isWalk", true);
               // 왼쪽으로 이동
               if (dir.x < 0)
                {
                  transform.localScale = new Vector3(0.3f,0.3f, 1);
                }
               // 오른쪽으로 이동  
               else
                {
                  transform.localScale = new Vector3(0.3f,0.3f, 1);
                }                
             }
#endif
            }
            // 클릭 안했을 때
            else
            {

            }
        }
    }
    public void JoyStickMove()
    {
        if (!isCantMove && isDrag)
        {
            // 드래그 동안        
            Vector2 vec = Input.mousePosition - backGround.position;
            stick.localPosition = Vector2.ClampMagnitude(vec, limit);

            Vector3 dir = (stick.position - backGround.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            //anim.SetBool("isWalk", true);
            // 왼쪽으로 이동
            if (dir.x < 0)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            // 오른쪽으로 이동
            else
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            if (Input.GetMouseButtonUp(0)) // 드래그 끝나면
            {
                stick.localPosition = new Vector3(0, 0, 0);
                //anim.SetBool("isWalk", false);

                isDrag = false;
            }
        }
    }
    // 스틱을 누르면 호출
    public void ClickStick()
    {
        isDrag = true;
        isCantMove = false;
    }
    // 게임 종료 누르면 호출
    public void ClickQuit()
    {
        LoadingUIController.Instance.LoadScene("Main");
        ((PlayGamesPlatform)Social.Active).SignOut();
        DestoyPlayer(); //캐릭터 삭제
    }
    public void ClickFriends()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        else
        {
            if (friendsSwich)
            {
                joyStick.gameObject.SetActive(false);
                StartCoroutine(FriendsPanelOpen());
                StartCoroutine(EmoticonPanelClose());
                StartCoroutine(ChattingClose());
                settings3_script.StartCoroutine("SettingClose");
            }
            else
            {
                joyStick.gameObject.SetActive(true);
                StartCoroutine(FriendsPanelClose());
            }
        }
    }
    public void ClickVoice()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        else
        {
            if (voiceSwich)
            {
                StartCoroutine(VoiceLock());
            }
            else
            {
                StartCoroutine(VoiceUnLock());
            }
        }
    }
    public void ClickEmoticon()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        else
        {
            if (emoticonSwich)
            {
                StartCoroutine(EmoticonPanelOpen());
                StartCoroutine(FriendsPanelClose());
                StartCoroutine(ChattingClose());
                settings3_script.StartCoroutine("SettingClose");
            }
            else
            {
                StartCoroutine(EmoticonPanelClose());
            }
        }
    }
    public void ClickChat()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        else
        {
            if (chattingSwich)
            {
                joyStick.gameObject.SetActive(false);
                StartCoroutine(ChattingOpen());
                StartCoroutine(EmoticonPanelClose());
                StartCoroutine(FriendsPanelClose());
                settings3_script.StartCoroutine("SettingClose");
            }
            else
            {
                joyStick.gameObject.SetActive(true);
                StartCoroutine(ChattingClose());
            }
        }

    }
    public void Emo_Hi()
    {
        StartCoroutine(Co_Emo_Hi());
    }
    public void Emo_Bye()
    {
        StartCoroutine(Co_Emo_Bye());
    }
    public void Emo_Like()
    {
        StartCoroutine(Co_Emo_Like());
    }
    public void Emo_Sad()
    {
        StartCoroutine(Co_Emo_Sad());
    }
    public void Emo_Sup()
    {
        StartCoroutine(Co_Emo_Sup());
    }
    public void Emo_Love()
    {
        StartCoroutine(Co_Emo_Love());
    }
    // 캐릭터 삭제
    public void DestoyPlayer()
    {
        Camera.main.transform.parent = null;

        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag == "StairUp")
        {

            transform.position = new Vector3(-13f, 24f, 0);
        }

        if (collision.tag == "StairDown")
        {

            transform.position = new Vector3(-15f, 11f, 0);
        }

        if (collision.tag == "EnterOffice")
        {

            this.transform.position = new Vector3(0f, 40.5f, 0);
            Camera.main.orthographicSize = 15f;
            nightTime.gameObject.SetActive(false);
            officeRoom.gameObject.SetActive(true);
        }

        if (collision.tag == "EnterPark")
        {
            // coll = null;
            this.transform.position = new Vector3(-6, 14, 0);
            Camera.main.orthographicSize = 10f;
            nightTime.gameObject.SetActive(true);
            officeRoom.gameObject.SetActive(false);
        }



    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {

        }
    }
    IEnumerator FriendsPanelOpen()
    {
        friendsPanel.transform.LeanMoveLocal(new Vector3(0f, 180f), 0.2f);
        friendsPanel.transform.LeanScale(Vector3.one, 0.2f);
        friendsSwich = false;
        yield return null;
    }
    IEnumerator FriendsPanelClose()
    {
        friendsPanel.transform.LeanMoveLocal(new Vector3(300f, 1020f), 0.2f);
        friendsPanel.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        friendsSwich = true;
        yield return null;
    }
    IEnumerator VoiceLock()
    {
        voiceBtn.GetComponent<Image>().sprite = voice_Btn[1];
        voiceLockImage.transform.LeanMoveLocal(new Vector3(0f, 250f), 0.2f);
        voiceLockImage.transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(1f);
        voiceLockImage.transform.LeanMoveLocal(new Vector3(0f, 0f), 0.2f);
        voiceLockImage.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        voiceSwich = false;
    }
    IEnumerator VoiceUnLock()
    {
        voiceBtn.GetComponent<Image>().sprite = voice_Btn[0];
        voiceUnLockImage.transform.LeanMoveLocal(new Vector3(0f, 250f), 0.2f);
        voiceUnLockImage.transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(1f);
        voiceUnLockImage.transform.LeanMoveLocal(new Vector3(0f, 0f), 0.2f);
        voiceUnLockImage.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        voiceSwich = true;
    }
    IEnumerator EmoticonPanelOpen()
    {
        panel.transform.LeanMoveLocal(new Vector3(287.59f, -459.44f), 0.2f);
        panel.transform.LeanScale(Vector3.one, 0.2f);
        emoticonSwich = false;
        yield return null;
    }
    IEnumerator EmoticonPanelClose()
    {
        panel.transform.LeanMoveLocal(new Vector3(287.59f, -838.82f), 0.2f);
        panel.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        emoticonSwich = true;
        yield return null;
    }
    IEnumerator ChattingOpen()
    {
        chatting.transform.LeanMoveLocal(new Vector3(0f, 250f), 0.2f);
        chatting.transform.LeanScale(Vector3.one, 0.2f);
        isCantMove = true;
        chattingSwich = false;
        yield return null;
    }
    IEnumerator ChattingClose()
    {
        chatting.transform.LeanMoveLocal(new Vector3(448f, -903f), 0.2f);
        chatting.transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
        isCantMove = false;
        chattingSwich = true;
        yield return null;
    }

    IEnumerator Co_Emo_Hi()
    {
        emoticons[0].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[0].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[0].transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
    }
    IEnumerator Co_Emo_Bye()
    {
        emoticons[1].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[1].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[1].transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
    }
    IEnumerator Co_Emo_Like()
    {
        emoticons[2].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[2].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[2].transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
    }
    IEnumerator Co_Emo_Sad()
    {
        emoticons[3].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[3].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[3].transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
    }
    IEnumerator Co_Emo_Sup()
    {
        emoticons[4].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[4].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[4].transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
    }
    IEnumerator Co_Emo_Love()
    {
        emoticons[5].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[5].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[5].transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack();
    }
}
