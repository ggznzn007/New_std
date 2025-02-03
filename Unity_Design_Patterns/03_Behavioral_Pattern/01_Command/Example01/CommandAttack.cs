using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Concrete Command ��ü : ���������� �����ϴ� ��ü
public class CommandAttack : CommandKey
{
    public CommandAttack(MonoBehaviour _mono, GameObject _shield,
                          GameObject _cannon, Transform _firePos)
    {
        this.shield = _shield;
        this.cannon = _cannon;
        this.firePos = _firePos;
        this.mono = _mono;
    }

    public override void Execute() { Attack(); }

    void Attack()
    {
        Debug.Log("Attack");
        GameObject.Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
