using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mission2 : MonoBehaviour
{
    public Transform rubbish, handle;
    public GameObject bottom;
    public Animator anim_Shake;

    Animator anim;
    PlayerCtrl playerCtrl_script;
    RectTransform rect_handle;
    MissionCtrl missionCtrl_script;

    bool isDrag, isPlay;
    Vector2 originPos;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rect_handle = handle.GetComponent<RectTransform>();
        originPos = rect_handle.anchoredPosition;
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
                rect_handle.anchoredPosition = new Vector2(originPos.x, Mathf.Clamp(rect_handle.anchoredPosition.y, -135, -47));

                anim_Shake.enabled = true;
                // �巡�� ��
                if (Input.GetMouseButtonUp(0))
                {
                    rect_handle.anchoredPosition = originPos;
                    isDrag = false;
                    anim_Shake.enabled = false;  
                }
            }

            // ������ ����
            if (rect_handle.anchoredPosition.y <= -130)
            {
                bottom.SetActive(false);
            }
            else
            {
                bottom.SetActive(true);
            }

            // ������ ����
            for (int i = 0; i < rubbish.childCount; i++)
            {
                if (rubbish.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y <= -600)
                {
                    Destroy(rubbish.GetChild(i).gameObject);
                }
            }

            // �������� üũ
            if (rubbish.childCount == 0)
            {
                MissionSuccess();
                isPlay = false;
            }

        }
    }

    // �̼� ����
    public void MissionStart()
    {
        anim.SetBool("isUp", true);
        playerCtrl_script = FindObjectOfType<PlayerCtrl>(); // ĳ���Ͱ� �߰��� ȣ��Ǳ� ������ ���⼭ ȣ��

        // �ʱ�ȭ
        for (int i = 0; i < rubbish.childCount; i++)
        {
            Destroy(rubbish.GetChild(i).gameObject);
        }

        // ������ ����
        for (int i = 0; i < 10; i++)
        {
            // ���
            GameObject rubbish4 = Instantiate(Resources.Load("Rubbish/Rubbish4"), rubbish) as GameObject;
            rubbish4.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish4.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
            // ĵ
            GameObject rubbish5 = Instantiate(Resources.Load("Rubbish/Rubbish5"), rubbish) as GameObject;
            rubbish5.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish5.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
        }
        for (int i = 0; i < 5; i++)
        {
            // ��
            GameObject rubbish1 = Instantiate(Resources.Load("Rubbish/Rubbish1"), rubbish) as GameObject;
            rubbish1.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish1.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
            // ����
            GameObject rubbish2 = Instantiate(Resources.Load("Rubbish/Rubbish2"), rubbish) as GameObject;
            rubbish2.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish2.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
            // ���
            GameObject rubbish3 = Instantiate(Resources.Load("Rubbish/Rubbish3"), rubbish) as GameObject;
            rubbish3.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish3.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
        }
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




