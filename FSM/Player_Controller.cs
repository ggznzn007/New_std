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
        UpdateState();
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
        playerState = newState;
    }
}
