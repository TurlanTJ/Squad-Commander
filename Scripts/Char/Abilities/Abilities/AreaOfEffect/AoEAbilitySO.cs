using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AoE Ability", menuName = "Ability/Area of Effect")]
public class AoEAbilitySO : AbilitySO
{
    public float areaRadius;
    public float areaGrowthRate;
    public float abilityDuration;
    public float modifier;
    public float modifierDuration;

    public GameObject buffAreaSpawner;

    public override void DrawAbility()
    {

    }
    
    public override void ActivateAbility(GameObject unit, GameObject target, Vector3 targetPos)
    {
        GameObject spawnedBuffArea = Instantiate(buffAreaSpawner, targetPos, Quaternion.identity);
        spawnedBuffArea.GetComponent<AoEObject>().
            Initialize(this, areaRadius, areaGrowthRate, abilityDuration, modifier, modifierDuration, effect, affect);
    }
}
