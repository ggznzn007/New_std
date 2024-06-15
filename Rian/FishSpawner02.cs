using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner02 : MonoBehaviour
{
    [SerializeField] GameObject[] fishPrefabs; // ������ �����
    [SerializeField] int fishCount; // ������ ����� ��
    private BoxCollider area; // ����� ���� ������ ���� ����
    private List<GameObject> fish = new List<GameObject>();// ����� ����Ʈ
    void Start()
    {
        area = GetComponent<BoxCollider>();

        for (int i = 0; i < fishCount; ++i)
        {
            Spawn();            
        }
       area.enabled = false;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);
        return spawnPos;
    }

    private void Spawn()
    {
        int selection = Random.Range(0, fishPrefabs.Length);
        GameObject selectedPrefab = fishPrefabs[selection];
        Vector3 spawnPos = GetRandomPosition(); //������ġ�Լ�
        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);        
    }    
}
