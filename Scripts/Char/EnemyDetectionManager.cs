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
        if(unit.unitData.targetLayerMask == (unit.unitData.targetLayerMask | (1 << other.gameObject.layer)))
            unit.availableTargets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == unit.currentTarget)
            unit.FollowTarget(unit.currentTarget);

        if(unit.unitData.targetLayerMask == (unit.unitData.targetLayerMask | (1 << other.gameObject.layer)))
            unit.availableTargets.Remove(other.gameObject);
    }
}
