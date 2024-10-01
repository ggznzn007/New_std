using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class OxygenBar : MonoBehaviour
{
    public GameObject Oxygen_Bar;
    public TextMeshProUGUI LevelNumber;
    public TextMeshProUGUI OxygenNumber;
    Slider oxSlider;
    int oxcnt;
    float slidercount;
    double levelcnt;

    void Start()
    {
        oxSlider = Oxygen_Bar.GetComponent<Slider>();
        oxSlider.value = 0.0f;
        OxygenAdd();        //초당 산소 추가
    }
    
    void Update()
    {
        /*if (oxcnt >= 1)
        {
            ParticleManager.instance.PlayTxtEf("TE1");
        }*/
        oxcnt = GameManager.inst.oxygenCnt + PlayerPrefs.GetInt("GetOfflineTime");      //산소 싱글톤 불러옴
        OxygenNumber.text = (oxcnt % 1000) + " / 1000";
        slidercount = ((float)oxcnt % 1000) / 1000;
        oxSlider.value = slidercount;            //나머지 출력 = 게이지 표시
        levelcnt = (oxcnt / 1000) + 1;            //몫 출력 = 정원 레벨
        LevelNumber.text = $"{levelcnt}";
        PlayerPrefs.SetInt("TotalOxygen", oxcnt);
    }

    void OxygenAdd()
    {
        StartCoroutine(coOxygenAdd());
    }

    IEnumerator coOxygenAdd()
    {
        while (true)
        {
            GameManager.inst.oxygenCnt += 5;         //초당 산소 추가
            yield return new WaitForSeconds(1f);
        }
    }
}
