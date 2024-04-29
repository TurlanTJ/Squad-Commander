using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Effect
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
        appliedTo.isStunned = true;
        while(remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        appliedTo.isStunned = false;
        appliedTo.appliedEffects.Remove(this);
        DestroyImmediate(this.gameObject, true);
    }
}
