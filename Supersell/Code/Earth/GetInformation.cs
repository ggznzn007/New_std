using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Diagnostics;

public class GetInformation : MonoBehaviour
{
    public UnityEngine.UI.Button[] asia;        // 나라정보를 담은 이미지 버튼들
    public GameObject[] nations; // 지도상의 나라들


    public void InfoOn(int number)
    {
        if (!DataManager.DM.isInfoOpen)
        {
            if (DataManager.DM.isInfoOpen) { return; }
            switch (number)
            {
                case 0:                    
                    asia[0].gameObject.transform.LeanScale(new Vector3(3840, 2400, 1), 0.25f).setEaseLinear();                    
                    break;
                case 1:
                    asia[1].gameObject.transform.LeanScale(new Vector3(3840, 2400, 1), 0.25f).setEaseLinear();
                    break;
                case 2:
                    asia[2].gameObject.transform.LeanScale(new Vector3(3840, 2400, 1), 0.25f).setEaseLinear();
                    break;
                case 3:
                    asia[3].gameObject.transform.LeanScale(new Vector3(3840, 2400, 1), 0.25f).setEaseLinear();
                    break;
            }
            DataManager.DM.isInfoOpen = true;
            EarthMovments.EM.isRotate = false;
        }
    }

    public void InfoOff(int number)
    {
        if (DataManager.DM.isInfoOpen)
        {
            if (!DataManager.DM.isInfoOpen) { return; }
            switch (number)
            {
                case 0:
                    asia[0].gameObject.transform.LeanScale(Vector3.zero, 0.25f).setEaseLinear();
                    break;
                case 1:
                    asia[1].gameObject.transform.LeanScale(Vector3.zero, 0.25f).setEaseLinear();
                    break;
                case 2:
                    asia[2].gameObject.transform.LeanScale(Vector3.zero, 0.25f).setEaseLinear();
                    break;
                case 3:
                    asia[3].gameObject.transform.LeanScale(Vector3.zero, 0.25f).setEaseLinear();
                    break;
            }
            DataManager.DM.isInfoOpen = false;
            EarthMovments.EM.isRotate = true;
        }
    }
}
