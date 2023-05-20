using System.Collections.Generic;
using UnityEngine;

public class RTS_Controller : MonoBehaviour
{
    [SerializeField]
    private UnitSpawner spawner;
    private List<UnitController> selUnitList;
    public List<UnitController> UnitList { private set; get; }

    private void Awake()
    {
        selUnitList = new List<UnitController>();
        UnitList = spawner.SpawnUnits();
    }


    // 마우스 클릭으로 유닛 선택시 호출
    public void ClickSelectUnit(UnitController newUnit)
    {
        DeselectAll(); // 기존 선택 유닛 모두 해제

        SelectUnit(newUnit);
    }

    public void ShiftClickSelectUnit(UnitController newUnit)
    {
        if(selUnitList.Contains(newUnit))  // 기존 선택유닛 선택 시
        {
            DeslectUnit(newUnit);
        }
        else
        {
            SelectUnit(newUnit);           // 새로운 유닛 선택 시
        }
    }

    public void DragSelectUnit(UnitController newUnit)
    {
        if(!selUnitList.Contains(newUnit))
        {
            SelectUnit(newUnit);
        }
    }

    public void MoveSelectedUnit(Vector3 end)
    {
        for (int i = 0; i < selUnitList.Count; ++i)
        {
            selUnitList[i].MoveTo(end);
        }
    }

    public void DeselectAll()
    {
        for (int i = 0; i < selUnitList.Count; i++)
        {
            selUnitList[i].DeslectUnit();
        }

        selUnitList.Clear();
    }


    private void SelectUnit(UnitController newUnit)
    {
        newUnit.SelectUnit();

        selUnitList.Add(newUnit);
    }

    private void DeslectUnit(UnitController newUnit)
    {
        newUnit.DeslectUnit();

        selUnitList.Remove(newUnit);
    }
}
