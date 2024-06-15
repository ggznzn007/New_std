using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BtnManager : MonoBehaviour
{
    
    public GameObject[] popupNowPrefab;
    public GameObject[] popupNextPrefab;
    public GameObject nextPupUp;
    public GameObject ErrorPopup;

    public void NextpopUp()
    {        
        popupNextPrefab[0].SetActive(true);
        popupNowPrefab[0].SetActive(false);   
    }

    public void PrepopUp()
    {       
        popupNowPrefab[0].SetActive(true);
        popupNextPrefab[0].SetActive(false);       
    }

    public void ClosePopup()
    {
        popupNowPrefab[0].SetActive(false);
        popupNextPrefab[0].SetActive(false);
    }

    //아래 kkm 메서드
    public void BtnMyRecords()
    {
        if (DecoGarden.decoGarden.pupUPobj != null)
        {
            DecoGarden.decoGarden.pupUPobj.SetActive(false);
        }
        nextPupUp.SetActive(true);
    }

    public void BtnClose()
    {        
        if (DecoGarden.decoGarden.pupUPobj != null)
        {
            DecoGarden.decoGarden.pupUPobj.SetActive(false);
        }
        nextPupUp.SetActive(false);
        AnswerText.n = 0;         //질문리스트 대답리스트 감정리스트 모두 초기화
    }

    public void BtnPrev()
    {
        if (DecoGarden.decoGarden.pupUPobj != null)
        {
            DecoGarden.decoGarden.pupUPobj.SetActive(true);
        }
        nextPupUp.SetActive(false);
    }

    public void BtnNext()
    {
        Debug.Log("next");
        //여기에서 텍스트를 바꾸어 주어야 함
    }

    public void CloseErrorPopup()
    {
        ErrorPopup.SetActive(false);
    }
}
