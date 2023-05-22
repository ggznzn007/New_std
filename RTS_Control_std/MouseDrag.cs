using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [SerializeField]
    private RectTransform dragRentangle;

    private Rect dragRect;
    private Vector2 start = Vector2.zero;
    private Vector2 end = Vector2.zero;

    private Camera mainCam;
    private RTS_Controller controller;

    private void Awake()
    {
        mainCam = Camera.main;
        controller = GetComponent<RTS_Controller>();

        DrawDragRectangle();
    }

    private void Update()
    {
      if(Input.GetMouseButtonDown(0))
        {
            start = Input.mousePosition;
            dragRect = new Rect();
        }  

      if(Input.GetMouseButton(0))
        {
            end = Input.mousePosition;
            DrawDragRectangle();
        }

      if(Input.GetMouseButtonUp(0))
        {
            CalculateDragRect();
            SelectUnits();
            start = end = Vector2.zero;
            DrawDragRectangle();
        }
    }

    private void DrawDragRectangle()
    {
        dragRentangle.position = (start + end) * 0.5f;
        dragRentangle.sizeDelta = new Vector2(Mathf.Abs(start.x-end.x),Mathf.Abs(start.y-end.y));
    }

    private void CalculateDragRect()
    {
        if(Input.mousePosition.x<start.x)
        {
            dragRect.xMin= Input.mousePosition.x;
            dragRect.xMax = start.x;
        }
        else
        {
            dragRect.xMin = start.x;
            dragRect.xMax = Input.mousePosition.x;
        }

        if(Input.mousePosition.y<start.y)
        {
            dragRect.yMin = Input.mousePosition.y;
            dragRect.yMax = start.y;
        }
        else
        {
            dragRect.yMin= start.y;
            dragRect.yMax= Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (UnitController unit in controller.UnitList)
        {
            if(dragRect.Contains(mainCam.WorldToScreenPoint(unit.transform.position)))
            {
                controller.DragSelectUnit(unit);
            }
        }
    }
}
