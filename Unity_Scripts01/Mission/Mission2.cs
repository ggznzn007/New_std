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
            // 드래그
            if (isDrag)
            {
                handle.position = Input.mousePosition;
                rect_handle.anchoredPosition = new Vector2(originPos.x, Mathf.Clamp(rect_handle.anchoredPosition.y, -135, -47));

                anim_Shake.enabled = true;
                // 드래그 끝
                if (Input.GetMouseButtonUp(0))
                {
                    rect_handle.anchoredPosition = originPos;
                    isDrag = false;
                    anim_Shake.enabled = false;  
                }
            }

            // 쓰레기 배출
            if (rect_handle.anchoredPosition.y <= -130)
            {
                bottom.SetActive(false);
            }
            else
            {
                bottom.SetActive(true);
            }

            // 쓰레기 삭제
            for (int i = 0; i < rubbish.childCount; i++)
            {
                if (rubbish.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y <= -600)
                {
                    Destroy(rubbish.GetChild(i).gameObject);
                }
            }

            // 성공여부 체크
            if (rubbish.childCount == 0)
            {
                MissionSuccess();
                isPlay = false;
            }

        }
    }

    // 미션 시작
    public void MissionStart()
    {
        anim.SetBool("isUp", true);
        playerCtrl_script = FindObjectOfType<PlayerCtrl>(); // 캐릭터가 중간에 호출되기 때문에 여기서 호출

        // 초기화
        for (int i = 0; i < rubbish.childCount; i++)
        {
            Destroy(rubbish.GetChild(i).gameObject);
        }

        // 쓰레기 생성
        for (int i = 0; i < 10; i++)
        {
            // 사과
            GameObject rubbish4 = Instantiate(Resources.Load("Rubbish/Rubbish4"), rubbish) as GameObject;
            rubbish4.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish4.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
            // 캔
            GameObject rubbish5 = Instantiate(Resources.Load("Rubbish/Rubbish5"), rubbish) as GameObject;
            rubbish5.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish5.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
        }
        for (int i = 0; i < 5; i++)
        {
            // 병
            GameObject rubbish1 = Instantiate(Resources.Load("Rubbish/Rubbish1"), rubbish) as GameObject;
            rubbish1.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish1.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
            // 생선
            GameObject rubbish2 = Instantiate(Resources.Load("Rubbish/Rubbish2"), rubbish) as GameObject;
            rubbish2.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish2.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
            // 비닐
            GameObject rubbish3 = Instantiate(Resources.Load("Rubbish/Rubbish3"), rubbish) as GameObject;
            rubbish3.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-170, 170), Random.Range(-170, 170));
            rubbish3.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0, 170));
        }
        isPlay = true;
    }
    // 엑스버튼 누르면 호출
    public void ClickCancel()
    {
        anim.SetBool("isUp", false);
        playerCtrl_script.MissionEnd();
    }

    // 손잡이 누르면 호출
    public void ClickHandle()
    {
        isDrag = true;
    }

    // 미션 성공시 호출
    public void MissionSuccess()
    {
        ClickCancel();
        missionCtrl_script.MissionSuccess(GetComponent<CircleCollider2D>());
    }
}




