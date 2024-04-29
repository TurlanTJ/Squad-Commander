using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackManager : MonoBehaviour
{
    private UnitManager unitManager;

    private List<GameObject> selectecUnits = new List<GameObject>();

    void Start()
    {
        unitManager = UnitManager.instance;
    }

    public void InitiateAttack(GameObject target)
    {
        selectecUnits = unitManager.unitSelectionManager.selectedUnits;

        if(unitManager.unitSelectionManager.ContainsSelectedUnits())
        {
            foreach(GameObject unit in selectecUnits)
                unit.GetComponent<Unit>().InitiateAttack(target);
        }
    }

    public void InitiateAttack(GameObject unit, GameObject target)
    {
        unit.GetComponent<Unit>().InitiateAttack(target);
    }

    public void CancelAttack()
    {
        selectecUnits = unitManager.unitSelectionManager.selectedUnits;

        if(unitManager.unitSelectionManager.ContainsSelectedUnits())
        {
            foreach(GameObject unit in selectecUnits)
                unit.GetComponent<Unit>();
        }
    }
}
