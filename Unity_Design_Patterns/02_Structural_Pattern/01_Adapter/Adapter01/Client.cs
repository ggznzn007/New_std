using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    void Start()
    {

        MallardDuck duck = new MallardDuck();
        WildTurkey turkey = new WildTurkey();
        Duck turkeyAdapter = new TurkeyAdapter(turkey);

        Debug.Log("���� ���...");
        testDuck(duck);

        Debug.Log("���� ����. ĥ������ ��ü...");
        testDuck(turkeyAdapter);
    }

    void testDuck(Duck duck)
    {
        // ������ ������� ���
        duck.quack();
        duck.fly();
    }
}
