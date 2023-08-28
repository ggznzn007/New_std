using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{        
    void Update()
    {
        if(this.transform.rotation.x>45)
        {
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("LocalAvatarHands");
        }
        else
        {
            Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("LocalAvatarHands"));
        }
    }
}
