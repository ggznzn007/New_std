using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Winter_Mng : MonoBehaviour
{
    [SerializeField] GameObject[] snowFlakes;
    void Start()
    {            
        StartCoroutine(TimeToFade());
    }

   
    void Update()
    {
        if (DataManager.Instance.isPlaying)
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
        int random = Random.Range(0, snowFlakes.Length);
        Vector3 hitPos = hit.point;
        GameObject mySnow = Instantiate(snowFlakes[random], hitPos, snowFlakes[random].transform.rotation);
        mySnow.transform.LeanScale(new Vector3(1, 1, 1), .5f).setEaseLinear();
       // mySnow.transform.LeanRotateZ(1, .5f).setEaseLinear();
        yield return new WaitForSecondsRealtime(2);
        mySnow.transform.LeanScale(new Vector3(0, 0, 0), .5f).setEaseLinear();
       // mySnow.transform.LeanRotateZ(-1, .5f).setEaseLinear();
        yield return new WaitForSecondsRealtime(1.1f);
         Destroy(mySnow);
    }
    IEnumerator TimeToFade()
    {
        FadeScreen.Instance.OnFade(FadeState.FadeIn);
        DataManager.Instance.isPlaying = true;
        yield return new WaitForSecondsRealtime(Settings.SceneTime);
        FadeScreen.Instance.OnFade(FadeState.FadeOut);
        DataManager.Instance.isPlaying = false;
        yield return new WaitForSecondsRealtime(Settings.DelayTime);
        SceneManager.LoadScene(0);
    }
}
