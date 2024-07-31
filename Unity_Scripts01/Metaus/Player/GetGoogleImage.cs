using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;

public class GetGoogleImage : MonoBehaviour
{
    private SpriteRenderer parentsRenderer;
    public RawImage userImage;
   
    void Start()
    {
        parentsRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Texture2D texture2D = Social.localUser.image;
        userImage.texture = texture2D;
    }
}
