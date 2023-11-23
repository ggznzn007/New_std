using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Summer_Mng : MonoBehaviour
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
        if (DataManager.Instance.isPlaying)
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
        yield return new WaitForSecondsRealtime(Settings.SceneTime);
        FadeScreen.Instance.OnFade(FadeState.FadeOut);
        DataManager.Instance.isPlaying = false;
        yield return new WaitForSecondsRealtime(Settings.DelayTime);
        SceneManager.LoadScene(2);
    }
}
