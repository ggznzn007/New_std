using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonDrinkFactory : MonoBehaviour
{
    Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();
    public GameObject cider;

    public GameObject getCider(string name)
    {
        if(!dic.ContainsKey(name))
        {
            float x = (float)Random.Range(-10, 11);
            float z = (float)Random.Range(-10, 11);
            Vector3 pos = new Vector3(x, 1f, z);

            GameObject obj = Instantiate(cider, pos, Quaternion.identity);
            obj.GetComponent<Cider>().setName(name);
            dic.Add(name, obj);
        }
        return dic[name];
    }
}
