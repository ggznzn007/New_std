using System.Collections;
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
        StopCoroutine(playerState.ToString());
        playerState = newState;
        StartCoroutine(playerState.ToString());
    }

    private IEnumerator Idle()
    {
        // 상태로 진입 시 1회 호출
        Debug.Log("비전투 모드로 변경");
        Debug.Log("체력/마력 초당 10씩 회복");

        // 매 프레임 호출
        while(true)
        {
            Debug.Log("플레이어 대기중");
            yield return null;
        }
    }

    private IEnumerator Walk()
    {
        // 상태로 진입 시 1회 호출
        Debug.Log("이동속도 2로 설정");        

        // 매 프레임 호출
        while (true)
        {
            Debug.Log("플레이어 걷는중");
            yield return null;
        }
    }

    private IEnumerator Run()
    {
        // 상태로 진입 시 1회 호출
        Debug.Log("이동속도 8로 설정");

        // 매 프레임 호출
        while (true)
        {
            Debug.Log("플레이어 뛰는중");
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        // 상태로 진입 시 1회 호출
        Debug.Log("전투모드로 변경");
        Debug.Log("자동회복 중지");

        // 매 프레임 호출
        while (true)
        {
            Debug.Log("플레이어 공격중");
            yield return null;
        }
    }
}
