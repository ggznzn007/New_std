using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour
{
    public Camera cam;

    private void Awake() => cam = GetComponent<Camera>();
    
}
