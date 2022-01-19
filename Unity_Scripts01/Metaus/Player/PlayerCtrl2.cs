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
        
        // �̼�
       /* if (isMission)
        {
            btn.GetComponent<Image>().sprite = use;

            text_cool.text = "";
        }
        // ų
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

       /* // ��Ÿ��
        if (isCool)
        {
            timer -= Time.deltaTime;
            text_cool.text = Mathf.Ceil(timer).ToString();// Ceil == �ø�
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

        // �ִϸ��̼��� �����ٸ�
        if (isAnim && killctrl_script.kill_anim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            killctrl_script.kill_anim.SetActive(false);
            killctrl_script.Kill();
            isCantMove = false;
            isAnim = false;
        }
    }

    // ���� ���� ������ ȣ��
    public void ClickQuit()
    {
        mainView.SetActive(true);
        playView.SetActive(false);

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

    // ĳ���� ������ ����
    void Move()
    {
       /* if (settings2_script.isJoyStick)
        {
            joyStick.SetActive(true);
        }
        else
        {
            joyStick.SetActive(false);

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
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    // ���������� �̵�  
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
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
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    // ���������� �̵�  
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }

                }
#endif
            }

            // Ŭ�� ������ ��
            else
            {
                anim.SetBool("isWalk", false);
            }

        }*/
    }

    // ĳ���� ����
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

    // ��ư ������ ȣ��
    public void ClickButton()
    {
        // �̼� �� ���
        if (isMission)
        {
            // �̼ǽ�ŸƮ ȣ��
            coll.SendMessage("MissionStart");
        }

        // ų �� ���
        else
        {
            Kill();
        }
        isCantMove = true; // �̼��� ������ �� ĳ���� ������ ����
       // btn.interactable = false;
    }

    void Kill()
    {
        // ���̴� �ִϸ��̼�
        killctrl_script.kill_anim.SetActive(true);
        isAnim = true;

        // ���� �̹���
        coll.SendMessage("Dead");

        // ���� NPC�� �ٽ� ���� �� ����
        coll.GetComponent<CircleCollider2D>().enabled = false;
    }

    // �̼� ����ȭ�� ȣ��
    public void MissionEnd()
    {
        isCantMove = false; // �̼��� ������ �� ĳ���� �ٽ� ������
    }
}
