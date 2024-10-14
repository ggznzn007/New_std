using System.Collections;
using UnityEngine;

public class HiddenHand : MonoBehaviour                           // 손으로 터치시 
{
    public ParticleSystem[] paint;                                // 페인트 파티클
    public GameObject[] splats;                                   // 페인트 효과오브젝트
    
    public string[] colorName =                                   // 손으로 터치시 해당컬러를 인식하기 위한 문자열
    {
        "Black","Blue","Green","LightGreen","Orange","Pink","Red","Sky","White","Yellow"
    };

   /* public string Black = "Black";
    public string Blue = "Blue";
    public string Red = "Red";
    public string Pink = "Pink";
    public string Green = "Green";
    public string LightGreen = "LightGreen";
    public string White = "White";
    public string Orange = "Orange";
    public string Yellow = "Yellow";
    public string Sky = "Sky";*/

    private void OnCollisionEnter(Collision collision)
    {       
        for (int i = 0; i < colorName.Length; i++)           // 컬러에 따라 
        {
            if (collision.collider.CompareTag(colorName[i])) // 컬러이름과 비교해서 해당컬러 인식
            {                
                Destroy(collision.gameObject);             // 풍선 파괴
                AudioManager.AM.PlaySE("Pop");
                Instantiate(splats[i], new Vector3(collision.gameObject.transform.position.x, 0,collision.gameObject.transform.position.z)
                    , splats[i].transform.rotation);
                // 풍선 터지는 효과 생성
                Manager_Ballon.MB.ballCount--;             // 풍선 총 개수 빼기
                Manager_Ballon.MB.sortIndex++;             // 풍선 정렬순서 앞으로
                
                if (colorName[i]==Manager_Ballon.MB.targetName)
                {
                    Manager_Ballon.MB.explodedBallon++;
                }

                else
                {
                    Manager_Ballon.MB.explodedBallon--;
                }
            }              
        }       
    }   
}
