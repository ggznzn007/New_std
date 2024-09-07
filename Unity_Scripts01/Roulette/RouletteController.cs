using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RouletteController : MonoBehaviour
{
    public RectTransform roulette;
    public Image finalImage;
    public List<Image> contents;
    public Button rouletteStart;

    bool rolling;

    float initSpeed = 1000;
    float breakSpeed = 800;
    float keepSpeedTimeMin = 1, keepSpeedTimeMax = 3;

    float currentTime;
    float currentSpeed;

    private void Start()
    {
        contents = new List<Image>();
        for (int i = 0; i < roulette.childCount; i++)
        {
            contents.Add(roulette.GetChild(i).GetComponent<Image>());
        }
    }

    private void Update()
    {
        StartCoroutine(RouletteRoll());       
    }
    
    //call when you want to roll the roulette
    public void Roll()
    {
        rolling = true;
        currentSpeed = initSpeed;
        currentTime = Random.Range(keepSpeedTimeMin, keepSpeedTimeMax);        
    }

     private IEnumerator RouletteRoll()
    {
        rouletteStart.interactable = rolling == false;

        if (rolling)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentSpeed -= breakSpeed * Time.deltaTime;
            }
            roulette.Rotate(0, 0, -currentSpeed * Time.deltaTime);

            if (currentSpeed <= 0)
            {
                float halfAng = 360 / contents.Count * 0.5f;
                float minAng = 360;
                Image targetImage = null;
                for (int i = 0; i < contents.Count; i++)
                {
                    Vector3 localDir = Quaternion.Euler(0, 0, halfAng + (i * 360 / contents.Count)) * Vector3.up;
                    float angle = Vector3.Angle(roulette.TransformDirection(localDir), Vector3.up);
                    if (angle <= minAng)
                    {
                        minAng = angle;
                        targetImage = contents[i]; //get target

                    }
                }
                finalImage.transform.position = new Vector3(finalImage.transform.position.x, finalImage.transform.position.y, 0);
                finalImage.transform.LeanScale(Vector2.one, 0.2f);
                finalImage.sprite = targetImage.sprite;
                rolling = false;
                yield return new WaitForSeconds(1.5f);
                finalImage.transform.LeanMove(new Vector3(finalImage.transform.position.x, finalImage.transform.position.y, 0), 0.2f);
                finalImage.transform.LeanScale(Vector2.zero, 0.2f).setEaseOutBack();                
            }
        }
    }   
}
