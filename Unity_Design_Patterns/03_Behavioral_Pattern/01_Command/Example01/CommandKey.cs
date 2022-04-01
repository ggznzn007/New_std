using UnityEngine;

// 인터페이스 : Execute() 메소드만 있는 추상클래스
public abstract class CommandKey
{
	public GameObject shield;
	public GameObject cannon;
	public Transform firePos;

	public MonoBehaviour mono;

	public virtual void Execute() { }
}
