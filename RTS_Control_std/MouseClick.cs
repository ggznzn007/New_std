using UnityEngine;

public class MouseClick : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerUnit;
    [SerializeField]
    private LayerMask layerGround;

    private Camera mainCam;
    private RTS_Controller controller;

    private void Awake()
    {
        mainCam = Camera.main;
        controller = GetComponent< RTS_Controller>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity,layerUnit)) 
            {
                if (hit.transform.GetComponent<UnitController>() == null) return;

                if(Input.GetKey(KeyCode.LeftShift)) // Shift 키를 누르고 클릭 시
                {
                    controller.ShiftClickSelectUnit(hit.transform.GetComponent<UnitController>());
                }
                else  // 일반 클릭 시
                {
                    controller.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
                }
            }

            else
            {
                if(!Input.GetKey(KeyCode.LeftShift))
                {
                    controller.DeselectAll();
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out hit, Mathf.Infinity,layerGround))
            {
                controller.MoveSelectedUnit(hit.point);
            }
        }
    }
}
