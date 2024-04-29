using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public string effectName;
    public float duration;
    public float modifier;

    public Sprite effectIcon;
    public IUnit appliedTo;
    public AbilitySO appliedBy;

    public virtual void InitializeEffect(IUnit unit, AbilitySO ability, float duration, float modifier){}
    public virtual bool CheckIfEffectAlreadyApplied(Unit unit, AbilitySO ability){ return true; }
    public virtual void ResetTimer(){}
}
