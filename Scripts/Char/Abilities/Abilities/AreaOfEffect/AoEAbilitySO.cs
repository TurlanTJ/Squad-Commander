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

    public override GameObject DrawAbility()
    {
        GameObject abilityLoc = new GameObject();
        LineRenderer abilityRenderer = abilityLoc.AddComponent<LineRenderer>();
        abilityRenderer.startWidth = 0.1f;
        abilityRenderer.endWidth = 0.1f;

        abilityRenderer.startColor = Color.red;
        abilityRenderer.endColor = Color.red;
        int segmentCount = 50;
        
        abilityRenderer.positionCount = segmentCount + 1;
        abilityRenderer.useWorldSpace = false;

        float angle = 20f;

        for(var i = 0; i < (segmentCount + 1); i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * areaRadius/2;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * areaRadius/2;

            abilityRenderer.SetPosition(i, new Vector3(x, 0, z));
            angle += (360f / segmentCount);
        }

        return abilityLoc;
    }
    
    public override void ActivateAbility(GameObject unit, GameObject target, Vector3 targetPos)
    {
        GameObject spawnedBuffArea = Instantiate(buffAreaSpawner, targetPos, Quaternion.identity);
        spawnedBuffArea.GetComponent<AoEObject>().
            Initialize(this, areaRadius, areaGrowthRate, abilityDuration, modifier, modifierDuration, effect, affect);
    }
}
