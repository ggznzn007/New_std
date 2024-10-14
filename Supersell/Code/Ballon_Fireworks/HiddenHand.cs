using System.Collections;
using UnityEngine;

public class HiddenHand : MonoBehaviour                           // ������ ��ġ�� 
{
    public ParticleSystem[] paint;                                // ����Ʈ ��ƼŬ
    public GameObject[] splats;                                   // ����Ʈ ȿ��������Ʈ
    
    public string[] colorName =                                   // ������ ��ġ�� �ش��÷��� �ν��ϱ� ���� ���ڿ�
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
        for (int i = 0; i < colorName.Length; i++)           // �÷��� ���� 
        {
            if (collision.collider.CompareTag(colorName[i])) // �÷��̸��� ���ؼ� �ش��÷� �ν�
            {                
                Destroy(collision.gameObject);             // ǳ�� �ı�
                AudioManager.AM.PlaySE("Pop");
                Instantiate(splats[i], new Vector3(collision.gameObject.transform.position.x, 0,collision.gameObject.transform.position.z)
                    , splats[i].transform.rotation);
                // ǳ�� ������ ȿ�� ����
                Manager_Ballon.MB.ballCount--;             // ǳ�� �� ���� ����
                Manager_Ballon.MB.sortIndex++;             // ǳ�� ���ļ��� ������
                
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
