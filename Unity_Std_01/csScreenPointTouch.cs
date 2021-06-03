using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csScreenPointTouch : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼 눌렀다 떼는 순간 호출
        if (Input.GetButtonUp("Fire1"))
        {
            // (카메라 -> 마우스 클릭 위치) 광선 정보
            Ray ray = 
                Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; // 광선 발사후 닿은 오브젝트정보
            // 실제 광선 발사
            // true면 물체에 닿았다
            // false면 물체에 닿지 않았다
            if(Physics.Raycast(ray, out hit))
            {
                // Equals, ==
                if (hit.transform.tag.Equals("ENEMY"))
                {
                    csEnemyRotate csRot =
                        //hit.transform.gameObject.GetComponent<csEnemyRotate>();
                        hit.transform.GetComponent<csEnemyRotate>();
                    if (csRot != null)
                        csRot.RotateByHit();
                }
            }
        }
    }
}
