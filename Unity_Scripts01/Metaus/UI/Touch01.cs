using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Touch01 : MonoBehaviour
{
    public GameObject effectPrefab;
    float spawnsTime;
    public float defaultTime = 0.05f;

    private void Update()
    {
        if (Input.GetMouseButton(0) && spawnsTime >= defaultTime)
        {
            EffectCreate();
            spawnsTime = 0;
        }
        spawnsTime += Time.deltaTime;
    }
    public void EffectCreate()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0;
        Instantiate(effectPrefab, mPosition, Quaternion.identity);
    }
}

