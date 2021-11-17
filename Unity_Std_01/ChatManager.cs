using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public List<string> chatList = new List<string>();
    public Button sendBtn;
    public Text chatLog;
    public Text chattingList;
    public InputField input;
    ScrollRect scroll_rect = null;
    string chatters;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
    }
    public void SendButtonOnClicked()
    {
        if (input.text.Equals("")) { Debug.Log("Empty"); return; }
        string msg = string.Format("[ {0} ] {1}", PhotonNetwork.LocalPlayer.NickName, input.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.ActivateInputField(); // 반대는 input.select(); (반대로 토글)
        input.text = "";
    }
    void Update()
    {
        chatterUpdate();
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused)
        { 
            SendButtonOnClicked();
        }
    }
    void chatterUpdate()
    {
        chatters = "Participant List\n";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            chatters += p.NickName + "\n";
        }
        chattingList.text = chatters;
        chattingList.color = Color.blue;
    }
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        chatLog.color = Color.white;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
