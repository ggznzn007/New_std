using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class PlayerController : MonoBehaviour
{
    public GameObject joyStick, mainView, playView, chatting, panel;
    public GameObject[] emoticons;
    public Settings2 settings2_script;
    public RectTransform stick, backGround;
    public Text userName;
    public RawImage userImage;
   


    public float speed;
    
    public bool isCantMove;
    //Animator anim;
    GameObject coll;
    bool isDrag;
    bool emoticonSwich = true;
    bool chattingSwich = true;
    float limit;
    TouchScreenKeyboard keyboard;

    private void Awake()
    {
       
    }

    private void Start()
    {
        // anim = GetComponent<Animator>();
        
        //chatting.transform.localScale = new Vector3(0, 0, 0);
        
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);


        limit = backGround.rect.width * .26f;
    }

    private void FixedUpdate()
    {
        Texture2D pic = Social.localUser.image;
        userImage.GetComponents<SpriteRenderer>();        
        userImage.texture = pic;

        
       
        userName.text = Social.localUser.userName;   


        if (settings2_script.isTouch == false)
        {
            Move(); // 조이스틱
        }
        else
        {
            // 클릭 판단
            if (Input.GetMouseButton(0))
            {
#if UNITY_EDITOR // 유니티 에디터 일 경우
                if (!EventSystem.current.IsPointerOverGameObject())
                {
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

                }
#else // 안드로이드 일 경우
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
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
                //anim.SetBool("isWalk", false);
            }
        }
    }

    public void Move()
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
        //캐릭터 삭제
        // DestoyPlayer();
    }

    public void ClickVoice()
    {

    }
    public void ClickEmoticon()
    {
        if(emoticonSwich)
        {
            panel.transform.LeanScale(Vector3.one, 0.2f);
            emoticonSwich = false;
        }
        else
        {
            panel.transform.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
            emoticonSwich = true;
        }
       
    }


    public void Emo_Hi()
    {        
        StartCoroutine("Co_Emo_Hi");     
    }

     private  IEnumerator Co_Emo_Hi()
    {
        emoticons[0].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[0].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[0].transform.LeanScale(Vector3.zero, 0.4f).setEaseOutBack();
    }

    public void Emo_Bye()
    {       
        StartCoroutine("Co_Emo_Bye");
    }
    private IEnumerator Co_Emo_Bye()
    {
        emoticons[1].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[1].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[1].transform.LeanScale(Vector3.zero, 0.4f).setEaseOutBack();
    }
    public void Emo_Like()
    {
        
        StartCoroutine("Co_Emo_Like");
    }
    private IEnumerator Co_Emo_Like()
    {
        emoticons[2].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[2].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[2].transform.LeanScale(Vector3.zero, 0.4f).setEaseOutBack();
    }
    public void Emo_Sad()
    {        
        StartCoroutine("Co_Emo_Sad");
    }
    private IEnumerator Co_Emo_Sad()
    {
        emoticons[3].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[3].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[3].transform.LeanScale(Vector3.zero, 0.4f).setEaseOutBack();
    }

    public void Emo_Sup()
    {
        StartCoroutine("Co_Emo_Sup");
    }
    private IEnumerator Co_Emo_Sup()
    {
        emoticons[4].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[4].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[4].transform.LeanScale(Vector3.zero, 0.4f).setEaseOutBack();
    }

    public void Emo_Love()
    {
        StartCoroutine("Co_Emo_Love");
    }
    private IEnumerator Co_Emo_Love()
    {
        emoticons[5].transform.position = new Vector3(this.transform.position.x, (this.transform.position.y + 1.65f), 1);
        emoticons[5].transform.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(2f);
        emoticons[5].transform.LeanScale(Vector3.zero, 0.4f).setEaseOutBack();
    }
    public void ClickChat()
    {
        if(chattingSwich)
        {
            chatting.transform.LeanScale(Vector3.one, 0.2f);
            //keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, true);
            isCantMove = true;
            chattingSwich = false;
        }
        else
        {
            chatting.transform.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
            isCantMove = false;
            chattingSwich = true;
        }
       
    }

    // 캐릭터 삭제
    public void DestoyPlayer()
    {
        Camera.main.transform.parent = null;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StairUp")
        {
            coll = null;
            transform.position = new Vector3(-6f, 23f, 0);
        }

        if (collision.tag == "StairDown")
        {
            coll = null;
            transform.position = new Vector3(-6f, 16f, 0);
        }

        if (collision.tag == "DoorIn")
        {
            coll = null;
            transform.position = new Vector3(-9f, 32f, 0);
        }

        if (collision.tag == "DoorOut")
        {
            coll = null;
            transform.position = new Vector3(-9f, 26.5f, 0);
        }

        if (collision.tag == "EnterOffice")
        {
            coll = null;
            LoadingUIController.Instance.LoadScene("OfficeRoom");
            DestoyPlayer();
        }

        if (collision.tag == "EnterPark")
        {
            coll = null;
            LoadingUIController.Instance.LoadScene("DayTime");
            DestoyPlayer();
        }
       
    }

  

}
