using System.Collections;
using UnityEngine; // if-then���� ������ FSM�� ���ÿ��� �ڷ�Ƽ�� Ȱ���� FSM����

public enum PlayerState { Idle = 0, Walk, Run, Attack }

public class Player_Controller : MonoBehaviour
{
    private PlayerState playerState;

    private void Awake()
    {
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        if(Input.GetKeyDown("1")) {ChangeState(PlayerState.Idle);}
        else if(Input.GetKeyDown("2")) {ChangeState(PlayerState.Walk);}
        else if(Input.GetKeyDown("3")) {ChangeState(PlayerState.Run);}
        else if(Input.GetKeyDown("4")) {ChangeState(PlayerState.Attack);}
        
    }

    private void UpdateState()
    {
        switch (playerState)
        {            
            case PlayerState.Idle:
                Debug.Log("�÷��̾� �����");
                break;
            case PlayerState.Walk:
                Debug.Log("�÷��̾� �ȴ���");
                break;
            case PlayerState.Run:
                Debug.Log("�÷��̾� �ٴ���");
                break;
            case PlayerState.Attack:
                Debug.Log("�÷��̾� ������");
                break;
        }

    }

    private void ChangeState(PlayerState newState)
    {
        StopCoroutine(playerState.ToString());
        playerState = newState;
        StartCoroutine(playerState.ToString());
    }

    private IEnumerator Idle()
    {
        // ���·� ���� �� 1ȸ ȣ��
        Debug.Log("������ ���� ����");
        Debug.Log("ü��/���� �ʴ� 10�� ȸ��");

        // �� ������ ȣ��
        while(true)
        {
            Debug.Log("�÷��̾� �����");
            yield return null;
        }
    }

    private IEnumerator Walk()
    {
        // ���·� ���� �� 1ȸ ȣ��
        Debug.Log("�̵��ӵ� 2�� ����");        

        // �� ������ ȣ��
        while (true)
        {
            Debug.Log("�÷��̾� �ȴ���");
            yield return null;
        }
    }

    private IEnumerator Run()
    {
        // ���·� ���� �� 1ȸ ȣ��
        Debug.Log("�̵��ӵ� 8�� ����");

        // �� ������ ȣ��
        while (true)
        {
            Debug.Log("�÷��̾� �ٴ���");
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        // ���·� ���� �� 1ȸ ȣ��
        Debug.Log("�������� ����");
        Debug.Log("�ڵ�ȸ�� ����");

        // �� ������ ȣ��
        while (true)
        {
            Debug.Log("�÷��̾� ������");
            yield return null;
        }
    }
}
