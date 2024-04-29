using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityManager : MonoBehaviour
{
    public AbilitySO unitAbility;

    public float remainingCoolDown;
    public bool abilityIsOnCoolDown;

    public void DrawAbility()
    {

    }

    public void UseAbility(GameObject target, Vector3 targetPos)
    {
        if(abilityIsOnCoolDown && HasEnoughEnergy())
            return;
        unitAbility.ActivateAbility(gameObject, target, targetPos);
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        abilityIsOnCoolDown = true;
        remainingCoolDown = unitAbility.cooldownTime;
        while(remainingCoolDown > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingCoolDown -= 1f;
        }
        abilityIsOnCoolDown = false;
        remainingCoolDown = unitAbility.cooldownTime;
    }

    private bool HasEnoughEnergy()
    {
        if(gameObject.GetComponent<Unit>().unitEnergyCurrent >= unitAbility.energyCost)
        {
            gameObject.GetComponent<Unit>().unitEnergyCurrent -= (int)unitAbility.energyCost;
            return true;
        }
        return false;
    }
}
