using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLeafMove : LeafMove
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Finish"))
        {
            StartCoroutine(DelayPos());
        }
    }

    IEnumerator DelayPos()
    {
        yield return new WaitForSecondsRealtime(10);                   // (10) 10.5초를 원하면 (10.5f) 넣을 것 디폴트 10초 
        transform.position = startPos;                                 // 낙엽 위치값을 처음위치값으로 초기화
        transform.rotation = startRot;                                 // 낙엽 회전값을 처음회전값으로 초기화
        transform.localScale = new Vector3(0, 0, 0);                   // 낙엽 스케일(크기)를 0,0,0으로 설정
        yield return new WaitForSecondsRealtime(1);                    // 1초의 딜레이를 설정
        transform.LeanScale(new Vector3(1, 1, 1), 2).setEaseLinear();  // 낙엽 초기크기가 1.5f라서 1.5f로 설정하고 뒤에 2는 커지는 시간
                                                                       // ex) transform.LeanScale(new Vector3(1,1,1), 1.5f).setEaseLinear();
                                                                       // 위의 예시는 크기가 1 커지는 시간이 1.5초 
        transform.position = startPos;                                 // 다시 낙엽 위치값 초기화
        transform.rotation = startRot;                                 // 다시 낙엽 회전값 초기화
    }
}
