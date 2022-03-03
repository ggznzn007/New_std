using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeNPCctrl : MonoBehaviour
{    
    public Canvas npcCanvas;
    public Sprite[] boxImg;
    //public Sprite[] textImg;
    public Image npcTextBox;
   // public Image npcText;
    public PlayerController player;
    public PlayerController2 player2;
    public void Start()
    {        
        npcCanvas.gameObject.SetActive(false);         
        npcTextBox.gameObject.transform.position = gameObject.transform.position;
       // npcText.gameObject.transform.position = gameObject.transform.position;
    }

   public void GetNPCTextBox()
    {
        npcTextBox.sprite = npcTextBox.sprite;
        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, boxImg.Length);
            npcTextBox.sprite = boxImg[rand];
        }
    }

   /* public void GetNPCText()
    {
        npcText.sprite = npcText.sprite;
        for (int i = 0; i < 4; i++)
        {
            int randTxt = Random.Range(0, textImg.Length);
            npcText.sprite = textImg[randTxt];
        }
    }*/


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerEnter");
            npcCanvas.gameObject.SetActive(true);
            npcTextBox.gameObject.SetActive(true);
            //npcText.gameObject.SetActive(true);
            StartCoroutine(TextBoxOpen());
        }
    }
    /*public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerStay");
            npcCanvas.gameObject.SetActive(true);
            npcTextBox.gameObject.SetActive(true);
            npcText.gameObject.SetActive(true);
            
        }
    }*/

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerExit");
            if (npcCanvas)
            {
                StartCoroutine(TextBoxClose());
            }
            NewMethod();

        }
    }

    private void NewMethod()
    {
        StartCoroutine(NPCCanvasOff());
    }

    IEnumerator TextBoxOpen()
    {
        GetNPCTextBox();
        //GetNPCText();
        npcTextBox.transform.LeanMove(new Vector3(transform.position.x, (transform.position.y + 2f), 0),0.2f);
        //npcText.transform.position = new Vector3(transform.position.x, (transform.position.y + 2f), 0);
        npcTextBox.transform.LeanScale(Vector2.one, 0.2f);
       // npcText.transform.LeanScale(Vector2.one, 0.2f);
        yield return null;
    }

    IEnumerator TextBoxClose()
    {        
        npcTextBox.transform.LeanMove(new Vector3(transform.position.x, transform.position.y, 0), 0.2f);
       // npcText.transform.LeanMove(new Vector3(transform.position.x, transform.position.y, 0), 0.2f);
        npcTextBox.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
        // npcText.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
        yield return null;
    }

    IEnumerator NPCCanvasOff()
    {
        yield return new WaitForSeconds(6f);
        npcCanvas.gameObject.SetActive(false);
        npcTextBox.gameObject.SetActive(false);
        //npcText.gameObject.SetActive(false);
    }
}
