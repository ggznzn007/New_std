using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Obsolete]
public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject textChatPrefab; // 대화를 출력하는 텍스트 
    [SerializeField] private Transform parentContent; // 대화가 출력되는 스크롤뷰 컨텐츠
    [SerializeField] private TMP_InputField inputField; // 대화 입력창

    private string ID = "Chemy Cast";
    TouchScreenKeyboard keyboard;
    string chattingInputText;
      
    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    private void FixedUpdate()
    {
        if (inputField.enabled)
        {            
            // 대화 입력창이 포커스 되지 않았을 때 엔터를 누르면 
            if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
            {
                inputField.ActivateInputField();// 대화입력창 포커스 활성화
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
    {   //엔터를 누르면 입력창에 입력된 내용 대화창에 출력
        if (Input.GetKeyDown(KeyCode.Return) || (keyboard.done))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return; // 비어있으면 종료

        GameObject clone = Instantiate(textChatPrefab, parentContent);// 대화내용 출력

        clone.GetComponent<TextMeshProUGUI>().text = $"{ID} : {inputField.text}";

        Destroy(clone, 20f);

        inputField.text = ""; // 초기화  
    }
}
