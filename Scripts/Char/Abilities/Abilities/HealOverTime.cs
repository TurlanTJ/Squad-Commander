using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : Effect
{
    public float remainingTime;

    public override void InitializeEffect(IUnit unit, AbilitySO ability, float dur, float mod)
    {
        appliedTo = unit;
        appliedBy = ability;
        duration = dur;
        modifier = mod;

        StartCoroutine(ApplyEffect());
    }

    public override void ResetTimer()
    {
        remainingTime = duration;
    }

    private IEnumerator ApplyEffect()
    {
        remainingTime = duration;
        appliedTo.appliedEffects.Add(this);
        while(remainingTime > 0)
        {
            appliedTo.UpdateCurrentHealth((int)modifier);

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        appliedTo.appliedEffects.Remove(this);
        DestroyImmediate(this.gameObject, true);
    }
}
