using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrl2 : MonoBehaviour
{
    public GameObject joyStick, mainView, playView,chatting;
    public Settings2 settings2_script;
    

    // public Button btn;
    //public Sprite use, kill;
    // public Text text_cool;

    Animator anim;
    GameObject coll;
    KillCtrl killctrl_script;

   

    public float speed;

    public bool isCantMove, isMission;

    float timer;
    bool isCool, isAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        joyStick.SetActive(true);
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        
        // 미션
       /* if (isMission)
        {
            btn.GetComponent<Image>().sprite = use;

            text_cool.text = "";
        }
        // 킬
        else
        {
            killctrl_script = FindObjectOfType<KillCtrl>();

            btn.GetComponent<Image>().sprite = kill;

            timer = 5;
            isCool = true;
        }*/
    }
    private void FixedUpdate()
    {

       /* // 쿨타임
        if (isCool)
        {
            timer -= Time.deltaTime;
            text_cool.text = Mathf.Ceil(timer).ToString();// Ceil == 올림
            if (text_cool.text == "0")
            {
                text_cool.text = "";
                isCool = false;
            }
        }*/

        if (isCantMove)
        {
            joyStick.SetActive(false);
        }
        else
        {
            Move();
        }

        // 애니메이션이 끝났다면
        if (isAnim && killctrl_script.kill_anim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            killctrl_script.kill_anim.SetActive(false);
            killctrl_script.Kill();
            isCantMove = false;
            isAnim = false;
        }
    }

    // 게임 종료 누르면 호출
    public void ClickQuit()
    {
        mainView.SetActive(true);
        playView.SetActive(false);

        //캐릭터 삭제
        DestoyPlayer();
    }

    public void ClickChat()
    {
        chatting.SetActive(true);
        isCantMove = true;
    }

    public void ClickChatQuit()
    {
        chatting.SetActive(false);
        isCantMove = false;
    }

    // 캐릭터 움직임 관리
    void Move()
    {
       /* if (settings2_script.isJoyStick)
        {
            joyStick.SetActive(true);
        }
        else
        {
            joyStick.SetActive(false);

            // 클릭 판단
            if (Input.GetMouseButton(0))
            {
#if UNITY_EDITOR // 유니티 에디터 일 경우
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f)).normalized;
                    transform.position += dir * speed * Time.deltaTime;
                    anim.SetBool("isWalk", true);
                    // 왼쪽으로 이동
                    if (dir.x < 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    // 오른쪽으로 이동  
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }

                }
#else // 안드로이드 일 경우
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f)).normalized;
                    transform.position += dir * speed * Time.deltaTime;
                    anim.SetBool("isWalk", true);
                    // 왼쪽으로 이동
                    if (dir.x < 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    // 오른쪽으로 이동  
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }

                }
#endif
            }

            // 클릭 안했을 때
            else
            {
                anim.SetBool("isWalk", false);
            }

        }*/
    }

    // 캐릭터 삭제
    public void DestoyPlayer()
    {
        Camera.main.transform.parent = null;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mission" && isMission)
        {
            coll = collision.gameObject;
            //btn.interactable = true;

        }
        if (collision.tag == "NPC" && !isMission && !isCool)
        {
            coll = collision.gameObject;
            //btn.interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mission" && isMission)
        {
            coll = null;
            //btn.interactable = false;
        }

        if (collision.tag == "NPC" && !isMission)
        {
            coll = null;
            //btn.interactable = false;
        }
    }

    // 버튼 누르면 호출
    public void ClickButton()
    {
        // 미션 일 경우
        if (isMission)
        {
            // 미션스타트 호출
            coll.SendMessage("MissionStart");
        }

        // 킬 일 경우
        else
        {
            Kill();
        }
        isCantMove = true; // 미션이 켜졌을 때 캐릭터 움직임 방지
       // btn.interactable = false;
    }

    void Kill()
    {
        // 죽이는 애니메이션
        killctrl_script.kill_anim.SetActive(true);
        isAnim = true;

        // 죽은 이미지
        coll.SendMessage("Dead");

        // 죽은 NPC는 다시 죽일 수 없게
        coll.GetComponent<CircleCollider2D>().enabled = false;
    }

    // 미션 종료화면 호출
    public void MissionEnd()
    {
        isCantMove = false; // 미션이 꺼졌을 때 캐릭터 다시 움직임
    }
}
