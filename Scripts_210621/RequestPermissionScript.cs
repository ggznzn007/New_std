using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RequestPermissionScript : MonoBehaviour
{
    
    void Start()
    {
        Permission.RequestUserPermission(Permission.FineLocation);
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }
}
