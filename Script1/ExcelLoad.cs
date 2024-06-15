using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelLoad : MonoBehaviour
{
    public Entity_TypicalQuestions item;
    
    void Start()
    {
        int cnt = item.sheets[0].list.Count;
        int ran = Random.Range(0, cnt);
        Debug.Log(item.sheets[0].list[ran].Question);

        /*for (int i = 0; i<item.sheets[0].list.Count;i++)
        {
            Debug.Log(item.sheets[0].list[i].Question);
            Debug.Log(item.sheets[0].list[i].Answer1);
            Debug.Log(item.sheets[0].list[i].Answer2);
            Debug.Log(item.sheets[0].list[i].Answer3);
            
        }*/
    }   
}
