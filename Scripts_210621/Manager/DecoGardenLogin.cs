using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DecoGardenLogin : MonoBehaviour
{
    public GameObject rosePrefab;
    public GameObject plantList;
    public GameObject RosePopUP;
    public GameObject btnnext;
    GameObject flowerLogin;
    Camera cameraLogin;
    Touch touchLogin;
    public bool isPlant = false;
    static List<ARRaycastHit> p_Hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Start()
    {
        cameraLogin = GameObject.Find("AR Camera").GetComponent<Camera>();
    }
    // Update is called once per frame

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (touchLogin.phase == TouchPhase.Began && isPlant == true)
            {
                Debug.Log("debug : 터치준비완료");
                Ray ray;
                ray = cameraLogin.ScreenPointToRay(Input.mousePosition);
                //ray = camera.ScreenPointToRay(touch.position);//카메라를 기준으로 터치한 곳에 레이를 쏜다
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 300f, 1 << 9))// 식물이면
                {
                    Debug.Log("debug : " + hit.transform.name);
                    RosePopUP.SetActive(true);
                    btnnext.SetActive(true);
                    Debug.Log("debug: 팝업창 열기");
                }
                if (Physics.Raycast(ray, out hit, 300f, 1 << 8))//화살표이면
                {
                    Debug.Log("debug : " + hit.transform.name);
                    StartCoroutine(coFlowerStartParticle(hit));
                    //Vector3 Pos = hit.transform.position;
                    Transform trs = PlaceObjectsOnPlaneLogin.PlaceObjectsOnLogin.spawnedObject.transform.GetChild(5);
                    trs.gameObject.SetActive(false);

                }

            }
        }
    }
    
    public void btnRose()
    {
        isPlant = true;
        GetComponent<PlaceObjectsOnPlaneLogin>().isTouch = false;
        Debug.Log("debug : btnRoseclick");
        plantList.SetActive(false);
        Transform trs = PlaceObjectsOnPlaneLogin.PlaceObjectsOnLogin.spawnedObject.transform.GetChild(5);
        trs.gameObject.SetActive(true);
    }

    IEnumerator coFlowerStartParticle(RaycastHit hit)
    {
        Vector3 Pos = hit.transform.position;
        //flower = Instantiate(FlowerParticle, Pos, Quaternion.identity, hit.transform.parent);//화살표 위치에 프리팹 생성
        ParticleManager.instance.PlayPlaEf("PE1", hit, 1f);
        yield return new WaitForSeconds(1f);
        flowerLogin = Instantiate(rosePrefab, Pos, Quaternion.identity);//터치한 위치에 식물생성
        flowerLogin.transform.rotation = new Quaternion(0f, 180f, 0f, 0);
        btnnext.SetActive(true);
    }
}










