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

    // 미션 시작
    public void MissionStart()
    {
        anim.SetBool("isUp", true);
        playerCtrl_script = FindObjectOfType<PlayerCtrl>(); // 캐릭터가 중간에 호출되기 때문에 여기서 호출

        // 초기화
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.white;
        }

        // 랜덤
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, 7);

            images[rand].color = blue;
        }
    }

    // 엑스버튼 누르면 호출
    public void ClickCancel()
    {
        anim.SetBool("isUp", false);
        playerCtrl_script.MissionEnd();
    }

    // 육각버튼 누르면 호출
    public void ClickButton()
    {
        Image img = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        if (img.color == Color.white)
        {
            // 색변경
            img.color = blue;
        }
        else
        {
            img.color = Color.white;
        }

        // 성공여부 체크
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
            //성공
            Invoke("MissionSuccess", .2f); // 시간차를 두고 미션창 내림
        }
    }

    // 미션 성공시 호출
    public void MissionSuccess()
    {
        ClickCancel();
        missionCtrl_script.MissionSuccess(GetComponent<CircleCollider2D>());
    }

}
