using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NPCController_Night : MonoBehaviour
{
    public TextMeshPro npcText;
    public Canvas npcCanvas;
    public Button npcTextBox;
    public GameObject eventBox;
    public PlayerController2 player2;
   
    //private string loadText = "이벤트가 있어요!!!";
    public void Start()
    {        
        npcCanvas.gameObject.SetActive(false);
        eventBox.gameObject.transform.position = this.gameObject.transform.position;
        npcText.transform.position = this.gameObject.transform.position;
        npcTextBox.gameObject.transform.position = this.gameObject.transform.position;
    }
    public void TextBtn2()
    {
        Debug.Log("Clicked Btn");
        StartCoroutine(TextBoxClose2());
        StartCoroutine(EventBoxOpen2());
    }
    public void AcceptBtn2()
    {
        SceneManager.LoadSceneAsync("RouletteScene_Night", LoadSceneMode.Additive);        
        StartCoroutine(EventBoxClose2());
    }
    public void RejectBtn2()
    {
        StartCoroutine(EventBoxClose2());
    }
    
    IEnumerator TextBoxOpen2()
    {
        npcTextBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 1.65f), 0), 0.2f);
        npcTextBox.transform.LeanScale(Vector2.one, 0.2f);
        yield return null;
    }

    IEnumerator TextBoxClose2()
    {
        npcTextBox.transform.LeanMove(new Vector3(transform.position.x, transform.position.y, 0), 0.2f);
        npcTextBox.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();

        yield return null;
    }

    IEnumerator EventBoxOpen2()
    {
        eventBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 3f), 0), 0.2f);
        eventBox.transform.LeanScale(Vector2.one, 0.2f);
        yield return null;
    }
    IEnumerator EventBoxClose2()
    {
        eventBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 1f), 0), 0.2f);
        eventBox.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
        yield return null;
    }

    IEnumerator NPCCanvasOff2()
    {
        yield return new WaitForSeconds(5f);
        npcCanvas.gameObject.SetActive(false);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerEnter");
            npcCanvas.gameObject.SetActive(true);
            if (npcCanvas)
            {                //StartCoroutine(TypingText());
                StartCoroutine(TextBoxOpen2());
            }

        }
    }

   /* public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerStay");
            npcCanvas.gameObject.SetActive(true);
            if (npcCanvas)
            {
                //StartCoroutine(TypingText());
                StartCoroutine(TextBoxOpen2());

            }
        }
    }*/

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerExit");
            StartCoroutine(TextBoxClose2());
            StartCoroutine(EventBoxClose2());
            StartCoroutine(NPCCanvasOff2());
        }
    }
}
