using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}

public class GoogleSheetManager : MonoBehaviour
{
	const string URL = "https://script.google.com/macros/s/AKfycbwaZo2PwYsu5sRyReah66ZyvBihzdu6xxlrhu9fOAXEcVQjQpbxIaWv4Z3R5Vj8w5CEPQ/exec";
	public GoogleData GD;
	public InputField IDInput, PassInput, ValueInput;
	string id, pass;
	public Text orderTxt, resultTxt, msgTxt, valueTxt;
	public Button enter;

    private void Update()
    {
		orderTxt.text = GD.order;
		resultTxt.text = GD.result;
		msgTxt.text = GD.msg;
		valueTxt.text = GD.value;
		if (Input.GetKeyDown(KeyCode.Escape))
		{			
			LoadingUIController.Instance.LoadScene("Enter");
			WWWForm form = new WWWForm();
			form.AddField("order", "logout");

			StartCoroutine(Post(form));
		}
	}

    bool SetIDPass()
	{
		id = IDInput.text.Trim();
		pass = PassInput.text.Trim();

		if (id == "" || pass == "") return false;
		else return true;
	}

	public void Enter()
    {
		LoadingUIController.Instance.LoadScene("Main");
    }
		
	public void Register()
	{
		if (!SetIDPass())
		{
			print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
			
			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("order", "register");
		form.AddField("id", id);
		form.AddField("pass", pass);

		StartCoroutine(Post(form));
	}


	public void Login()
	{
		if (!SetIDPass())
		{
			print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("order", "login");
		form.AddField("id", id);
		form.AddField("pass", pass);		
		enter.gameObject.SetActive(true);
		StartCoroutine(Post(form));
	}


	void OnApplicationQuit()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "logout");

		StartCoroutine(Post(form));
	}


	public void SetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "setValue");
		form.AddField("value", ValueInput.text);

		StartCoroutine(Post(form));
	}


	public void GetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "getValue");

		StartCoroutine(Post(form));
	}





	IEnumerator Post(WWWForm form)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("���� ������ �����ϴ�.");
		}
	}


	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;

		GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.result == "ERROR")
		{
			print(GD.order + "�� ������ �� �����ϴ�. ���� �޽��� : " + GD.msg);
			return;
		}

		print(GD.order + "�� �����߽��ϴ�. �޽��� : " + GD.msg);

		if (GD.order == "getValue")
		{
			ValueInput.text = GD.value;
		}
	}
}
