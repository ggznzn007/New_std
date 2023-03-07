using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotState
{
    Stop,
    Left,
    Right
}

public class CameraRot : MonoBehaviour
{
    public float Speed;
    public bool isStop;    
    public RotState state;

    private void Start()
    {
        state = RotState.Left;        
        isStop = false;
    }

    private void Update()
    {
        KeyCtrl();
    }
    void FixedUpdate()
    {
        RotateCtrl();
    }

    public void KeyCtrl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isStop)
            {
                state = RotState.Stop;
                isStop = true;
            }
            else if(isStop) 
            { 
                state= RotState.Left;                
                isStop = false;
            }          
        }
       /* if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            state = RotState.Left;
            isLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            state = RotState.Right;
            isLeft= false;
        }*/
    }

    public void RotateCtrl()
    {
        switch (state)
        {
            case RotState.Stop:
                transform.Rotate(Speed * Time.deltaTime * Vector3.zero);
                break;
            case RotState.Left:
                transform.Rotate(Speed * Time.deltaTime * Vector3.down);
                break;
          /*  case RotState.Right:
                transform.Rotate(Speed * Time.deltaTime * Vector3.up);
                break;*/
        }
    }

}
