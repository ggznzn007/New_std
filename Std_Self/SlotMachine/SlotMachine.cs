using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBetAmount;    // ���� �ݾ� �ʵ�
    [SerializeField] private Image imageBetAmount;             // ���� �ݾ� �ʵ�(���󺯰��)
    [SerializeField] private TextMeshProUGUI textCredits;      // �÷��̾� ���� �ݾ�
    [SerializeField] private TextMeshProUGUI textFirstReel;    // ù��° �� ����
    [SerializeField] private TextMeshProUGUI textSecondReel;   // �ι�° �� ����
    [SerializeField] private TextMeshProUGUI textThirdReel;    // ����° �� ����
    [SerializeField] private TextMeshProUGUI textResult;       // ���� ��� ���   

    private float spinDuration = 0.7f;                         // �� ������ ���ӽð�
    private float elapsedTime = 0.2f;                             // ���� ���� �����ð�(���� �������ư��°�ó��)
    private bool isStartSpin = false;                          // �� ���� true�̸� �� ������ ����
    private int credits = 10000;                               // �÷��̾� ���� �ݾ�

    // ���� ���� (�� ���� false�̸� ���� ������ ��)
    private bool isFirstReelSpinned = false;
    private bool isSecondReelSpinned = false;
    private bool isThirdReelSpinned = false;

    // ���� ��� ��
    private int firstReelResult = 0;
    private int secondReelResult = 0;
    private int thirdReelResult = 0;

    private List<int> weightReelPoll;                          // ���� �����ϴ� ������ Ȯ�� ���� ����Ʈ
    private int zeroProbability = 30;                          // 0�� ���� Ȯ�� 30==30%

    private void Awake()
    {
        textCredits.text = $"�����ݾ� : {credits + 0:#,###0}��";
        weightReelPoll = new List<int>(100);
        // zeroProbability ������ 30����ŭ 0���� ä����
        for (int i = 0; i < zeroProbability; ++i)
        {
            weightReelPoll.Add(0);
        }

        // 0�� ���� Ȯ���� 30%�̱� ������ 1~9�� ���� Ȯ���� 70%
        // 7.7777 = (100 - 30) / 9;
        int remaining_values_prob = (100 - zeroProbability) / 9;

        // 1~9���� ���ڸ� 7���� weightReelPoll ����Ʈ�� ä���ִ´�.
        for (int i = 1; i < 10; ++i)
        {
            for (int j = 0; j < remaining_values_prob; ++j)
            {
                weightReelPoll.Add(i);
            }
        }

        // �ش� �ڵ�� �� �� Ȯ�ο����� ����.. 0�� 30��, 1~9�� 7���� �� 93���� ���� ����
        Debug.Log($"weightReelPoll ���� : {weightReelPoll.Count}");
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
        Application.Quit(); // ���ø����̼� ����
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
        // �ʵ��� ����� �Է� ������ �ٲ�� ���� �� ������ �ʱ�ȭ
        OnMessage(Color.white, string.Empty);

        // �ʵ忡 ���� �Է����� �ʾ��� �� ����
        if (inputBetAmount.text.Trim().Equals(""))
        {
            // OnMessage(Color.red, "Please Fill Bet Amount");
            OnMessage(Color.red, "���ñݾ��� �����ּ���.");
            return;
        }

        int parse = int.Parse(inputBetAmount.text);

        if (credits - parse >= 0)
        {
            credits -= parse;
            // textCredits.text = $"Credits : {credits}";
            textCredits.text = $"�����ݾ� : {credits+0:#,###0}��";          
            
            isStartSpin = true;
        }
        else
        {
            // OnMessage(Color.red, "You don't have enough money.");
            OnMessage(Color.red, "���� ���׿�...");
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
            textResult.text = $"�Ϳ�! ��� ����!!!\n\n {betAmount * 100 + 0:#,###0}�� ��÷";
        }
        else if (firstReelResult == 0 && thirdReelResult == 0)
        {
            credits += (int)(betAmount * 1.5f);
            AudioManager.AM.PlaySE("Success");
            //textResult.text = $"There are Two 0! You Win! {betAmount * 0.5f}";
            textResult.text = $"0 �ΰ� ��ġ ����!\n\n {betAmount * 1.5f + 0:#,###0}�� ��÷";
        }
        else if (firstReelResult == secondReelResult)
        {
            AudioManager.AM.PlaySE("Fail");
            //textResult.text = "Oh...My..JackPo..t....";
            textResult.text = "�� ����...��..�ƿ�..";
        }
       /* else if (secondReelResult == thirdReelResult)
        {
            AudioManager.AM.PlaySE("Fail");
            // textResult.text = "Oh...My..JackPo..t....";
            textResult.text = "�� ����...��..�ƿ�..";
        }*/
        else if (firstReelResult == thirdReelResult)
        {
            credits += betAmount * 10;
            AudioManager.AM.PlaySE("Success");
            // textResult.text = $"There are Two same number! You Win! {betAmount * 2}";
            textResult.text = $"���� �ΰ� ��ġ ����!\n\n {betAmount * 10 + 0:#,###0}�� ��÷";
        }
        else
        {
            AudioManager.AM.PlaySE("Fail");
            // textResult.text = "YOU LOSE!";
            textResult.text = "����...";
        }

        //textCredits.text = $"Credits : {credits}";
        textCredits.text = $"�����ݾ� : {credits + 0:#,###0}��";
    }

    private void OnMessage(Color color, string msg)
    {
        imageBetAmount.color = color;
        textResult.text = msg;
    }


    private IEnumerator DelayExit()
    {       
        textResult.text = "3�ʵڿ� ���� �ڵ�����˴ϴ�.";
        yield return new WaitForSeconds(2);
        textResult.text = "3";
        yield return new WaitForSeconds(1.2f);
        textResult.text = "2";
        yield return new WaitForSeconds(1.2f);
        textResult.text = "1";
        yield return new WaitForSeconds(1.2f);
        textResult.text = "�ȳ�~~~������";
        yield return new WaitForSeconds(1.2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif    
    }
}
