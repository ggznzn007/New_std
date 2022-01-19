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
        if (isDrag) // �巡�� ����
        {
            Vector2 vec = Input.mousePosition - backGround.position;
            stick.localPosition = Vector2.ClampMagnitude(vec, limit);

            Vector3 dir = (stick.position - backGround.position).normalized;
            transform.position += dir * playerCtrl2_script.speed * Time.deltaTime;
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
    }
}
