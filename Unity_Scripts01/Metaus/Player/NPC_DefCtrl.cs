using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NPC_DefCtrl : MonoBehaviour
{
    public TextMeshPro npcText;
    public Canvas npcCanvas;
    public Sprite[] boxImg;
    public Image npcTextBox;

    private PlayerController player;
    private PlayerController2 player2;
    private string loadText = "æ»≥Á«œººø‰!";
    
    public void Start()
    {
        npcCanvas.gameObject.SetActive(false);
        
        npcText.transform.position = gameObject.transform.position;
        npcTextBox.gameObject.transform.position = gameObject.transform.position;
    }
    
    public void GetNPCTextBox()
    {
        npcTextBox.sprite = npcTextBox.sprite;
        for (int i = 0; i < boxImg.Length; i++)
        {
            int rand = Random.Range(0, boxImg.Length);
            npcTextBox.sprite = boxImg[rand];
        }
    }

    /* IEnumerator TypingText()
    {
        npcText.transform.position = new Vector3(transform.position.x, (transform.position.y + 1.65f), 0);
        npcText.transform.LeanScale(Vector2.one, 0.2f);
        for (int i = 0; i < loadText.Length; i++)
        {
            npcText.text = loadText.Substring(0, i + 1);

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.8f);
        npcText.transform.LeanMoveLocal(new Vector3(transform.position.x, transform.position.y, 0), 0.2f);
        npcText.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();
    }*/


    IEnumerator TextBoxOpen()
    {
        GetNPCTextBox();
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
    
    IEnumerator NPCCanvasOff()
    {
        yield return new WaitForSeconds(5f);
        npcCanvas.gameObject.SetActive(false);
        npcTextBox.gameObject.SetActive(false);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("NPCTriggerEnter");
            npcCanvas.gameObject.SetActive(true);
            npcTextBox.gameObject.SetActive(true);
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
            StartCoroutine(TextBoxClose());            
            StartCoroutine(NPCCanvasOff());
        }
    }
}
