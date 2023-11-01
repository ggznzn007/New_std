using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class API_Call : MonoBehaviour
{
    public class Root
    {
        public int code { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
    private string scanURL = "http://192.168.0.106:15464/testScan";
    void Start()
    {

    }

    void ReFresch()
    {
        StartCoroutine(GetRequest("http://192.168.0.106:15464/testScan"));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ReFresch();
        }
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(string.Format("Something went wrong:{0}", webRequest.error));
                    break;
                case UnityWebRequest.Result.Success:
                    
                    break;
            }
        }
    }
}
