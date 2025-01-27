using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingLine : UI_Scene
{
    private List<UI_WaitingUnit> _waitingUnitList = new List<UI_WaitingUnit>();
    private Transform _grid;

    public void Start()
    {
        _grid = Util.FindChild(gameObject, "Grid", true).transform;
    }

    public void AddUnit(BattleUnit addUnit)
    {
        UI_WaitingUnit newUnit = GameManager.Resource.Instantiate("UI/Sub/WaitingUnit", _grid).GetComponent<UI_WaitingUnit>();
        newUnit.SetUnit(addUnit);
        _waitingUnitList.Add(newUnit);
    }

    public void RemoveUnit(BattleUnit removeUnit)
    {
        for(int i=0; i<_waitingUnitList.Count; i++)
            if(_waitingUnitList[i].GetUnit() == removeUnit)
                DestroyIcon(_waitingUnitList[i]);
    }

    public void SetWaitingLine(List<BattleUnit> orderList)
    {
        ClearLine();

        for (int i = 0; i < orderList.Count; i++)
            AddUnit(orderList[i]);
    }

    private void ClearLine()
    {
        for (int i = _waitingUnitList.Count - 1; i >= 0; i--)
            DestroyIcon(_waitingUnitList[i]);
    }

    public void DestroyIcon(UI_WaitingUnit unit)
    {
        _waitingUnitList.Remove(unit);
        Destroy(unit.gameObject);
    }
}
