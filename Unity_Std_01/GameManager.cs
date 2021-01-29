using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText; // 게임오버 시 활성화할 텍스트 게임 오브젝트
    public Text timeText; // 생존 시간을 표시할 텍스트 컴포넌트
    public Text recordText; // 최고 기록을 표시할 텍스트 컴포넌트

    private float surviveTime; // 생존 시간
    private bool isGameover; // 게임오버 상태
    //private bool isPause;
    // Start is called before the first frame update
    void Start()
    {
        // 생존 시간과 게임오버 상태 초기화
        surviveTime = 0;
        isGameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 게임오버가 아닌 동안
        if (!isGameover)
        {
            // 생존시간 갱신
            surviveTime += Time.deltaTime;
            // 갱신한 생존 시간을 텍스트를 이용해 표시
            timeText.text = "Time: " + (int)surviveTime;
        }
        
        else
        {
            // 게임오버 상태에서 스페이스바를 누른 경우
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 재시작
                SceneManager.LoadScene("Dodge");
            }
            
        }
    }
    // 현재 게임을 게임오버 상태로 변경하는 메서드
    public void EndGame()
    {
        // 현재 상태를 게임오버 상태로 전환
        isGameover = true;
        // 게임오버 텍스트를 활성화
        gameoverText.SetActive(true);

        // 최고기록 가져오기
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        // 이전까지의 최고 기록보다 현재 생존 시간이 더 크면
        if (surviveTime > bestTime)
        {
            // 최고기록값을 현재 생존 시간 값으로 변경
            bestTime = surviveTime;
            // 변경된 최고기록을 최고기록으로 저장
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // 최고기록을 텍스트를 이용해 표시
        recordText.text = "Best Time: " + (int)bestTime;


    }


}
