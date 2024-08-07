using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeRoom : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        //OfficeRoomPlayerSpawn();
    }

    public void OfficeRoomPlayerSpawn()
    {
        GameObject player1 = Instantiate(Resources.Load("InPlayer"), new Vector3(0f, 40.5f, 0), Quaternion.identity) as GameObject;
        player1.transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }

    public void ClickBack()
    {
        //LoadingSceneCtrl.LoadScene("Main");
        LoadingUIController.Instance.LoadScene("Main");
    }

    // ä��â Ȱ��ȭ
    public void ClickChat()
    {
        // chatting.SetActive(true);
    }

    // ä���� ��Ȱ��ȭ
    public void ClickQuit()
    {
        // chatting.SetActive(false);
    }
}
