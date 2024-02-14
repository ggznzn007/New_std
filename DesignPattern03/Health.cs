using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 관찰 대상 관리

public class Health
{
    float health = 0;

    private List<IHealthObserver> observers = new List<IHealthObserver>();

    // 관찰자를 등록하는 메서드
    public void RegisterObserver(IHealthObserver observer)
    {
        observers.Add(observer);
    }

    // 관찰자를 해제하는 메서드
    public void RemoveObserver(IHealthObserver observer)
    {
        observers.Remove(observer);
    }

    // 관찰자에게 체력 변경을 알리는 메서드
    public void NotifyObservser()
    {
        foreach (var observer in observers)
        {
            observer.OnHealthChanged(health);
        }
    }

    // 관리할 메서드
    public void ModifyHealth(float currentHP, float maxHP)
    {
        health = currentHP / maxHP;

        NotifyObservser();
    }
}
