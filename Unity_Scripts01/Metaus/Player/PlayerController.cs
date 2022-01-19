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
            Move(); // ���̽�ƽ
        }
        else
        {
            // Ŭ�� �Ǵ�
            if (Input.GetMouseButton(0))
            {
#if UNITY_EDITOR // ����Ƽ ������ �� ���
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f)).normalized;
                    transform.position += dir * speed * Time.deltaTime;
                    anim.SetBool("isWalk", true);
                    // �������� �̵�
                    if (dir.x < 0)
                    {
                        transform.localScale = new Vector3(-0.3f, 0.3f, 1);
                    }
                    // ���������� �̵�  
                    else
                    {
                        transform.localScale = new Vector3(0.3f, 0.3f, 1);
                    }

                }
#else // �ȵ���̵� �� ���
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f)).normalized;
                    transform.position += dir * speed * Time.deltaTime;
                    anim.SetBool("isWalk", true);
                    // �������� �̵�
                    if (dir.x < 0)
                    {
                        transform.localScale = new Vector3(-0.3f,0.3f, 1);
                    }
                    // ���������� �̵�  
                    else
                    {
                        transform.localScale = new Vector3(0.3f,0.3f, 1);
                    }

                }
#endif
            }

            // Ŭ�� ������ ��
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
            // �巡�� ����        
            Vector2 vec = Input.mousePosition - backGround.position;
            stick.localPosition = Vector2.ClampMagnitude(vec, limit);

            Vector3 dir = (stick.position - backGround.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            anim.SetBool("isWalk", true);
            // �������� �̵�
            if (dir.x < 0)
            {
                transform.localScale = new Vector3(-0.3f, 0.3f, 1);
            }
            // ���������� �̵�
            else
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            if (Input.GetMouseButtonUp(0)) // �巡�� ������
            {
                stick.localPosition = new Vector3(0, 0, 0);
                anim.SetBool("isWalk", false);

                isDrag = false;
            }
        }
    }

    // ��ƽ�� ������ ȣ��
    public void ClickStick()
    {
        isDrag = true;
        isCantMove = false;
    }
    // ���� ���� ������ ȣ��
    public void ClickQuit()
    {
        /* mainView.SetActive(true);
         playView.SetActive(false);*/
        LoadingSceneCtrl.LoadScene("Main_220118");

        //ĳ���� ����
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

    // ĳ���� ����
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
