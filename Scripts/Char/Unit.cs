using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Unit : IUnit
{
    public UnitAbilityManager unitAbility;

    public int unitOrder;
    public bool isSelected = false;
    public bool isFocused = false;

    public GameObject unitSelectionHightlight;
    public GameObject unitFocusHightlight;

    public delegate void OnUnitSelection(bool status);
    public OnUnitSelection onUnitSelection;
    public delegate void OnUnitFocused(bool status);
    public OnUnitFocused onUnitFocused;

    void Awake()
    {
        isAlive = true;
        unitHealthCurrent = unitData.unitHealthMax;
        unitEnergyCurrent = unitData.unitEnergyMax;

        attackDelay = 60f / unitData.unitAttackRPM;
        defaultSpeed = unitAgent.speed;
        SetUnitSelectionStatus(false);
        SetUnitFocusStatus(false);
    }

    void Update()
    {
        // Movement Check
        if(!unitAgent.pathPending)
        {
            if(unitAgent.remainingDistance <= unitAgent.stoppingDistance)
            {
                if(!unitAgent.hasPath || unitAgent.velocity.magnitude == 0f)
                    StopMovement();
            }
        }

        if(unitStanceCurrent == UnitStance.Combat)
            FaceTarget(currentTarget);
    }

    // Select Unit
    public void SetUnitSelectionStatus(bool status)
    {
        isSelected = status;
        unitSelectionHightlight.SetActive(status);
        onUnitSelection?.Invoke(status);
    }

    // Focus On Unit
    public void SetUnitFocusStatus(bool status)
    {
        isFocused = status;
        unitFocusHightlight.SetActive(status);
        onUnitFocused?.Invoke(status);
    }

    // Assign Order to Access Unit Through Shortkeys
    public void SetUnitOrder(int order)
    {
        unitOrder = order;
    }

    public Vector2Int GetHealth()
    {
        return new Vector2Int(unitHealthCurrent, unitData.unitHealthMax);
    }

    public Vector2Int GetEnergy()
    {
        return new Vector2Int(unitEnergyCurrent, unitData.unitEnergyMax);
    }
}
