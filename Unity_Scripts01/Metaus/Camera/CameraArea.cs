using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    public static CameraArea instance;

    [SerializeField] float smoothTimeX, smoothTimeY;

    [Header("Now Position")]
    [SerializeField] Vector2 velocity;
    [SerializeField] GameObject player;
    [Header("Control Area")]
    [SerializeField] Vector2 minPos, maxPos;
   
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            this.gameObject.SetActive(false);
        }
    }    

    private void FixedUpdate()
    {
        CameraMoving();
    }

    public void CameraMoving() // ĳ������ ���� ���� ī�޶� �̵��ϵ��� �ϴ� �޼���
    {
        // Mathf.SmoothDamp�� õõ�� ���� ������Ű�� �޼����̴�.
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
        // ī�޶� �̵�
        transform.position = new Vector3(posX, posY, transform.position.z);
        //Mathf.Clamp(���� ��, �ִ밪, �ּҰ�);  ���� ���� �ִ밪������ ��ȯ���ְ� �ּҰ����� ������ �� �ּҰ������� ��ȯ�մϴ�.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
        Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
        Mathf.Clamp(transform.position.z, transform.position.z, transform.position.z));
    }
}

