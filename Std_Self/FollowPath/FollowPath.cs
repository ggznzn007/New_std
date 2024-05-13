using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform target;          // 실제 이동하는 대상의 트랜스폼
    [SerializeField] private Transform[] wayPoints;     // 이동 가능한 지점
    [SerializeField] private float waitTime;            // 웨이포인트 도착 후 대기시간
    [SerializeField] private float unitPerSecond = 1;   // 1초에 움직이는 거리
    [SerializeField] private bool isPlayOnAwake = true; // 자동 시작 여부
    [SerializeField] private bool isLoop = true;        // 마지막경로에서 시작경로로 이어지는 여부
    [SerializeField] private GameObject btn;

    private int wayPointCount;                          // 이동가능한 웨이포인트 개수
    private int currentIndex;                           // 현재 웨이포인트 인덱스    


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
            // wayPoints[currentIndex].position 위치까지 이동
            yield return StartCoroutine(MoveAToB(target.position, wayPoints[currentIndex].position));

            // 다음 이동지점 설정
            if (currentIndex < wayPointCount - 1)
            {
                currentIndex= Random.Range(0, wayPointCount);
            }
            else
            {
                if (isLoop) currentIndex = 0;
                else break;
            }

            // waiteTime 시간 동안 대기
            yield return wait;
        }

        Debug.Log("모든 경로 탐색 완료!!!");
    }

    private IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0;
        // 거리에 관계없이 3초 동안 이동
        //float moveTime = 3;
        // 이동 시간 = 총 이동 거리 / 초당 이동 거리
        float moveTime = Vector3.Distance(start, end) / unitPerSecond;

        Debug.Log($"현재인덱스 : {currentIndex}번\n이동거리 : {Vector3.Distance(start, end)}\n이동시간 : {moveTime}");

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
