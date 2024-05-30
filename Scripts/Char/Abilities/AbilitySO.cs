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
    public List<UnitFaction> affect;
    public InteractableType requiredLayer;
    public EffectType effect;


    public virtual GameObject DrawAbility(){return new GameObject();}
    public virtual void ActivateAbility(GameObject unit, GameObject target, Vector3 targetPos){}
}
