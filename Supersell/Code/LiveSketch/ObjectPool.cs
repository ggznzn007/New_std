using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    #region 변수 선언부
    public static ObjectPool OP;
    public int ObjectCount = 10;   
    public List<Transform> treePoints;
    public List<Transform> groundPoints;

    public List<GameObject> Ante;
    public List<GameObject> Blac;
    public List<GameObject> Cham;
    public List<GameObject> Elep;
    public List<GameObject> Gori;
    public List<GameObject> Jagu;
    public List<GameObject> Okap;
    public List<GameObject> Pois;
    public List<GameObject> Slot;
    public List<GameObject> Toco;

    public List<GameObject> Baro;
    public List<GameObject> Brac;
    public List<GameObject> Camp;
    public List<GameObject> Hete;
    public List<GameObject> Huay;
    public List<GameObject> Lufe;
    public List<GameObject> Mame;
    public List<GameObject> Scel;
    public List<GameObject> Shun;
    public List<GameObject> Tuoj;


    private Queue<GameObject> objectPool_Ante = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Blac = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Cham = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Elep = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Gori = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Jagu = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Okap = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Pois = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Slot = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Toco = new Queue<GameObject>();

    public Queue<GameObject> objectPool_Baro = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Brac = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Camp = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Hete = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Huay = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Lufe = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Mame = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Scel = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Shun = new Queue<GameObject>();
    private Queue<GameObject> objectPool_Tuoj = new Queue<GameObject>();  
    
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f); // 생성 영역 크기
    public int maxSpawnAttempts = 100; // 최대 시도 횟수
    public float minDistance = 2f; // 최소 거리 (겹치지 않게 할 최소 거리)
    private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트를 저장할 리스트

    private int addCount_Ante;
    private int addCount_Blac;
    private int addCount_Cham;
    private int addCount_Elep;
    private int addCount_Gori;
    private int addCount_Jagu;
    private int addCount_Okap;
    private int addCount_Pois;
    private int addCount_Slot;
    private int addCount_Toco;

    private int addCount_Baro;
    private int addCount_Brac;
    private int addCount_Camp;
    private int addCount_Hete;
    private int addCount_Huay;
    private int addCount_Lufe;
    private int addCount_Mame;
    private int addCount_Scel;
    private int addCount_Shun;
    private int addCount_Tuoj;
    #endregion

    #region 유니티 메서드 집합
    private void Start()
    {
        OP = this;
        InitAdd();
        InitializePool_Ante();
        InitializePool_Blac();
        InitializePool_Cham();
        InitializePool_Elep();
        InitializePool_Gori();
        InitializePool_Jagu();
        InitializePool_Okap();
        InitializePool_Pois();
        InitializePool_Slot();
        InitializePool_Toco();

        InitializePool_Baro();
        InitializePool_Brac();
        InitializePool_Camp();
        InitializePool_Hete();
        InitializePool_Huay();
        InitializePool_Lufe();
        InitializePool_Mame();
        InitializePool_Scel();
        InitializePool_Shun();
        InitializePool_Tuoj();
    }
    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(x, 0, z);
    }

    bool CheckOverlap(Vector3 newPosition)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (Vector3.Distance(obj.transform.position, newPosition) < minDistance)
            {
                return true; // 겹침
            }
        }
        return false; // 겹치지 않음
    }

    void InitAdd()
    {
        addCount_Ante = 0;
        addCount_Blac = 0;
        addCount_Cham = 0;
        addCount_Elep = 0;
        addCount_Gori = 0;
        addCount_Jagu = 0;
        addCount_Okap = 0;
        addCount_Pois = 0;
        addCount_Slot = 0;
        addCount_Toco = 0;

        addCount_Baro = 0;
        addCount_Brac = 0;
        addCount_Camp = 0;
        addCount_Hete = 0;
        addCount_Huay = 0;
        addCount_Lufe = 0;
        addCount_Mame = 0;
        addCount_Scel = 0;
        addCount_Shun = 0;
        addCount_Tuoj = 0;
    }
    #endregion

    #region  오브젝트 생성 메서드 집합
    void InitializePool_Ante() //개미핥기 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
                // 오브젝트 생성 및 위치 설정
                int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
                GameObject obj = Instantiate(Ante[i], groundPoints[rand[0]].position, Ante[i].transform.rotation);
                obj.SetActive(false); // 비활성화 상태로 시작
                objectPool_Ante.Enqueue(obj);
                spawnedObjects.Add(obj);
            }
        }
       /* for (int i = 0; i < ObjectCount; i++)
        {
            //int rand = Random.Range(0, groundPoints.Count);
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Ante[i], groundPoints[rand[0]].position, Ante[i].transform.rotation);           
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Ante.Enqueue(obj);
        }*/
    }
    void InitializePool_Blac() //악어 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();//추가

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);// 추가

            if (!isOverlapping)//추가
            {
                int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
                GameObject obj = Instantiate(Blac[i], groundPoints[rand[0]].position, Blac[i].transform.rotation);
                obj.SetActive(false); // 비활성화 상태로 시작
                objectPool_Blac.Enqueue(obj);
            }
        }
    }
    void InitializePool_Cham() //카멜레온 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
                int[] rand = Setting.RandomNumbers(treePoints.Count, 1);
                GameObject obj = Instantiate(Cham[i], treePoints[rand[0]].position, Cham[i].transform.rotation);
                obj.SetActive(false); // 비활성화 상태로 시작
                objectPool_Cham.Enqueue(obj);
            }
        }
    }
    void InitializePool_Elep() //코끼리 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
                int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
                GameObject obj = Instantiate(Elep[i], groundPoints[rand[0]].position, Elep[i].transform.rotation);
                obj.SetActive(false); // 비활성화 상태로 시작
                objectPool_Elep.Enqueue(obj);
            }
        }
    }
    void InitializePool_Gori() //고릴라 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
                int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
                GameObject obj = Instantiate(Gori[i], groundPoints[rand[0]].position, Gori[i].transform.rotation);
                obj.SetActive(false); // 비활성화 상태로 시작
                objectPool_Gori.Enqueue(obj);
            }
        }
    }
    void InitializePool_Jagu() //재규어 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
                int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
                GameObject obj = Instantiate(Jagu[i], groundPoints[rand[0]].position, Jagu[i].transform.rotation);
                obj.SetActive(false); // 비활성화 상태로 시작
                objectPool_Jagu.Enqueue(obj);
            }
        }
    }
    void InitializePool_Okap() //오카피 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Okap[i], groundPoints[rand[0]].position, Okap[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Okap.Enqueue(obj);
            }
        }
    }
    void InitializePool_Pois() //독 개구리 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Pois[i], groundPoints[rand[0]].position, Pois[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Pois.Enqueue(obj);
            }
        }
    }
    void InitializePool_Slot() //나무늘보 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(treePoints.Count, 1);
            GameObject obj = Instantiate(Slot[i], treePoints[rand[0]].position, Slot[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Slot.Enqueue(obj);
            }
        }
    }
    void InitializePool_Toco() //토코 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(treePoints.Count, 1);
            GameObject obj = Instantiate(Toco[i], treePoints[rand[0]].position, Toco[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Toco.Enqueue(obj);
            }
        }
    }

    void InitializePool_Baro() // 바로사우루스 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Baro[i], groundPoints[rand[0]].position, Baro[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Baro.Enqueue(obj);
            }
        }
    }
    void InitializePool_Brac() // 브라키오사우루스 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Brac[i], groundPoints[rand[0]].position, Brac[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Brac.Enqueue(obj);
            }
        }
    }
    void InitializePool_Camp() // 캡터사우루스 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Camp[i], groundPoints[rand[0]].position, Camp[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Camp.Enqueue(obj);
            }
        }
    }
    void InitializePool_Hete() // 헤테로돈토사우루스 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Hete[i], groundPoints[rand[0]].position, Hete[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Hete.Enqueue(obj);
            }
        }
    }
    void InitializePool_Huay() // 후양고사우루스 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Huay[i], groundPoints[rand[0]].position, Huay[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Huay.Enqueue(obj);
            }
        }
    }
    void InitializePool_Lufe() // 루펜고사우루스 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Lufe[i], groundPoints[rand[0]].position, Lufe[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Lufe.Enqueue(obj);
            }
        }
    }
    void InitializePool_Mame() // 메멘키사우루스 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Mame[i], groundPoints[rand[0]].position, Mame[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Mame.Enqueue(obj);
            }
        }
    }
    void InitializePool_Scel() // 스사우루스 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Scel[i], groundPoints[rand[0]].position, Scel[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Scel.Enqueue(obj);
            }
        }
    }
    void InitializePool_Shun() // 셔노사우루스 - fix
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Shun[i], groundPoints[rand[0]].position, Shun[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Shun.Enqueue(obj);
            }
        }
    }
    void InitializePool_Tuoj() // 투지안고사우루스 - move
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // 새로운 위치와 기존 위치 사이의 충돌 확인
            bool isOverlapping = CheckOverlap(randomPosition);

            if (!isOverlapping)
            {
            int[] rand = Setting.RandomNumbers(groundPoints.Count, 1);
            GameObject obj = Instantiate(Tuoj[i], groundPoints[rand[0]].position, Tuoj[i].transform.rotation);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool_Tuoj.Enqueue(obj);
            }
        }
    }
    #endregion

    #region 오브젝트 사용할때 풀에서 꺼내는 메서드 집합
    public GameObject GetFromPool_Ante()  // 개미핥기
    {
        if (objectPool_Ante.Count > 0)
        {
            GameObject obj = objectPool_Ante.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Ante[addCount_Ante], groundPoints[rand].position, Ante[addCount_Ante].transform.rotation);
            newObj.SetActive(true);
            addCount_Ante++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Blac()  // 악어
    {
        if (objectPool_Blac.Count > 0)
        {
            GameObject obj = objectPool_Blac.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Blac[addCount_Blac], groundPoints[rand].position, Blac[addCount_Blac].transform.rotation);
            newObj.SetActive(true);
            addCount_Blac++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Cham()  // 카멜레온
    {
        if (objectPool_Cham.Count > 0)
        {
            GameObject obj = objectPool_Cham.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, treePoints.Count);
            GameObject newObj = Instantiate(Cham[addCount_Cham], treePoints[rand].position, Cham[addCount_Cham].transform.rotation);
            newObj.SetActive(true);
            addCount_Cham++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Elep()  // 코끼리
    {
        if (objectPool_Elep.Count > 0)
        {
            GameObject obj = objectPool_Elep.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Elep[addCount_Elep], groundPoints[rand].position, Elep[addCount_Elep].transform.rotation);
            newObj.SetActive(true);
            addCount_Elep++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Gori()  // 고릴라
    {
        if (objectPool_Gori.Count > 0)
        {
            GameObject obj = objectPool_Gori.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Gori[addCount_Gori], groundPoints[rand].position, Gori[addCount_Gori].transform.rotation);
            newObj.SetActive(true);
            addCount_Gori++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Jagu()  // 재규어
    {
        if (objectPool_Jagu.Count > 0)
        {
            GameObject obj = objectPool_Jagu.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Jagu[addCount_Jagu], groundPoints[rand].position, Jagu[addCount_Jagu].transform.rotation);
            newObj.SetActive(true);
            addCount_Jagu++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Okap()  // 오카피
    {
        if (objectPool_Okap.Count > 0)
        {
            GameObject obj = objectPool_Okap.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Okap[addCount_Okap], groundPoints[rand].position, Okap[addCount_Okap].transform.rotation);
            newObj.SetActive(true);
            addCount_Okap++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Pois()  // 독개구리
    {
        if (objectPool_Pois.Count > 0)
        {
            GameObject obj = objectPool_Pois.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Pois[addCount_Pois], groundPoints[rand].position, Pois[addCount_Pois].transform.rotation);
            newObj.SetActive(true);
            addCount_Pois++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Slot()  // 나무늘보
    {
        if (objectPool_Slot.Count > 0)
        {
            GameObject obj = objectPool_Slot.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Slot[addCount_Slot], groundPoints[rand].position, Slot[addCount_Slot].transform.rotation);
            newObj.SetActive(true);
            addCount_Slot++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Toco()  // 토코

    {
        /*foreach (var obj in objectPool_Toco)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                objectPool_Toco.Dequeue();
                return obj;
            }
        }   
        return null; // 풀이 가득 찬 경우에도 비활성화할 오브젝트가 없는 경우*/
        if (objectPool_Toco.Count > 0)
        {
            GameObject obj = objectPool_Toco.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, treePoints.Count);
            GameObject newObj = Instantiate(Toco[addCount_Toco], treePoints[rand].position, Toco[addCount_Toco].transform.rotation);
            newObj.SetActive(true);
            addCount_Toco++;
            return newObj;
            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }


    public GameObject GetFromPool_Baro()  // 바로사우루스
    {        
        if (objectPool_Baro.Count > 0)
        {            
            GameObject obj = objectPool_Baro.Dequeue();
            obj.SetActive(true);
            return obj;            
        }
       
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Baro[addCount_Baro], groundPoints[rand].position, Baro[addCount_Baro].transform.rotation);
            newObj.SetActive(true);
            addCount_Baro++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Brac()  // 브라키오사우루스
    {
        if (objectPool_Brac.Count > 0)
        {
            GameObject obj = objectPool_Brac.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Brac[addCount_Brac], groundPoints[rand].position, Brac[addCount_Brac].transform.rotation);
            newObj.SetActive(true);
            addCount_Brac++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Camp()  // 캡터사우루스
    {
        if (objectPool_Camp.Count > 0)
        {
            GameObject obj = objectPool_Camp.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Camp[addCount_Camp], groundPoints[rand].position, Camp[addCount_Camp].transform.rotation);
            newObj.SetActive(true);
            addCount_Camp++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Hete()  // 헤테로돈토사우루스
    {
        if (objectPool_Hete.Count > 0)
        {
            GameObject obj = objectPool_Hete.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Hete[addCount_Hete], groundPoints[rand].position, Hete[addCount_Hete].transform.rotation);
            newObj.SetActive(true);
            addCount_Hete++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Huay()  // 후양고사우루스
    {
        if (objectPool_Huay.Count > 0)
        {
            GameObject obj = objectPool_Huay.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Huay[addCount_Huay], groundPoints[rand].position, Huay[addCount_Huay].transform.rotation);
            newObj.SetActive(true);
            addCount_Huay++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Lufe()  // 루펜고사우루스
    {
        if (objectPool_Lufe.Count > 0)
        {
            GameObject obj = objectPool_Lufe.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Lufe[addCount_Lufe], groundPoints[rand].position, Lufe[addCount_Lufe].transform.rotation);
            newObj.SetActive(true);
            addCount_Lufe++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Mame()  // 메멘키사우루스
    {
        if (objectPool_Mame.Count > 0)
        {
            GameObject obj = objectPool_Mame.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Mame[addCount_Mame], groundPoints[rand].position, Mame[addCount_Mame].transform.rotation);
            newObj.SetActive(true);
            addCount_Mame++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Scel()  // 사우루스
    {
        if (objectPool_Scel.Count > 0)
        {
            GameObject obj = objectPool_Scel.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Scel[addCount_Scel], groundPoints[rand].position, Scel[addCount_Scel].transform.rotation);
            newObj.SetActive(true);
            addCount_Scel++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Shun()  // 셔노사우루스
    {
        if (objectPool_Shun.Count > 0)
        {
            GameObject obj = objectPool_Shun.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Shun[addCount_Shun], groundPoints[rand].position, Shun[addCount_Shun].transform.rotation);
            newObj.SetActive(true);
            addCount_Shun++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }
    public GameObject GetFromPool_Tuoj()  // 투지안고사우루스
    {
        if (objectPool_Tuoj.Count > 0)
        {
            GameObject obj = objectPool_Tuoj.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int rand = Random.Range(0, groundPoints.Count);
            GameObject newObj = Instantiate(Tuoj[addCount_Tuoj], groundPoints[rand].position, Tuoj[addCount_Tuoj].transform.rotation);
            newObj.SetActive(true);
            addCount_Tuoj++;
            return newObj;            // 풀이 가득 찬 경우, 새로운 오브젝트를 풀에 추가
        }
    }

    #endregion

    #region 오브젝트 사용 후 풀에 리턴하는 메서드 집합
    public void ReturnToPool_Ante(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Ante.Enqueue(obj);
    }
    public void ReturnToPool_Blac(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Blac.Enqueue(obj);
    }
    public void ReturnToPool_Cham(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Cham.Enqueue(obj);
    }
    public void ReturnToPool_Elep(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Elep.Enqueue(obj);
    }
    public void ReturnToPool_Gori(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Gori.Enqueue(obj);
    }
    public void ReturnToPool_Jagu(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Jagu.Enqueue(obj);
    }
    public void ReturnToPool_Okap(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Okap.Enqueue(obj);
    }
    public void ReturnToPool_Pois(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Pois.Enqueue(obj);
    }
    public void ReturnToPool_Slot(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Slot.Enqueue(obj);
    }
    public void ReturnToPool_Toco(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Toco.Enqueue(obj);
    }


    public void ReturnToPool_Baro(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Baro.Enqueue(obj);
    }
    public void ReturnToPool_Brac(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Brac.Enqueue(obj);
    }
    public void ReturnToPool_Camp(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Camp.Enqueue(obj);
    }
    public void ReturnToPool_Hete(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Hete.Enqueue(obj);
    }
    public void ReturnToPool_Huay(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Huay.Enqueue(obj);
    }
    public void ReturnToPool_Lufe(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Lufe.Enqueue(obj);
    }
    public void ReturnToPool_Mame(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Mame.Enqueue(obj);
    }
    public void ReturnToPool_Scel(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Scel.Enqueue(obj);
    }
    public void ReturnToPool_Shun(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Shun.Enqueue(obj);
    }
    public void ReturnToPool_Tuoj(GameObject obj)
    {
        obj.SetActive(false);
        objectPool_Tuoj.Enqueue(obj);
    }

    #endregion
}
