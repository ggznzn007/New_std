using System.Collections;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] private GameObject prefab;
    [SerializeField][Range(0, 100)] private float chance = 0;

    public float Weight { set; get; }

    public GameObject Prefab => prefab;
    public float Chance => chance;
}

public class Percent_Spawner : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField][Range(0, 1000)] private int maxItemCount = 100;
    [SerializeField] private Vector2 min = new(-8, -4);
    [SerializeField] private Vector2 max = new(8, 4);
    private float accumulateWeights;

    private void Awake()
    {
        CaculateWeight();
    }

    IEnumerator Start()
    {
        int count = 0;

        while(count<maxItemCount)
        {
            SpawnItem(new (Random.Range(min.x, max.x),Random.Range(min.y, max.y)));
            count++;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void CaculateWeight()
    {
        accumulateWeights = 0;
        foreach(var  item in items)
        {
            accumulateWeights += item.Chance;
            item.Weight = accumulateWeights;
        }
    }

    void SpawnItem(Vector2 position)
    {
        var clone = items[GetRandomIndex()];

        Instantiate(clone.Prefab, position, Quaternion.identity);

        Debug.Log($"{clone.Prefab.name} {clone.Chance}%");
    }

    int GetRandomIndex()
    {
        float random = Random.value * accumulateWeights;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Weight>= random)
            {
                return i;
            }
        }

        return 0;
    }
}
