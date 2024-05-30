using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityManager : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int segmentCount = 50;
    private bool rangeDrawn = false;

    public AbilitySO unitAbility;
    public GameObject abilityLoc;

    public float remainingCoolDown;
    public bool abilityIsOnCoolDown;

    public delegate void UpdateUIAbilityCD(float cd);
    public UpdateUIAbilityCD onAbilityCDChanged;

    public void DrawAbility()
    {
        lineRenderer.positionCount = segmentCount + 1;
        lineRenderer.useWorldSpace = false;

        float angle = 20f;

        for(var i = 0; i < (segmentCount + 1); i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * unitAbility.abilityRange;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * unitAbility.abilityRange;

            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            angle += (360f / segmentCount);
        }

        lineRenderer.enabled = true;
    }

    public void UnDrawAbilityRange()
    {
        lineRenderer.enabled = false;
    }

    public void DrawAoEArea()
    {
        if(abilityLoc == null)
            abilityLoc = unitAbility.DrawAbility();
    }

    public void UnDrawAoEArea()
    {
        DestroyImmediate(abilityLoc, true);
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
            onAbilityCDChanged?.Invoke(remainingCoolDown/unitAbility.cooldownTime);
            yield return new WaitForSeconds(1f);
            remainingCoolDown -= 1f;
        }
        onAbilityCDChanged?.Invoke(0f);
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
