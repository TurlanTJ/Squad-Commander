using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementManager : MonoBehaviour
{
    private UnitManager unitManager;

    private List<GameObject> selectecUnits = new List<GameObject>();

    void Start()
    {
        unitManager = UnitManager.instance;
    }

    public void MoveSelectedUnits(Vector3 destination)
    {
        selectecUnits = unitManager.unitSelectionManager.selectedUnits;

        if(unitManager.unitSelectionManager.ContainsSelectedUnits())
        {
            foreach(GameObject unit in selectecUnits)
                unit.GetComponent<Unit>().Move(destination);
        }
    }

    public void MoveSelectedUnits(GameObject unit, Vector3 destination)
    {
        unit.GetComponent<Unit>().Move(destination);
    }

    public void StopSelectedUnitsMovement()
    {
        selectecUnits = unitManager.unitSelectionManager.selectedUnits;

        if(unitManager.unitSelectionManager.ContainsSelectedUnits())
        {
            foreach(GameObject unit in selectecUnits)
                unit.GetComponent<Unit>().StopAllUnitActions();
        }
    }
}
