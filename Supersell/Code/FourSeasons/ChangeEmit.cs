using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmit : MonoBehaviour
{
    public Material mat;
    private Color colors;
    private Color oldColor;
    private Color newColor;
    private float timer = 0f;
    private float duration = 5;
    private float smoothness = 0.04f;
   
    private void Start()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
        //mat.EnableKeyword("_EmissionColor");
        //Debug.Log("_Emission working");
        //StartCoroutine(LerpColor());        
        // oldColor = mat.color;
        //  InvokeRepeating(nameof(RandomColor), 1, 5);
    }
    
    Color RandomColor()
    {
        oldColor = new Color(Random.Range(44, 191), Random.Range(44, 191), Random.Range(44, 191), 1);
        newColor = new Color(Random.Range(44, 191), Random.Range(44, 191), Random.Range(44, 191), 1);
        colors = Color.Lerp(oldColor, newColor, 1);
        mat.SetColor("_EmissionColor", colors);
        return mat.color;        
    }

    Color PingPong()
    {
        float emission = Mathf.PingPong(Time.deltaTime, 1);
        Color basColor = mat.color;
        Color finalColor = basColor * Mathf.LinearToGammaSpace(emission);
        return finalColor;
    }

    IEnumerator LerpColor()
    {
        float progress = 0; // this float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; // the amount of change to apply.
        while (progress < 1)
        {
            colors = Color.Lerp(oldColor, newColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
            mat.SetColor("_EmissionColor", colors);
        }
        //return true;
    }
}
