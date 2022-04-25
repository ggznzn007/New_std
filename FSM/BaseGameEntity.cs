using UnityEngine; // 하나의 캐릭터를 위한 FSM

public abstract class BaseGameEntity : MonoBehaviour
{
    // 정적 변수이기 때문에 1개만 존재
    private static int m_iNextValidID = 0;

    // 본 가상클래스를 상속받는 모든 오브젝트는 ID번호를 부여받는데
    // 0부터 시작해서 1씩 증가 ( 현실의 주민등록번호 처럼 사용)
    private int id;
    public int ID
    {
        set
        {
            id = value;
            m_iNextValidID++;
        }
        get => id;
    }

    private string entityName; // 캐릭터 이름
    private string personalColor; // 캐릭터색상(출력용)

    // 파생클래스에서 base.Setup()으로 호출
    public virtual void Setup(string name)
    {
        // 고유번호 설정
        ID = m_iNextValidID;
        // 이름 설정
        entityName = name;
        // 색상 설정
        int color = Random.Range(0, 1000000);
        personalColor = $"#{color.ToString("X6")}";
    }

    // 게임컨트롤러 클래스에서 모든 캐릭터의 Updated()를 호출해 캐릭터 구동
    public abstract void Updated();

    // 콘솔에 이름과 대사 출력
    public void PrintText(string text)
    {
        Debug.Log($"<color={personalColor}><b>{entityName}</b></color> : {text}");
    }
}
