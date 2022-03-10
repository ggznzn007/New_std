using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mission1 : MonoBehaviour
{
    public Color blue;
    public Image[] images;

    Animator anim;
    PlayerCtrl playerCtrl_script;
    MissionCtrl missionCtrl_script;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        missionCtrl_script = FindObjectOfType<MissionCtrl>();
    }

    // �̼� ����
    public void MissionStart()
    {
        anim.SetBool("isUp", true);
        playerCtrl_script = FindObjectOfType<PlayerCtrl>(); // ĳ���Ͱ� �߰��� ȣ��Ǳ� ������ ���⼭ ȣ��

        // �ʱ�ȭ
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.white;
        }

        // ����
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, 7);

            images[rand].color = blue;
        }
    }

    // ������ư ������ ȣ��
    public void ClickCancel()
    {
        anim.SetBool("isUp", false);
        playerCtrl_script.MissionEnd();
    }

    // ������ư ������ ȣ��
    public void ClickButton()
    {
        Image img = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        if (img.color == Color.white)
        {
            // ������
            img.color = blue;
        }
        else
        {
            img.color = Color.white;
        }

        // �������� üũ
        int count = 0;
        for (int i = 0; i < images.Length; i++)
        {
            if(images[i].color==Color.white)
            {
                count++;
            }
        }

        if(count ==images.Length)
        {
            //����
            Invoke("MissionSuccess", .2f); // �ð����� �ΰ� �̼�â ����
        }
    }

    // �̼� ������ ȣ��
    public void MissionSuccess()
    {
        ClickCancel();
        missionCtrl_script.MissionSuccess(GetComponent<CircleCollider2D>());
    }

}
