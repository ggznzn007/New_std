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

        Debug.Log("오리 사용...");
        testDuck(duck);

        Debug.Log("오리 부족. 칠면조로 대체...");
        testDuck(turkeyAdapter);
    }

    void testDuck(Duck duck)
    {
        // 동일한 방법으로 사용
        duck.quack();
        duck.fly();
    }
}
