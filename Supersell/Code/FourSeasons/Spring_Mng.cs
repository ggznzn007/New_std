using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spring_Mng : MonoBehaviour
{
    [SerializeField] GameObject[] flowers;
  
    void Start()
    {        
        StartCoroutine(TimeToFade());       
    }
    
    void Update()
    {
        if(DataManager.Instance.isPlaying&&!DataManager.Instance.isPaused)
        {
            if (!DataManager.Instance.isPlaying) return;
            TouchPoint();
        }     
    }

   public void TouchPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                StartCoroutine(DelayAppear(hit));               
            }
        }
    }

    IEnumerator DelayAppear(RaycastHit hit)
    {
        int random = Random.Range(0, flowers.Length);
        Vector3 hitPos = hit.point;
        GameObject myFlower = Instantiate(flowers[random], hitPos, flowers[random].transform.rotation);
        myFlower.transform.LeanScale(new Vector3(10, 10, 10), 0.7f).setEaseLinear();
        yield return new WaitForSecondsRealtime(2);
       // myFlower.transform.LeanScale(new Vector3(0, 0, 0), 1).setEaseLinear();
      //  yield return new WaitForSecondsRealtime(1.1f);
      //  Destroy(myFlower);
    }

    IEnumerator TimeToFade()
    {
        yield return new WaitForSeconds(1);        
        FadeScreen.Instance.OnFade(FadeState.FadeIn);
        DataManager.Instance.isPlaying = true;
        yield return new WaitForSecondsRealtime(DataManager.Instance.SceneTime_Spring);
        FadeScreen.Instance.OnFade(FadeState.FadeOut);
        DataManager.Instance.isPlaying = false;
        yield return new WaitForSecondsRealtime(DataManager.Instance.DelayTime);
        SceneManager.LoadScene(1);
    }
}
