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


    // ���콺 Ŭ������ ���� ���ý� ȣ��
    public void ClickSelectUnit(UnitController newUnit)
    {
        DeselectAll(); // ���� ���� ���� ��� ����

        SelectUnit(newUnit);
    }

    public void ShiftClickSelectUnit(UnitController newUnit)
    {
        if(selUnitList.Contains(newUnit))  // ���� �������� ���� ��
        {
            DeslectUnit(newUnit);
        }
        else
        {
            SelectUnit(newUnit);           // ���ο� ���� ���� ��
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
