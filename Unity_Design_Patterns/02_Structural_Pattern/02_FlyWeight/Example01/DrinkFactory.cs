using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkFactory : MonoBehaviour
{
    static Dictionary<string, Cola> dic = new Dictionary<string, Cola>(); // 이미 생성해놓은 드링크 객체가 있는지 관리하기위함

    public static Cola getDrink(string name) // 해당 이름의 생성해 놓은 객체가 없을때만 생성
    {
        if(!dic.ContainsKey(name))
        {
            Cola tmp = new Cola(name);
            dic.Add(name, tmp); // 딕셔너리에 추가
        }

        return dic[name]; // 생성한 것 리턴
    }
}
