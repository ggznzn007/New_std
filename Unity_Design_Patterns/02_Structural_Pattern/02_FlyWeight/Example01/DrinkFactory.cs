using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkFactory : MonoBehaviour
{
    static Dictionary<string, Cola> dic = new Dictionary<string, Cola>(); // �̹� �����س��� �帵ũ ��ü�� �ִ��� �����ϱ�����

    public static Cola getDrink(string name) // �ش� �̸��� ������ ���� ��ü�� �������� ����
    {
        if(!dic.ContainsKey(name))
        {
            Cola tmp = new Cola(name);
            dic.Add(name, tmp); // ��ųʸ��� �߰�
        }

        return dic[name]; // ������ �� ����
    }
}
