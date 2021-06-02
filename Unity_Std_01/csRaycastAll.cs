using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRaycastAll : MonoBehaviour
{
    public float speed = 20f;
    public float rayLength = 16f;
    void Update()
    {
        // 좌우 이동
        float h = Input.GetAxis("Horizontal");
        h = h * speed * Time.deltaTime;
        transform.Translate(Vector3.right * h);

        // Debug Draw Ray
        Debug.DrawRay(transform.position,
                    transform.forward * rayLength,
                    Color.red);
        // Ray를 발사해서 모든 닿는 물체를 감지
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position,
                                transform.forward, rayLength);
        for(int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
