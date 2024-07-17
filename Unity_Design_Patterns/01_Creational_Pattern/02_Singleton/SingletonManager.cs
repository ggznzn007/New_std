using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingletonManager : MonoBehaviour // 싱글톤컴포넌트 예시
{
    /* // 싱글톤 // * instance라는 변수를 static으로 선언을 하여 다른 오브젝트 안의 스크립트에서도 instance를 불러올 수 있게 합니다 */
    private static SingletonManager instance;

    public static SingletonManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SingletonManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SingletonManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<SingletonManager>();
        if(objs.Length !=1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}

