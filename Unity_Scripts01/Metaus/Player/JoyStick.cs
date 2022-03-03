using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 스틱드래그 + 제한
// 2. 드래그한만큼 캐릭터이동
public class JoyStick : MonoBehaviour
{
    public RectTransform stick, backGround;
    PlayerCtrl playerCtrl_script;
    Animator anim;
    bool isDrag;
    float limit;
    private void Start()
    {
        playerCtrl_script = GetComponent<PlayerCtrl>();
        anim = GetComponent<Animator>();
        limit = backGround.rect.width * .25f;
    }

    private void FixedUpdate()
    {
        if(isDrag) // 드래그 동안
        {
            Vector2 vec = Input.mousePosition - backGround.position;
            stick.localPosition = Vector2.ClampMagnitude(vec, limit);

            Vector3 dir = (stick.position - backGround.position).normalized;
            transform.position += dir * playerCtrl_script.speed * Time.deltaTime;
            anim.SetBool("isWalk", true);
            // 왼쪽으로 이동
            if (dir.x < 0)
            {
                transform.localScale = new Vector3(-1.2f, 1.2f, 1);
            }
            // 오른쪽으로 이동
            else
            {
                transform.localScale = new Vector3(1.2f, 1.2f, 1);
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
    }
}
