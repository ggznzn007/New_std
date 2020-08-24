using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRotateCube : MonoBehaviour
{
    public static int numX = 0;

    public static int AddTwoNum(int x, int y)
    {
        return x + y;
    }

    public void Rotate1()
    {
        transform.Rotate(Vector3.up * 90f);
    }

    private void Rotate2()
    {
        transform.Rotate(Vector3.up * -90f);
    }
}
