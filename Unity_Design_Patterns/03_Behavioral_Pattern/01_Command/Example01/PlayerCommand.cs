using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommand : MonoBehaviour
{
	public GameObject shield;
	public GameObject cannon;
	public Transform firePos;
	bool bCmd;
	Text txt1;
	Text txt2;

	CommandKey btnA, btnB;

	void Start()
	{
		bCmd = true;
		txt1 = GameObject.Find("Text1").GetComponent<Text>();
		txt2 = GameObject.Find("Text2").GetComponent<Text>();

		SetCommand();
	}

	// SetCommand() 메소드를 통해 버튼을 누르면 어떤 동작을 수행할지를 각 버튼에 등록
	public void SetCommand()
	{
		if (bCmd == true)
		{
			btnA = new CommandAttack(this, shield, cannon, firePos);
			btnB = new CommandDefense(this, shield, cannon, firePos);

			bCmd = false;
			txt1.text = "A - Attack";
			txt2.text = "D - Defense";
		}
		else
		{
			btnA = new CommandDefense(this, shield, cannon, firePos);
			btnB = new CommandAttack(this, shield, cannon, firePos);

			bCmd = true;
			txt1.text = "A - Defense";
			txt2.text = "D - Attack";
		}
	}

	// 버튼을 누르면 단지 버튼의 Execute()만 호출
	void Update()
	{

		if (Input.GetKeyDown("a"))
		{
			btnA.Execute();
		}
		else if (Input.GetKeyDown("d"))
		{
			btnB.Execute();
		}
	}
}
