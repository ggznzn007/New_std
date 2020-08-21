using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTitleScreen : MonoBehaviour
{
    public GUISkin skin;
    Quaternion originalRotation;
    void Start()
    {
        originalRotation = transform.localRotation;
    }
    void Update()
    {
        transform.localRotation =
            Quaternion.AngleAxis(Mathf.Sin(2f * Time.time) * 20f, Vector3.up)
          * Quaternion.AngleAxis(Mathf.Sin(2f * Time.time) * 20f, Vector3.right)
          * originalRotation;


        // RotationObj
        transform.parent.localRotation =
            Quaternion.AngleAxis(Time.deltaTime * 10f, Vector3.up) *
            transform.parent.localRotation;
    }
    private void OnGUI()
    {
        GUI.skin = skin;
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.Label(new Rect(0, 0.6f * sh, sw, 0.4f * sh),
            "PRESS SPACE TO START", "Title");
    }
}
