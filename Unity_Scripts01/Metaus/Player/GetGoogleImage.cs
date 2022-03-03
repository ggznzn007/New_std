using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;

public class GetGoogleImage : MonoBehaviour
{
    private SpriteRenderer parentsRenderer;
    public RawImage userImage;
    // Start is called before the first frame update
    void Start()
    {
        parentsRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Texture2D texture2D = Social.localUser.image;
        userImage.texture = texture2D;

    }
}
