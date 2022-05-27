using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ObjectPooler.SpawnFromPool<Ball>("Ball", Vector2.zero)
            .Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ObjectPooler.SpawnFromPool<Cube>("Cube", Vector2.zero)
            .Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));
        }
    }
}
