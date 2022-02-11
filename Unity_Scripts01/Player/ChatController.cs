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
   
    private string ID = "ChemyCast";
    TouchScreenKeyboard keyboard;
    private void Start()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false);
    }
    private void FixedUpdate()
    {
            
        
        // ��ȭ �Է�â�� ��Ŀ�� ���� �ʾ��� �� ���͸� ������ 
        if (Input.GetKeyDown(KeyCode.Return)&&inputField.isFocused== false)
        {
           
            inputField.ActivateInputField();// ��ȭ�Է�â ��Ŀ�� Ȱ��ȭ
            //inputField.text = keyboard.text;
            inputField.onSubmit.AddListener(delegate
            { if(this.keyboard.text.Length>0) OnEndEditEventMethod(); });
        }
    }

    public void OnEndEditEventMethod()
    {   //���͸� ������ �Է�â�� �Էµ� ���� ��ȭâ�� ���
        if(Input.GetKeyDown(KeyCode.Return) || (keyboard.status == TouchScreenKeyboard.Status.Done))
        {            
            UpdateChat();
        }

      
    }

    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return; // ��������� ����
        
        GameObject clone = Instantiate(textChatPrefab, parentContent);// ��ȭ���� ���

        clone.GetComponent<TextMeshProUGUI>().text = $"{ID} : {inputField.text}";

        Destroy(clone, 15f);

        inputField.text = ""; // �ʱ�ȭ  
    }

   
}
