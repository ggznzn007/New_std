using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Concrete Command ��ü : ���������� �����ϴ� ��ü
public class CommandDefense : CommandKey
{
	public CommandDefense(MonoBehaviour _mono, GameObject _shield,
						  GameObject _cannon, Transform _firePos)
	{
		this.shield = _shield;
		this.cannon = _cannon;
		this.firePos = _firePos;
		this.mono = _mono;
	}

	public override void Execute()
	{
		Defense();
	}

	void Defense()
	{
		Debug.Log("Defense");
		shield.SetActive(true);
		mono.StartCoroutine(Defense(1f));
	}

	IEnumerator Defense(float second)
	{
		yield return new WaitForSeconds(second);
		this.shield.SetActive(false);
	}
}
