using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick2 : MonoBehaviour
{
    public RectTransform stick, backGround;
    PlayerCtrl2 playerCtrl2_script;
    Animator anim;
    bool isDrag;
    float limit;
    private void Start()
    {
        playerCtrl2_script = GetComponent<PlayerCtrl2>();
        anim = GetComponent<Animator>();
        limit = backGround.rect.width * .25f;
    }

    private void FixedUpdate()
    {
        if (isDrag) // 드래그 동안
        {
            Vector2 vec = Input.mousePosition - backGround.position;
            stick.localPosition = Vector2.ClampMagnitude(vec, limit);

            Vector3 dir = (stick.position - backGround.position).normalized;
            transform.position += dir * playerCtrl2_script.speed * Time.deltaTime;
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
