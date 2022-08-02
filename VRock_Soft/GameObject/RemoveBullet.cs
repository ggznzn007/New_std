using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject exploreEffet;

    private void OnCollisionEnter(Collision collision)
    {
        // 터지는 이펙트 보여지고
        if (collision.collider.CompareTag("Bullet"))
        {
            //var exEffect = ObjectPooler.SpawnFromPool<ExploreEffect>("ExplorelEffect");
            ShowEffect(collision);     
           
            //Destroy(collision.gameObject); // 총알 맞은 오브젝트가 사라짐 
            Debug.Log("총알태그됨");
        }
    }
    private void ShowEffect(Collision coll)
    {
        // 충돌지점의 정보를 추출
        ContactPoint contact = coll.contacts[0];

        // 법선 벡타가 이루는 회전각도 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // 폭발 효과 생성
        Instantiate(exploreEffet, contact.point, rot);


    }
}
