using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionManager : MonoBehaviour
{
    [SerializeField] private IUnit unit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == unit.currentTarget)
            unit.StopMovement();

        if(other.gameObject.TryGetComponent(out IUnit unitScript))
        {
            if(unitScript.unitFaction != unit.unitData.targetLayerMask)
                return;

            if(!unit.availableTargets.Contains(unitScript.gameObject))
                unit.availableTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == unit.currentTarget)
        {
            unit.FollowTarget(unit.currentTarget);
            return;
        }

        if(other.gameObject.TryGetComponent(out IUnit unitScript))
        {
            if(unitScript.unitFaction != unit.unitData.targetLayerMask)
                return;
                
            if(unit.availableTargets.Contains(unitScript.gameObject))
                unit.availableTargets.Remove(other.gameObject);
        }
    }
}
