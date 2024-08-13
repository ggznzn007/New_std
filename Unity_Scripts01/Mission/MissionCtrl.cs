using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionCtrl : MonoBehaviour
{
    public Slider guage;
    public CircleCollider2D[] colls;
    public GameObject text_anim;//, mainView;

    int missionCount;

    private void Awake()
    {
        MissionPlayerSpawn();
    }

    public void MissionPlayerSpawn()
    {
        GameObject player4 = Instantiate(Resources.Load("Character"), new Vector3(-3, 0, 0), Quaternion.identity) as GameObject;
        player4.transform.localScale = new Vector3(1.2f, 1.2f, 1);        
        player4.GetComponent<PlayerCtrl>().isMission = true;
    }

    // �̼� �ʱ�ȭ
    public void MissionReset()
    {
        guage.value = 0;
        missionCount = 0;

        for (int i = 0; i < colls.Length; i++)
        {
            colls[i].enabled = true;
        }

        text_anim.SetActive(false);
    }

    // �̼� ���� �� ȣ��
    public void MissionSuccess(CircleCollider2D coll)
    {
        missionCount++;

        guage.value = missionCount / 7f;

        // ������ �̼��� ���÷��� �Ұ�
        coll.enabled = false;

        // �������� üũ
        if(guage.value ==1)
        {
            text_anim.SetActive(true);

            Invoke("Change", 3f);
        }
    }

    // ȭ�� ��ȯ
    public void Change()
    {
        LoadingUIController.Instance.LoadScene("Main");
        //LoadingSceneCtrl.LoadScene("Main");
        /* mainView.SetActive(true);
         gameObject.SetActive(false);*/

        //ĳ���� ����
        FindObjectOfType<PlayerCtrl>().DestoyPlayer();
    }
}

