using UnityEngine; // if-then으로 구현한 FSM의 예시에서 코루티을 활용한 FSM으로

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
                Debug.Log("플레이어 대기중");
                break;
            case PlayerState.Walk:
                Debug.Log("플레이어 걷는중");
                break;
            case PlayerState.Run:
                Debug.Log("플레이어 뛰는중");
                break;
            case PlayerState.Attack:
                Debug.Log("플레이어 공격중");
                break;
        }

    }

    private void ChangeState(PlayerState newState)
    {
        playerState = newState;
    }
}
