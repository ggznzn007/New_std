using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBetAmount;    // 배팅 금액 필드
    [SerializeField] private Image imageBetAmount;             // 배팅 금액 필드(색상변경용)
    [SerializeField] private TextMeshProUGUI textCredits;      // 플레이어 소지 금액
    [SerializeField] private TextMeshProUGUI textFirstReel;    // 첫번째 릴 숫자
    [SerializeField] private TextMeshProUGUI textSecondReel;   // 두번째 릴 숫자
    [SerializeField] private TextMeshProUGUI textThirdReel;    // 세번째 릴 숫자
    [SerializeField] private TextMeshProUGUI textResult;       // 실행 결과 출력   

    private float spinDuration = 0.7f;                         // 릴 굴리기 지속시간
    private float elapsedTime = 0.2f;                             // 숫자 선택 지연시간(릴이 실제돌아가는것처럼)
    private bool isStartSpin = false;                          // 이 값이 true이면 릴 굴리기 시작
    private int credits = 10000;                               // 플레이어 소지 금액

    // 릴의 상태 (이 값이 false이면 릴을 굴리는 중)
    private bool isFirstReelSpinned = false;
    private bool isSecondReelSpinned = false;
    private bool isThirdReelSpinned = false;

    // 릴의 결과 값
    private int firstReelResult = 0;
    private int secondReelResult = 0;
    private int thirdReelResult = 0;

    private List<int> weightReelPoll;                          // 릴이 등장하는 숫자의 확률 조작 리스트
    private int zeroProbability = 30;                          // 0이 나올 확률 30==30%

    private void Awake()
    {
        textCredits.text = $"보유금액 : {credits + 0:#,###0}원";
        weightReelPoll = new List<int>(100);
        // zeroProbability 개수인 30개만큼 0으로 채워줌
        for (int i = 0; i < zeroProbability; ++i)
        {
            weightReelPoll.Add(0);
        }

        // 0이 나올 확률이 30%이기 때문에 1~9가 나올 확률이 70%
        // 7.7777 = (100 - 30) / 9;
        int remaining_values_prob = (100 - zeroProbability) / 9;

        // 1~9까지 숫자를 7개씩 weightReelPoll 리스트에 채워넣는다.
        for (int i = 1; i < 10; ++i)
        {
            for (int j = 0; j < remaining_values_prob; ++j)
            {
                weightReelPoll.Add(i);
            }
        }

        // 해당 코드는 들어간 값 확인용으로 삭제.. 0이 30개, 1~9가 7개씩 총 93개의 숫자 저장
        Debug.Log($"weightReelPoll 개수 : {weightReelPoll.Count}");
        for (int i = 0; i < weightReelPoll.Count; i++)
        {
            Debug.Log(weightReelPoll[i]);
        }
    }    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif    
        }
        if (!isStartSpin) return;
        elapsedTime += Time.deltaTime;
        int random_spinResult = Random.Range(0, 10);

        if (!isFirstReelSpinned)
        {
            firstReelResult = random_spinResult;
            if (elapsedTime >= spinDuration)
            {
                int weighted_random = Random.Range(0, weightReelPoll.Count);
                firstReelResult = (int)weightReelPoll[weighted_random];
                isFirstReelSpinned = true;
                elapsedTime = 0;
            }
        }
        else if (!isSecondReelSpinned)
        {
            secondReelResult = random_spinResult;
            if (elapsedTime >= spinDuration)
            {
                isSecondReelSpinned = true;
                elapsedTime = 0;
            }
        }
        else if (!isThirdReelSpinned)
        {
            thirdReelResult = random_spinResult;
            if (elapsedTime >= spinDuration)
            {
                int weighted_random = Random.Range(0, weightReelPoll.Count);
                thirdReelResult = (int)weightReelPoll[weighted_random];

                if ((firstReelResult == secondReelResult) && (thirdReelResult != firstReelResult))
                {
                    if (thirdReelResult < firstReelResult) thirdReelResult = firstReelResult - 1;
                    if (thirdReelResult > firstReelResult) thirdReelResult = firstReelResult + 1;
                }

                isStartSpin = false;
                elapsedTime = 0;
                isFirstReelSpinned = false;
                isSecondReelSpinned = false;
                isThirdReelSpinned = false;

                CheckBet();
            }
        }

        textFirstReel.text = firstReelResult.ToString("D1");
        textSecondReel.text = secondReelResult.ToString("D1");
        textThirdReel.text = thirdReelResult.ToString("D1");      

        if (credits <= 0&&!isStartSpin)
        {
            StartCoroutine(DelayExit());
        }

    }

    public void OnClickPull()
    {
        AudioManager.AM.PlaySE("Button");
        // 필드의 색상과 입력 정보가 바뀌어 있을 수 있으니 초기화
        OnMessage(Color.white, string.Empty);

        // 필드에 값을 입력하지 않았을 때 에러
        if (inputBetAmount.text.Trim().Equals(""))
        {
            // OnMessage(Color.red, "Please Fill Bet Amount");
            OnMessage(Color.red, "베팅금액을 적어주세요.");
            return;
        }

        int parse = int.Parse(inputBetAmount.text);

        if (credits - parse >= 0)
        {
            credits -= parse;
            // textCredits.text = $"Credits : {credits}";
            textCredits.text = $"보유금액 : {credits+0:#,###0}원";          
            
            isStartSpin = true;
        }
        else
        {
            // OnMessage(Color.red, "You don't have enough money.");
            OnMessage(Color.red, "돈이 없네요...");
        }
    }

    private void CheckBet()
    {
        int betAmount = int.Parse(inputBetAmount.text);

        if (firstReelResult == secondReelResult && secondReelResult == thirdReelResult)
        {
            credits += betAmount * 100;
            //credits += int.Parse(inputBetAmount.text) * 100;
            //textCredits.text  = $"Credits : {credits}";
            AudioManager.AM.PlaySE("Great");
            textResult.text = $"와우! 대박 잭팟!!!\n\n {betAmount * 100 + 0:#,###0}원 당첨";
        }
        else if (firstReelResult == 0 && thirdReelResult == 0)
        {
            credits += (int)(betAmount * 1.5f);
            AudioManager.AM.PlaySE("Success");
            //textResult.text = $"There are Two 0! You Win! {betAmount * 0.5f}";
            textResult.text = $"0 두개 일치 성공!\n\n {betAmount * 1.5f + 0:#,###0}원 당첨";
        }
        else if (firstReelResult == secondReelResult)
        {
            AudioManager.AM.PlaySE("Fail");
            //textResult.text = "Oh...My..JackPo..t....";
            textResult.text = "오 마이...갓..아오..";
        }
       /* else if (secondReelResult == thirdReelResult)
        {
            AudioManager.AM.PlaySE("Fail");
            // textResult.text = "Oh...My..JackPo..t....";
            textResult.text = "오 마이...갓..아오..";
        }*/
        else if (firstReelResult == thirdReelResult)
        {
            credits += betAmount * 10;
            AudioManager.AM.PlaySE("Success");
            // textResult.text = $"There are Two same number! You Win! {betAmount * 2}";
            textResult.text = $"숫자 두개 일치 성공!\n\n {betAmount * 10 + 0:#,###0}원 당첨";
        }
        else
        {
            AudioManager.AM.PlaySE("Fail");
            // textResult.text = "YOU LOSE!";
            textResult.text = "실패...";
        }

        //textCredits.text = $"Credits : {credits}";
        textCredits.text = $"보유금액 : {credits + 0:#,###0}원";
    }

    private void OnMessage(Color color, string msg)
    {
        imageBetAmount.color = color;
        textResult.text = msg;
    }


    private IEnumerator DelayExit()
    {       
        textResult.text = "3초뒤에 게임 자동종료됩니다.";
        yield return new WaitForSeconds(2);
        textResult.text = "3";
        yield return new WaitForSeconds(1.2f);
        textResult.text = "2";
        yield return new WaitForSeconds(1.2f);
        textResult.text = "1";
        yield return new WaitForSeconds(1.2f);
        textResult.text = "안녕~~~ㅎㅎㅎ";
        yield return new WaitForSeconds(1.2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif    
    }
}
