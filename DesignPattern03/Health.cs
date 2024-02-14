using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��� ����

public class Health
{
    float health = 0;

    private List<IHealthObserver> observers = new List<IHealthObserver>();

    // �����ڸ� ����ϴ� �޼���
    public void RegisterObserver(IHealthObserver observer)
    {
        observers.Add(observer);
    }

    // �����ڸ� �����ϴ� �޼���
    public void RemoveObserver(IHealthObserver observer)
    {
        observers.Remove(observer);
    }

    // �����ڿ��� ü�� ������ �˸��� �޼���
    public void NotifyObservser()
    {
        foreach (var observer in observers)
        {
            observer.OnHealthChanged(health);
        }
    }

    // ������ �޼���
    public void ModifyHealth(float currentHP, float maxHP)
    {
        health = currentHP / maxHP;

        NotifyObservser();
    }
}
