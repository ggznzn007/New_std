using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class DecoGarden : MonoBehaviour
{
    public GameObject btnplantList; //식물리스트 버튼
    public GameObject btnPlant; //식물버튼
    public GameObject btnDecoFinish;//꾸미기완료버튼
    public GameObject plantPosCollection; //화살표를 담아두는 오브젝트
    public GameObject gardenCamera;
    public GameObject[] plantPrefab; //식물 프리팹 배열 
    public GameObject[] plantPopUp; //식물 팝업 배열
    public GameObject[] locks;
    [SerializeField] GameObject FlowerParticle;//식물 파티클

    public GameObject pupUPobj; //식물을 선택했을때 나오는 팝업
    GameObject placeObject;//선택한 식물 프리팹을 담기위해
    GameObject flower;//식물프리팹을 생성하기 위해 옮겨서 생성해야 clone을 지울 수 있다.
    private bool isbtnPlantChecked;//버튼이 true일때만 실행되게 하기위해
    private bool isbtnDecoFinish;//정원꾸미기 완료상태인지 꾸미기 상태인지 확인하기 위해서
    string plantNum = "00";

    public static DecoGarden decoGarden;
    public List<GameObject> plants = new List<GameObject>(); //식물 프리팹을 한번에 제거하기 위해서

    Transform canvastf;
    Touch touch;//터치 상태를 나타내기 위해      

    private void Awake()
    {
        decoGarden = this; //GetComponent<DecoGarden>();//Garden 스크립트를 다른곳에서 가져오기 위해서
        canvastf = GameObject.Find("Canvas").transform;
    }
    public void Select(int num)//식물선택
    {
      


        if (plantPrefab != null)
        {
            placeObject = plantPrefab[num];
            Debug.Log("선택한 식물 프리팹 :" + num);



            if (PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround != null)
            {
                Debug.Log("debug : spawnedObject is not null");
                //Vector3 Pos = PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.transform.position;
                //plantPosCollection.transform.position = new Vector3(Pos.x, 100f, Pos.z);
                //plantPosCollection.transform.position = new Vector3(Pos.x, Pos.y + 0.1f, Pos.z);
                //plantPosCollection.SetActive(true);//식물을 선택한 후 놓을 화살표UI 활성화

                //Transform tr = PlaceObjectsOnPlane.PlaceObjectsOn.groundPrefab.transform.GetChild(5);
                //tr.gameObject.SetActive(true);

                
                GameObject collection = PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.GetComponent<GardenStageArrow>().plantPosCollectionGarden;
                collection.SetActive(true);
                GameObject[] ar = PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.GetComponent<GardenStageArrow>().arrowsArrayGarden;
                
                for (int i = 0; i < ar.Length; i++)
                {
                    GameObject obj = ar[i].transform.parent.gameObject;
                    int n = obj.transform.childCount;
                    Debug.Log("-------------------------childCount : " + n);
                    if (n == 1)
                    {

                        //ar[i].transform.parent.gameObject.SetActive(true);
                        ar[i].transform.gameObject.SetActive(true);
                    }
                }
               
                Debug.Log("debug : 화살표 활성화");

            }

        }
    }

    private void Update()
    {

        if (Input.touchCount > 0 && (touch = Input.GetTouch(0)).phase == TouchPhase.Began && isbtnDecoFinish == false) //터치를 할 때마다 실행
        {
            TouchArrow();
        }
    }

    public void TouchArrow()
    {

        if (touch.phase == TouchPhase.Began)
        {

            Camera camera = GameObject.Find("AR Camera").GetComponent<Camera>();
            Ray ray;
            ray = camera.ScreenPointToRay(Input.mousePosition);
            //ray = camera.ScreenPointToRay(touch.position);//카메라를 기준으로 터치한 곳에 레이를 쏜다
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 300f, 1 << 8))//터치한 것이 화살표이면
            {
                Debug.Log("debug : 화살표터치");
                StartCoroutine(coFlowerStartParticle(hit));

                GameObject collection = PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.GetComponent<GardenStageArrow>().plantPosCollectionGarden;
                collection.SetActive(true);

                GameObject[] ar = PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.GetComponent<GardenStageArrow>().arrowsArrayGarden;
                Debug.Log("-------------------------ar.length : " + ar.Length);

                for (int i = 0; i < ar.Length; i++)
                {
                    GameObject obj = ar[i].transform.parent.gameObject;
                    int n = obj.transform.childCount;
                    Debug.Log("-------------------------childCount : " + n);
                    if (n == 1)
                    {
                        Debug.Log("-------------------" + i + "번째 자식 카운트" + n);
                        ar[i].transform.gameObject.SetActive(false);
                        //transform.parent.gameObject.SetActive(false);
                    }
                }

                //for (int i = 0; i < arrowsArray.Length; i++)
                //{

                //    GameObject obj = arrowsArray[i].transform.parent.gameObject;//arrow부모오브젝트 찾기
                //    int num = obj.transform.childCount;
                //    if (num == 1)//식물이 배치되지 않은 화살표이면
                //    {
                //        arrowsArray[i].SetActive(false);//비활성화
                //    }
                //}
            }
            else if (Physics.Raycast(ray, out hit, 300f, 1 << 9))//식물이면
            {
                
                string plantName = hit.transform.gameObject.name;  //터치한 식물의 이름을 얻기위해  
                plantNum = plantName.Substring(0, 2); //앞에 2자리만 받기위해서 
                Debug.Log("debug : plantNum = " + plantNum);
                int i = 0;//반복문을 실행하기 위해
                while (true)
                {
                    string pupName = plantPopUp[i].name; // plantPopUp배열 i번째 이름을 가져온다
                    string pupNum = pupName.Substring(0, 2); // 앞의 2자리만 받는다
                    Debug.Log("debug : pupNum = " + pupNum);
                    if (plantNum == pupNum) //식물의 앞의 2자리와 plantPupUp i번째 이름의 2자리가 같으면
                    {
                        //GameObject obj = Instantiate(plantPopUp[i], canvastf.position,Quaternion.identity, canvastf); //해당 팝업창 생성후 캔버스안에 넣기        
                        pupUPobj = plantPopUp[i];
                        pupUPobj.SetActive(true);
                        Debug.Log("debug : 해당팝업창활성화");                        
                        break;                        
                    }
                    i++;
                }
                //StartCoroutine(coFlowerDestroyParticle(hit));
            }
        }
    }

    public void btnYes()
    {
        string plocknum = PlayerPrefs.GetString("locknum");
        Debug.Log("락넘버 : "+PlayerPrefs.GetString("locknum"));
        Debug.Log("락 길이 : " + locks.Length);
        for(int i = 0; i<locks.Length; i++)
        {
            string lockName = locks[i].name;
            string lockNum = lockName.Substring(0, 2);
            Debug.Log("배열 락넘버 : " + lockNum);

            if (plocknum == lockNum)
            {
                locks[i].SetActive(false);
                break;
            }
        }   
     }

    public void removeArrows()
    {
        GameObject[] ar = PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.GetComponent<GardenStageArrow>().arrowsArrayGarden;

        for (int i = 0; i < ar.Length; i++)
        {
            GameObject obj = ar[i].transform.parent.gameObject;
            int n = obj.transform.childCount;
            Debug.Log("-------------------------childCount : " + n);
            if (n == 1)
            {
                Debug.Log("-------------------" + i + "번째 자식 카운트" + n);
                ar[i].transform.gameObject.SetActive(false);
                //transform.parent.gameObject.SetActive(false);
            }
        }
    }    
        
    IEnumerator coFlowerStartParticle(RaycastHit hit)
    {
        Vector3 Pos = hit.transform.position;
        //flower = Instantiate(FlowerParticle, Pos, Quaternion.identity, hit.transform.parent);//화살표 위치에 프리팹 생성
        ParticleManager.instance.PlayPlaEf("PE1",hit,1f);
        yield return new WaitForSeconds(1f);
        flower = Instantiate(placeObject, Pos, Quaternion.identity, hit.transform.parent);//화살표 위치에 프리팹 생성
        flower.transform.rotation = new Quaternion(0f, 180f, 0f, 0);
        hit.transform.gameObject.SetActive(false);//화살표 비활성화
        plants.Add(flower);//리스트에 식물 프리팹 추가
                           //식물이 배치되어있지 않은 화살표 제거

    }

    IEnumerator coFlowerDestroyParticle(RaycastHit hit)
    {
        Vector3 Pos = hit.transform.position;
        //flower = Instantiate(FlowerParticle, Pos, Quaternion.identity, hit.transform.parent);//화살표 위치에 프리팹 생성
        ParticleManager.instance.PlayPlaEf("PE2", hit, 1f);
        yield return new WaitForSeconds(1f);

        //여기에 팝업창이 뜨게 해야함

        GameObject temp = hit.transform.parent.Find("arrow").gameObject; //arrow오브젝트 찾기
        temp.SetActive(true);//arrow 활성화
        Destroy(hit.transform.gameObject);//식물 프리팹 파괴

    }

    public void removePlants()//홈버튼을 눌렀을 때 실행되는 함수
    {
        //for (int i = 0; i < plants.Count; i++)
        //{

        //    Destroy(plants[i]);//리스트에 담겨진 식물 프리팹 모두 제거
        //}

        removeArrows();
        btnplantList.SetActive(false);//식물 리스트 비활성화
        if (isbtnDecoFinish)//구경하기 상태에서 홈버튼을 눌렀을 시
        {
            btnPlant.SetActive(true); //식물버튼 활성화
            gardenCamera.SetActive(false); //카메라 비활성화
            isbtnDecoFinish = false; //정원꾸미기 상태로 전환
        }
        if (PlaceObjectsOnPlane.PlaceObjectsOn.spawnedObject != null)
        {
            PlaceObjectsOnPlane.PlaceObjectsOn.spawnedObject.SetActive(false);

        }
        if (PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround != null)
        {
            PlaceObjectsOnPlane.PlaceObjectsOn.spawnedGround.SetActive(false);
        }
    }

    public void clickBtnPlant()
    {
        isbtnPlantChecked = !isbtnPlantChecked;//버튼을 누르면 bool형이 바뀜
        if (isbtnPlantChecked)
        {
            btnplantList.SetActive(true);//식물 리스트 활성화
        }
        else
        {
            btnplantList.SetActive(false);//식물 리스트 비활성화
        }
    }

    public void clickbtnDecoFinish()
    {        
        isbtnDecoFinish = !isbtnDecoFinish;
        if (isbtnDecoFinish)//꾸미기를 완료했으면
        {
            gardenCamera.SetActive(true); //카메라UI띄우기
            btnplantList.SetActive(false); //식물리스트 비활성화
            btnPlant.SetActive(false);  //식물 비활성화
            if (btnplantList != null)
            {
                btnDecoFinish.GetComponentInChildren<Text>().text = "Decorate";//버튼 텍스트 바꾸기
            }

        }
        else//다시 정원꾸미기이면
        {
            gardenCamera.SetActive(false); //카메라 비활성화
            btnPlant.SetActive(true);  //식물활성화                  
            if (btnplantList != null)
            {
                btnDecoFinish.GetComponentInChildren<Text>().text = "Observe";//버튼 텍스트 바꾸기
            }
        }
    }
    private void Start()
    {
        btnDecoFinish.GetComponentInChildren<Text>().text = "Observe";
    }



}