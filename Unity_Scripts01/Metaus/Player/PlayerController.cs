using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameObject joyStick, mainView, playView, chatting;
    public Settings2 settings2_script;
    public RectTransform stick, backGround;    

    public float speed;

    public bool isCantMove;

    Animator anim;
    GameObject coll;
    bool isDrag;
    float limit;

    private void Start()
    {
        anim = GetComponent<Animator>();
       
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
       

        limit = backGround.rect.width * .25f;
    }

    private void FixedUpdate()
    {
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
                    anim.SetBool("isWalk", true);
                    // 왼쪽으로 이동
                    if (dir.x < 0)
                    {
                        transform.localScale = new Vector3(-0.3f, 0.3f, 1);
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
                    anim.SetBool("isWalk", true);
                    // 왼쪽으로 이동
                    if (dir.x < 0)
                    {
                        transform.localScale = new Vector3(-0.3f,0.3f, 1);
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
                anim.SetBool("isWalk", false);
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
            anim.SetBool("isWalk", true);
            // 왼쪽으로 이동
            if (dir.x < 0)
            {
                transform.localScale = new Vector3(-0.3f, 0.3f, 1);
            }
            // 오른쪽으로 이동
            else
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            if (Input.GetMouseButtonUp(0)) // 드래그 끝나면
            {
                stick.localPosition = new Vector3(0, 0, 0);
                anim.SetBool("isWalk", false);

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
        /* mainView.SetActive(true);
         playView.SetActive(false);*/
        LoadingSceneCtrl.LoadScene("Main_220118");

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
    }
}
