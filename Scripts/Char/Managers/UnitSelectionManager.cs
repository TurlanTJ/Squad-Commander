using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    public List<GameObject> spawnedUnits = new List<GameObject>();

    public List<GameObject> selectedUnits = new List<GameObject>();
    public GameObject focusedUnit = null;

    private PlayerInputManager inputManager;

    void Start()
    {
        inputManager = PlayerInputManager.instance;

        inputManager.inputActions.Player.FocusOnUnit1.performed += FocusOnFirstUnit;
        inputManager.inputActions.Player.FocusOnUnit2.performed += FocusOnSecondUnit;
        inputManager.inputActions.Player.FocusOnUnit3.performed += FocusOnThirdUnit;
        inputManager.inputActions.Player.FocusOnUnit4.performed += FocusOnFourthUnit;
    }

    public bool ContainsSelectedUnits()
    {
        if(selectedUnits.Count > 0)
            return true;

        return false;
    }

    public void SelectUnit(GameObject unit, bool shiftSelect)
    {
        if(selectedUnits.Contains(unit))
            return;
        if(!shiftSelect)
            DeselectUnit();

        if(!selectedUnits.Contains(unit))
            selectedUnits.Add(unit);
        else
            FocusOnUnit(unit);

        unit.GetComponent<Unit>().SetUnitSelectionStatus(true);
        if(focusedUnit != null)
            focusedUnit.GetComponent<Unit>().SetUnitFocusStatus(false);
        FocusOnUnit(unit);
    }

    public void DeselectUnit()
    {
        foreach(GameObject unit in selectedUnits)
        {
            unit.GetComponent<Unit>().SetUnitSelectionStatus(false);
            if(unit.GetComponent<Unit>().isFocused)
                unit.GetComponent<Unit>().SetUnitFocusStatus(false);
        }

        if(focusedUnit != null)
        {
            focusedUnit.GetComponent<Unit>().unitAbilityManager.UnDrawAoEArea();
            focusedUnit.GetComponent<Unit>().unitAbilityManager.UnDrawAbilityRange();
        }
        focusedUnit = null;
        selectedUnits.Clear();
        PlayerManager.instance.isGivingCommand = false;
    }

    public void DeselectUnit(GameObject unit)
    {
        if(!selectedUnits.Contains(unit))
            return;
        
        if(unit.GetComponent<Unit>().isFocused)
            unit.GetComponent<Unit>().SetUnitFocusStatus(false);

        focusedUnit = null;
        unit.GetComponent<Unit>().SetUnitSelectionStatus(false);
        selectedUnits.Remove(unit);
    }

    public void FocusOnUnit(GameObject unit)
    {
        unit.GetComponent<Unit>().SetUnitFocusStatus(true);
        focusedUnit = unit;
    }

    public void FocusOnFirstUnit(InputAction.CallbackContext context)
    {
        if(spawnedUnits[0] != null)
            FocusOnUnit(spawnedUnits[0].GetComponent<Unit>().unitOrder);
    }

    public void FocusOnSecondUnit(InputAction.CallbackContext context)
    {
        if(spawnedUnits[1] != null)
            FocusOnUnit(spawnedUnits[1].GetComponent<Unit>().unitOrder);
    }

    public void FocusOnThirdUnit(InputAction.CallbackContext context)
    {
        if(spawnedUnits[2] != null)
            FocusOnUnit(spawnedUnits[2].GetComponent<Unit>().unitOrder);
    }

    public void FocusOnFourthUnit(InputAction.CallbackContext context)
    {
        if(spawnedUnits[3] != null)
            FocusOnUnit(spawnedUnits[3].GetComponent<Unit>().unitOrder);
    }

    public void FocusOnUnit(int unitOrder)
    {
        foreach(GameObject unit in spawnedUnits)
        {
            if(unit.GetComponent<Unit>().isFocused)
                unit.GetComponent<Unit>().SetUnitFocusStatus(false);
        }

        if(!selectedUnits.Contains(spawnedUnits[unitOrder - 1]))
        {
            DeselectUnit();
            SelectUnit(spawnedUnits[unitOrder - 1], false);
        }
        else 
        {
            spawnedUnits[unitOrder - 1].GetComponent<Unit>().SetUnitFocusStatus(true);
            focusedUnit = spawnedUnits[unitOrder - 1];
        }
    }

}
