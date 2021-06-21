using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FairyImage : MonoBehaviour
{

    //버튼의 이미지를 Resources에서 가져와서 바꾸어라

    private void Start()
    {
        if (PlayerPrefs.HasKey("Fairy"))
        {
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("FairyImage/" + PlayerPrefs.GetInt("Fairy"));

        }
    }
}
