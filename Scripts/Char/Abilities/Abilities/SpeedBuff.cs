using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Effect
{
    public float remainingTime;
    public float prevSpeed;

    public override void InitializeEffect(IUnit unit, AbilitySO ability, float dur, float mod)
    {
        appliedTo = unit;
        appliedBy = ability;
        duration = dur;
        modifier = mod;

        ApplyEffect();
        StartCoroutine(StartTimer());
    }

    public override void ResetTimer()
    {
        remainingTime = duration;
    }

    private void ApplyEffect()
    {
        prevSpeed = appliedTo.defaultSpeed;
        float currSpeed = prevSpeed * modifier;
        appliedTo.unitAgent.speed = currSpeed;
    }

    private IEnumerator StartTimer()
    {
        remainingTime = duration;
        appliedTo.appliedEffects.Add(this);
        while(remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        appliedTo.unitAgent.speed = prevSpeed;
        appliedTo.appliedEffects.Remove(this);
        DestroyImmediate(this.gameObject, true);
    }
}
