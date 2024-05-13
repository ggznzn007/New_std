using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform target;          // ���� �̵��ϴ� ����� Ʈ������
    [SerializeField] private Transform[] wayPoints;     // �̵� ������ ����
    [SerializeField] private float waitTime;            // ��������Ʈ ���� �� ���ð�
    [SerializeField] private float unitPerSecond = 1;   // 1�ʿ� �����̴� �Ÿ�
    [SerializeField] private bool isPlayOnAwake = true; // �ڵ� ���� ����
    [SerializeField] private bool isLoop = true;        // ��������ο��� ���۰�η� �̾����� ����
    [SerializeField] private GameObject btn;

    private int wayPointCount;                          // �̵������� ��������Ʈ ����
    private int currentIndex;                           // ���� ��������Ʈ �ε���    


    private void Awake()
    {
        wayPointCount = wayPoints.Length;
        currentIndex = Random.Range(0, wayPointCount);

        if (target == null) target = transform;
        if (isPlayOnAwake) Play();
    }

    public void Play()
    {
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        isPlayOnAwake = true;
        var wait = new WaitForSeconds(waitTime);

        while (true)
        {
            // wayPoints[currentIndex].position ��ġ���� �̵�
            yield return StartCoroutine(MoveAToB(target.position, wayPoints[currentIndex].position));

            // ���� �̵����� ����
            if (currentIndex < wayPointCount - 1)
            {
                currentIndex= Random.Range(0, wayPointCount);
            }
            else
            {
                if (isLoop) currentIndex = 0;
                else break;
            }

            // waiteTime �ð� ���� ���
            yield return wait;
        }

        Debug.Log("��� ��� Ž�� �Ϸ�!!!");
    }

    private IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0;
        // �Ÿ��� ������� 3�� ���� �̵�
        //float moveTime = 3;
        // �̵� �ð� = �� �̵� �Ÿ� / �ʴ� �̵� �Ÿ�
        float moveTime = Vector3.Distance(start, end) / unitPerSecond;

        Debug.Log($"�����ε��� : {currentIndex}��\n�̵��Ÿ� : {Vector3.Distance(start, end)}\n�̵��ð� : {moveTime}");

        while (percent < 1)
        {
            percent += Time.deltaTime / moveTime;

            target.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

    private void Update()
    {
        if (isPlayOnAwake) btn.SetActive(false);
        else btn.SetActive(true);
    }

}
