using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Obsolete]
public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject textChatPrefab; // ��ȭ�� ����ϴ� �ؽ�Ʈ 
    [SerializeField] private Transform parentContent; // ��ȭ�� ��µǴ� ��ũ�Ѻ� ������
    [SerializeField] private TMP_InputField inputField; // ��ȭ �Է�â


    private string ID = "Chemy Cast";
    TouchScreenKeyboard keyboard;
    string chattingInputText;
    private void Start()
    {


    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    private void FixedUpdate()
    {
        if (inputField.enabled)
        {            
            // ��ȭ �Է�â�� ��Ŀ�� ���� �ʾ��� �� ���͸� ������ 
            if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
            {
                inputField.ActivateInputField();// ��ȭ�Է�â ��Ŀ�� Ȱ��ȭ
                OpenKeyboard();
                if (TouchScreenKeyboard.visible == false && keyboard != null)
                {                   
                    
                    chattingInputText = keyboard.text;
                    inputField.text = chattingInputText;

                    inputField.onSubmit.AddListener(delegate
                    { if (this.keyboard.text.Length > 0) OnEndEditEventMethod(); });
                }
            }

        }

    }

    public void OnEndEditEventMethod()
    {   //���͸� ������ �Է�â�� �Էµ� ���� ��ȭâ�� ���
        if (Input.GetKeyDown(KeyCode.Return) || (keyboard.done))
        {
            UpdateChat();
        }

    }

    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return; // ��������� ����

        GameObject clone = Instantiate(textChatPrefab, parentContent);// ��ȭ���� ���

        clone.GetComponent<TextMeshProUGUI>().text = $"{ID} : {inputField.text}";

        Destroy(clone, 20f);

        inputField.text = ""; // �ʱ�ȭ  


    }


}
