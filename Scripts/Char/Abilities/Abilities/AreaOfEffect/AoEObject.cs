using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEObject : MonoBehaviour
{
    private float AoERadius;
    private float AoEDuration;
    private float AoESphereGrowthRate;
    private float AoEModifier;
    private float AoEEffecDuration;

    private bool AoEActive = false;

    private EffectType AoEType;
    private List<LayerMask> affectLayers;
    private List<IUnit> unitsInArea = new List<IUnit>();
    private AbilitySO AoESpawner;

    public void Initialize(AbilitySO spawner, float areaRad, float growthRate, float duration,
         float modifier, float modifierDuration, EffectType effect, List<LayerMask>  affect)
    {
        AoERadius = areaRad;
        AoEDuration = duration;
        AoESphereGrowthRate = growthRate;
        AoEModifier = modifier;
        AoEEffecDuration = modifierDuration;
        AoEType = effect;
        affectLayers = affect;
        AoESpawner = spawner;

        StartCoroutine(UpdateSphere());
        StartCoroutine(StartAbility());
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(LayerMask affectLayer in affectLayers)
        {
            if(affectLayer== (affectLayer | (1 << other.gameObject.layer)))
                unitsInArea.Add(other.gameObject.GetComponent<IUnit>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach(LayerMask affectLayer in affectLayers)
        {
            if(affectLayer== (affectLayer | (1 << other.gameObject.layer)))
                unitsInArea.Remove(other.gameObject.GetComponent<IUnit>());
        }
    }

    private IEnumerator UpdateSphere()
    {
        float currentRadius = 0f;

        while (currentRadius < AoERadius)
        {
            // Calculate the new scale based on growth rate and time
            currentRadius += AoESphereGrowthRate * Time.deltaTime;
            currentRadius = Mathf.Min(currentRadius, AoERadius);

            // Set the scale of the sphere
            transform.localScale = new Vector3(currentRadius, currentRadius, currentRadius);

            yield return null;
        }
    }

    private IEnumerator StartAbility()
    {
        float remainingTime = AoEDuration;
        StartCoroutine(ApplyEffect())
        
        while(remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        DestroyImmediate(this.gameObject, true);
    }

    private IEnumerator ApplyEffect()
    {
	AoEActive = true;
        while(AoEActive)
        {
            switch(AoEType)
            {
                case(EffectType.Heal):
                    Heal();
                    break;
                case(EffectType.HealOverTime):
                    HealOverTime();
                    break;
                case(EffectType.Damage):
                    Damage();
                    break;
                case(EffectType.DamageOverTime):
                    DamageOverTime();
                    break;
                case(EffectType.SpeedBuff):
                    SpeedBuff();
                break;
                case(EffectType.Stun):
                    Stun();
                    break;
                default:
                    break;
            }

            yield return null;
        }
    }

    public void Heal()
    {
        // Heal Logic
    }

    public void HealOverTime()
    {
        // Heal Logic
        if(unitsInArea.Count <= 0)
            return;

        foreach(IUnit unit in unitsInArea)
        {
            GameObject effect = new GameObject("Heal " + UnityEngine.Random.Range(0, 1000), typeof(HealOverTime));
            effect.transform.SetParent(unit.gameObject.transform);
            HealOverTime buff = effect.GetComponent<HealOverTime>();

            if(unit.appliedEffects.Count <= 0)
                buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
            else
            {
                int effectIdx = -1;
                for(var i = 0; i < unit.appliedEffects.Count; i++)
                {
                    if(unit.appliedEffects[i].appliedBy == AoESpawner)
                    {
                        unit.appliedEffects[i].ResetTimer();
                        effectIdx = i;
                    }                       
                }
                if(effectIdx == -1)
                    buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
                else
                    DestroyImmediate(effect, true);
            }
        }
    }

    public void Damage()
    {
        // Damage Logic
    }

    public void DamageOverTime()
    {
        // Damage Logic

        if(unitsInArea.Count <= 0)
            return;

        foreach(IUnit unit in unitsInArea)
        {
            GameObject effect = new GameObject("Damage " + UnityEngine.Random.Range(0, 1000), typeof(DamageOverTime));
            effect.transform.SetParent(unit.gameObject.transform);
            DamageOverTime buff = effect.GetComponent<DamageOverTime>();

            if(unit.appliedEffects.Count <= 0)
                buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
            else
            {
                int effectIdx = -1;
                for(var i = 0; i < unit.appliedEffects.Count; i++)
                {
                    if(unit.appliedEffects[i].appliedBy == AoESpawner)
                    {
                        unit.appliedEffects[i].ResetTimer();
                        effectIdx = i;
                    }                       
                }
                if(effectIdx == -1)
                    buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
                else
                    DestroyImmediate(effect, true);
            }
        }
    }

    public void SpeedBuff()
    {
        // Speef Buff Logic
        if(unitsInArea.Count <= 0)
            return;

        foreach(IUnit unit in unitsInArea)
        {
            GameObject effect = new GameObject("SpeedBuff " + UnityEngine.Random.Range(0, 1000), typeof(SpeedBuff));
            effect.transform.SetParent(unit.gameObject.transform);
            SpeedBuff buff = effect.GetComponent<SpeedBuff>();

            if(unit.appliedEffects.Count <= 0)
                buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
            else
            {
                int effectIdx = -1;
                for(var i = 0; i < unit.appliedEffects.Count; i++)
                {
                    if(unit.appliedEffects[i].appliedBy == AoESpawner)
                    {
                        unit.appliedEffects[i].ResetTimer();
                        effectIdx = i;
                    }                       
                }
                if(effectIdx == -1)
                    buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
                else
                    DestroyImmediate(effect, true);
            }
        }
    }

    public void Stun()
    {
        // Stun Logic

        if(unitsInArea.Count <= 0)
            return;

        foreach(IUnit unit in unitsInArea)
        {
            GameObject effect = new GameObject("SpeedBuff " + UnityEngine.Random.Range(0, 1000), typeof(Stun));
            effect.transform.SetParent(unit.gameObject.transform);
            Stun buff = effect.GetComponent<Stun>();

            if(unit.appliedEffects.Count <= 0)
                buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
            else
            {
                for(var i = 0; i < unit.appliedEffects.Count; i++)
                {
                    if(unit.appliedEffects[i].appliedBy == AoESpawner)
                        return;
                }
                buff.InitializeEffect(unit, AoESpawner, AoEEffecDuration, AoEModifier);
            }
        }
    }
}

public enum EffectType
{
    SpeedBuff,
    Heal,
    HealOverTime,
    Damage,
    DamageOverTime,
    Stun
}
