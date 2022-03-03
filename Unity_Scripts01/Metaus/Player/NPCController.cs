using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NPCController : MonoBehaviour
{
   // public TextMeshPro npcText;
    public Canvas npcCanvas;
    public Button npcTextBox;
    public GameObject eventBox;
    public PlayerController player;
    
    //private string loadText = "이벤트가 있어요!!!";
    public void Start()
    {        
        npcCanvas.gameObject.SetActive(false);
        eventBox.gameObject.transform.position = gameObject.transform.position;
        //npcText.transform.position = gameObject.transform.position;
        npcTextBox.gameObject.transform.position = gameObject.transform.position;
    }
    public void TextBtn()
    {
        StartCoroutine(TextBoxClose());
        StartCoroutine(EventBoxOpen());
    }
    public void AcceptBtn()
    {
        SceneManager.LoadSceneAsync("RouletteScene", LoadSceneMode.Additive);        
        StartCoroutine(EventBoxClose());
    }
    public void RejectBtn()
    {
        StartCoroutine(EventBoxClose());
    }
    
     IEnumerator TextBoxOpen()
    {
        npcTextBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 1.65f), 0),0.2f);
        npcTextBox.transform.LeanScale(Vector2.one, 0.2f);
        yield return null;
    }

     IEnumerator TextBoxClose()
    {
        npcTextBox.transform.LeanMove(new Vector3(transform.position.x, transform.position.y, 0), 0.2f);
        npcTextBox.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();

        yield return null;
    }

     IEnumerator EventBoxOpen()
    {
        eventBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 3f), 0), 0.2f);
        eventBox.transform.LeanScale(Vector2.one, 0.2f);
        yield return null;
    }
     IEnumerator EventBoxClose()
    {
        eventBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 1f), 0), 0.2f);
        eventBox.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
        yield return null;
        
    }

     IEnumerator NPCCanvasOff()
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
            {
                //StartCoroutine(TypingText());
                StartCoroutine(TextBoxOpen());

            }

        }
    }

   public void OnTriggerStay2D(Collider2D collision)
     {
         if (collision.tag == "Player")
         {
            Debug.Log("NPCTriggerStay");
            npcCanvas.gameObject.SetActive(true);
            if (npcCanvas)
            {                
                StartCoroutine(TextBoxOpen());                
            }
        }
     }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerExit");
            // StartCoroutine(TextBoxClose());
            // StartCoroutine(EventBoxClose());
            StartCoroutine(NPCCanvasOff());
        }
    }
}
