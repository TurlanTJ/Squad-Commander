using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySO : ScriptableObject
{
    public string name;
    public float energyCost;
    public float abilityRange;
    public float activeTime;
    public float cooldownTime;
    public List<LayerMask> affect;
    public LayerMask requiredLayer;
    public EffectType effect;

    public virtual void DrawAbility(){}
    public virtual void ActivateAbility(GameObject unit, GameObject target, Vector3 targetPos){}
}
