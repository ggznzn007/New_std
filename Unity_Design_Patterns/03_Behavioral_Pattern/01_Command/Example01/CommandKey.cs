using UnityEngine;

// �������̽� : Execute() �޼ҵ常 �ִ� �߻�Ŭ����
public abstract class CommandKey
{
	public GameObject shield;
	public GameObject cannon;
	public Transform firePos;

	public MonoBehaviour mono;

	public virtual void Execute() { }
}
