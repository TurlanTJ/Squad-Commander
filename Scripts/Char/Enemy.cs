using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IUnit
{
    void Awake()
    {
        isAlive = true;
        unitHealthCurrent = unitData.unitHealthMax;
        unitEnergyCurrent = unitData.unitEnergyMax;

        attackDelay = 60f / unitData.unitAttackRPM;
        defaultSpeed = unitAgent.speed;
    }

    void Update()
    {
        if(isStunned)
            StopAllUnitActions();

        if(currentTarget == null || !currentTarget.GetComponent<IUnit>().isAlive)
        {
            availableTargets.Remove(currentTarget);
            SelectNewTarget();
        }

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

    private void SelectNewTarget()
    {
        if(availableTargets.Count > 0)
        {
            currentTarget = availableTargets[0];
            InitiateAttack(currentTarget);
        }
        else
            unitStanceCurrent = UnitStance.Idle;
    }
}
