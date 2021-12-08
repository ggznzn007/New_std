using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

// 옵저버 구현클래스
public class ConcreteObserver2 : Observer
{
    // 대상타입의 클래스에서 이 메소드를 실행시킴
    public override void OnNotify()
    {
        Console.WriteLine("옵저버 클래스의 메서드 실행 #2");
        //Debug.Log("옵저버 클래스의 메서드 실행 #2");
    }
}