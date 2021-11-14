using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class YoutubeManager : MonoBehaviour
{
    public YoutubeSimplified youtube;
    public InputField field;
    string inputURL = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputURL = field.text;
    }
    public void changeVideo()
    {
        youtube.url = inputURL;
    }
}
