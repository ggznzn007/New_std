using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall_Mng : MonoBehaviour
{
    [SerializeField] GameObject hiddenFoot;
    private Vector3 m_Offset;
    private float m_ZCoord;
    private GameObject myFoot;

    void Start()
    {        
        StartCoroutine(TimeToFade());
    }

    
    void Update()
    {
        if (DataManager.Instance.isPlaying && !DataManager.Instance.isPaused)
        {
            if (!DataManager.Instance.isPlaying) return;

            if (Input.GetMouseButtonDown(0))                                     // ���콺 Ŭ���� �ѹ��� ȣ��
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    Vector3 hitPos = hit.point;
                    myFoot = Instantiate(hiddenFoot, hitPos, hiddenFoot.transform.rotation);
                    m_ZCoord = Camera.main.WorldToScreenPoint(myFoot.transform.position).z;
                    m_Offset = myFoot.transform.position - GetMouseWorldPosition();
                }
            }

            if (Input.GetMouseButton(0))                                         // ���콺 Ŭ����(�巡��) ��� ȣ��
            {
                myFoot.transform.position = GetMouseWorldPosition() + m_Offset;
            }

            if (Input.GetMouseButtonUp(0))                                       // ���콺 Ŭ������ �� �ѹ��� ȣ��
            {
                Destroy(myFoot);
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(3);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(4);
        }
    }

    Vector3 GetMouseWorldPosition()                                        // ���콺 ��ġ�� �Լ�
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    IEnumerator TimeToFade()
    {
        FadeScreen.Instance.OnFade(FadeState.FadeIn);
        DataManager.Instance.isPlaying = true;
        yield return new WaitForSecondsRealtime(DataManager.Instance.SceneTime_Fall);
        FadeScreen.Instance.OnFade(FadeState.FadeOut);
        DataManager.Instance.isPlaying = false;
        yield return new WaitForSecondsRealtime(DataManager.Instance.DelayTime);
        SceneManager.LoadScene(3);
    }
}
