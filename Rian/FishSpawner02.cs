using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner02 : MonoBehaviour
{
    [SerializeField] GameObject[] fishPrefabs; // 생성할 물고기
    [SerializeField] int fishCount; // 생성할 물고기 수
    private BoxCollider area; // 물고기 생성 범위를 위한 공간
    private List<GameObject> fish = new List<GameObject>();// 물고기 리스트
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
        Vector3 spawnPos = GetRandomPosition(); //랜덤위치함수
        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);        
    }    
}
