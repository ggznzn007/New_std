using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotStates
{
    Stop,
    Left,
    Right
}

public class CameraRot : MonoBehaviour
{
    public float Speed = 10;
    public bool isStop;
    public RotStates state;    

    private void Start()
    {
        state = RotStates.Left;
        isStop = false;
    }

    public void Update()
    {
        KeyCtrl();       
    }

    public void FixedUpdate()
    {
        RotateCtrl();
    }

    public void KeyCtrl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStop)
            {
                state = RotStates.Stop;
                isStop = true;
            }
            else if (isStop)
            {
                state = RotStates.Left;
                isStop = false;
            }
        }      
    }

    public void RotateCtrl()
    {
        switch (state)
        {
            case RotStates.Stop:
                transform.Rotate(Speed * Time.deltaTime * Vector3.zero);
                break;
            case RotStates.Left:
                transform.Rotate(Speed * Time.deltaTime * Vector3.down);
                break;
        }
    }
}
