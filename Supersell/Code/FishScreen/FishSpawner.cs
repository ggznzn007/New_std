using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Debug;
using TMPro;

public class FishSpawner : MonoBehaviour
{    
    [SerializeField] GameObject[] fishPrefabs;                                        // 생성할 물고기    
    [SerializeField] int fishCount;                                                   // 생성할 물고기 수
    [SerializeField] private Color areaColor = new(1, 0, 0, 0.3f);                    // 생성할 지역 색
    [SerializeField] private BoxCollider area;                                                         // 물고기 생성 범위를 위한 공간
    [SerializeField] GameObject hidden;

    //private List<GameObject> fish = new List<GameObject>();                           // 물고기 리스트
    private Vector3 m_Offset;
    private float m_ZCoord;
    private GameObject hiddenStick;

    void Start()
    {
        area = GetComponent<BoxCollider>();        

        for (int i = 0; i < fishCount; ++i)
        {
            Spawn();
        }
        area.enabled = false;
    }

    private void Update()
    {
        HiddenStick();
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new(posX, posY, posZ);
        return spawnPos;
    }

    public void Spawn()
    {       
        int selection = Random.Range(0, fishPrefabs.Length);
        GameObject selectedPrefab = fishPrefabs[selection]; 
        Vector3 spawnPosition = GetRandomPosition();
        GameObject fishIns = Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);
    }

    void HiddenStick()
    {
        if (Input.GetMouseButtonDown(0))                                     // 마우스 클릭시 한번만 호출
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            

            if (Physics.Raycast(ray, out RaycastHit hit, 300f))
            {
                Vector3 hitPos = hit.point;                
                hiddenStick = Instantiate(hidden, hitPos,hidden.transform.rotation);      
                m_ZCoord = Camera.main.WorldToScreenPoint(hiddenStick.transform.position).z;
                m_Offset = hiddenStick.transform.position - GetMouseWorldPosition();
            }
        }

        if (Input.GetMouseButton(0))                                         // 마우스 클릭중(드래그) 계속 호출
        {
            hiddenStick.transform.position = GetMouseWorldPosition()+ m_Offset;
        }

        if (Input.GetMouseButtonUp(0))                                       // 마우스 클릭종료 시 한번만 호출
        {
            StartCoroutine(DelayDestroy());
        }

        if (Input.GetKey(KeyCode.Space))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif   
        }                                // 프로그램 종료
    }

    Vector3 GetMouseWorldPosition()                                        // 마우스 위치값 함수
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    IEnumerator DelayDestroy()                                             // 가상의 봉을 딜레이 후 삭제
    {
        yield return null;
        Destroy(hiddenStick);
    }
}
