using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mission5 : MonoBehaviour
{
    public Transform rotate, handle;
    public Color blue, red;

    Animator anim;
    PlayerCtrl playerCtrl_script;
    MissionCtrl missionCtrl_script;
    RectTransform rect_handle;

    bool isDrag, isPlay;
    float rand;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rect_handle = handle.GetComponent<RectTransform>();
        missionCtrl_script = FindObjectOfType<MissionCtrl>();
    }

    private void FixedUpdate()
    {
        if (isPlay)
        {
            // �巡��
            if (isDrag)
            {
                handle.position = Input.mousePosition;
                rect_handle.anchoredPosition = new Vector2(184, Mathf.Clamp(rect_handle.anchoredPosition.y, -238, 238));


                // �巡�� ��
                if (Input.GetMouseButtonUp(0))
                {
                    // �������� üũ
                    if (rect_handle.anchoredPosition.y > -5 && rect_handle.anchoredPosition.y < 5)
                    {
                        Invoke("MissionSuccess", 0.3f);
                        isPlay = true;
                    }

                    isDrag = false;

                }
            }

            rotate.eulerAngles = new Vector3(0, 0, 90 * rect_handle.anchoredPosition.y / 238);

         
            // ������
            if (rect_handle.anchoredPosition.y > -5 && rect_handle.anchoredPosition.y < 5)
            {
                rotate.GetComponent<Image>().color = blue;
            }
            else
            {
                rotate.GetComponent<Image>().color = red;
            }

        }
    }

    // �̼� ����
    public void MissionStart()
    {
        anim.SetBool("isUp", true);
        playerCtrl_script = FindObjectOfType<PlayerCtrl>(); // ĳ���Ͱ� �߰��� ȣ��Ǳ� ������ ���⼭ ȣ��

        // �ʱ�ȭ
        rand = 0;

        // ����
        rand = Random.Range(-238, 238);

        while (rand <= -10 && rand <= 10)
        {
            rand = Random.Range(-238, 238);
        }
        rect_handle.anchoredPosition = new Vector2(184, rand);
        isPlay = true;

    }
    // ������ư ������ ȣ��
    public void ClickCancel()
    {
        anim.SetBool("isUp", false);
        playerCtrl_script.MissionEnd();
    }

    // ������ ������ ȣ��
    public void ClickHandle()
    {
        isDrag = true;
    }

    // �̼� ������ ȣ��
    public void MissionSuccess()
    {
        ClickCancel();
        missionCtrl_script.MissionSuccess(GetComponent<CircleCollider2D>());
    }
}




