using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject joyStick, mainView, missionView;
    public Settings settings_script;
    public Button btn;

    Animator anim;
    GameObject coll;

    public float speed;

    public bool isCantMove;

    private void Start()
    {
        anim = GetComponent<Animator>();

        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }
    private void FixedUpdate()
    {
        if (isCantMove)
        {
            joyStick.SetActive(false);
        }
        else
        {
            Move();
        }
    }

    // 캐릭터 움직임 관리
    void Move()
    {
        if (settings_script.isJoyStick)
        {
            joyStick.SetActive(true);
        }
        else
        {
            joyStick.SetActive(false);

            // 클릭 판단
            if (Input.GetMouseButton(0))
            {
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

            }

            // 클릭 안했을 때
            else
            {
                anim.SetBool("isWalk", false);
            }

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
        if(collision.tag == "Mission")
        {
            coll = collision.gameObject;
            btn.interactable = true;
           // btn.image.sprite
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mission")
        {
            coll = null;
            btn.interactable = false;
        }
    }

    // 버튼 누르면 호출
    public void ClickButton()
    {
        // 미션스타트 호출
        coll.SendMessage("MissionStart");
        isCantMove = true; // 미션이 켜졌을 때 캐릭터 움직임 방지
        btn.interactable = false;
    }

    // 미션 종료화면 호출
    public void MissionEnd()
    {
        isCantMove = false; // 미션이 꺼졌을 때 캐릭터 다시 움직임
    }
}
