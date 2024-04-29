using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IUnit : MonoBehaviour
{
    public UnitSO unitData;
    public List<Effect> appliedEffects = new List<Effect>();

    public int unitHealthCurrent;
    public int unitEnergyCurrent;

    public bool isAlive;
    public bool isStunned;
    public bool isMoving;
    public float defaultSpeed;
    public float attackDelay;

    public NavMeshAgent unitAgent;
    public UnitStance unitStanceCurrent;

    public GameObject currentlyInteracting;

    public GameObject currentTarget;
    public List<GameObject> availableTargets = new List<GameObject>();

    public delegate void OnUnitMovement(bool status);
    public OnUnitMovement onUnitMovement;
    public delegate void OnUnitAttack(bool status);
    public OnUnitAttack onUnitAttack;
    public delegate void OnUnitHealthChange(int currentHealth);
    public OnUnitHealthChange onUnitHealthChange;
    public delegate void OnUnitEnergyChange(int currentHealth);
    public OnUnitEnergyChange onUnitEnergyChange;
    public delegate void OnUnitDeath();
    public OnUnitDeath onUnitDeath;

    public void StopAllUnitActions()
    {
        if(isMoving)
            StopMovement();
        CancelAttack();
        StopInteraction();
    }

    // Initiate Attack By Assigning Target and Changing Stance Status
    public void InitiateAttack(GameObject target)
    {
        if(isStunned || !isAlive)
            return;

        StopAllUnitActions();
        if(!availableTargets.Contains(target))
            Move(target.transform.position);
        
        currentTarget = target;
        unitStanceCurrent = UnitStance.Combat;
        StartCoroutine(AttackTarget());
        onUnitAttack?.Invoke(true);
    }

    public IEnumerator AttackTarget()
    {
        if(isMoving && availableTargets.Contains(currentTarget))
            StopMovement();
        
        while(unitStanceCurrent == UnitStance.Combat)
        {
            yield return null;

            if(currentTarget == null)
                break;

            if(!availableTargets.Contains(currentTarget))
            {
                FollowTarget(currentTarget);
                onUnitAttack?.Invoke(false);
                yield return null;
                continue;
            }

            Attack();

            if(currentTarget.TryGetComponent(out IUnit unit))
            {
                if(!unit.isAlive)
                {
                    if(availableTargets.Count > 0)
                        currentTarget = availableTargets[0];
                    else
                        CancelAttack();
                }
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    // Attack the Assigned Target
    public void Attack()
    {
        if(isStunned || !isAlive)
            return;

        // Spawn Bullets
        Debug.Log("Fire");

        if(currentTarget.TryGetComponent(out IUnit unitTarget))
            unitTarget.TakeDamage(unitData.unitDamage);

        if(currentTarget.TryGetComponent(out Dummy dummy))
            dummy.TakeDamage(unitData.unitDamage);

        onUnitAttack?.Invoke(true);
    }

    public void CancelAttack()
    {
        currentTarget = null;
        unitStanceCurrent = UnitStance.Idle;
        onUnitAttack?.Invoke(false);
    }

    // Face Towards the Target Location
    public void FaceTarget(GameObject target)
    {
        if(target == null)
            return;
        float rotationSpeed = 10f;
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Take Damage While Considering Damage-Reduction Logic
    public void TakeDamage(int damage)
    {
        int finalDmg = damage;

        if(unitData.unitArmor > 0)
        {
            if(unitData.unitArmor > 1)
                finalDmg = 0;
            else
                finalDmg = Mathf.RoundToInt(damage * (1 - unitData.unitArmor)/2);
        }
        unitStanceCurrent = UnitStance.Combat;
        UpdateCurrentHealth(-finalDmg);
    }

    // Update Unit's Current Health
    public void UpdateCurrentHealth(int modifier)
    {
        if(!isAlive)
            return;

        unitHealthCurrent += modifier;

        if(unitHealthCurrent <= 0 && isAlive)
        {
            unitHealthCurrent = 0;
            Die();
        }

        if(unitHealthCurrent >= unitData.unitHealthMax)
            unitHealthCurrent = unitData.unitHealthMax;

        onUnitHealthChange?.Invoke(unitHealthCurrent);
    }

    // Move Unit by Assinging Destingation To the NavMeshAgent
    public void Move(Vector3 location)
    {
        if(isStunned || !isAlive)
            return;

        StopAllUnitActions();
        unitAgent.isStopped = false;
        unitAgent.SetDestination(location);
        isMoving = true;
        onUnitMovement?.Invoke(isMoving);
    }

    public void FollowTarget(GameObject target)
    {
        if(isStunned || !isAlive)
            return;

        unitAgent.isStopped = false;
        unitAgent.SetDestination(target.transform.position);
        isMoving = true;
        onUnitMovement?.Invoke(isMoving);
    }

    // Stop The Movement of the NavMeshAgent
    public void StopMovement()
    {
        isMoving = false;
        unitAgent.isStopped = true;
        onUnitMovement?.Invoke(isMoving);
    }

    public void Interact(GameObject interactable)
    {
        if(isStunned || !isAlive)
            return;

        currentlyInteracting = interactable;
        unitStanceCurrent = UnitStance.Busy;
    }


    public void StopInteraction()
    {
        currentlyInteracting = null;
        unitStanceCurrent = UnitStance.Idle;
    }

    // Dealth Related Logic
    public void Die()
    {
        // Death-related functions

        onUnitDeath?.Invoke();
        isAlive = false;

        gameObject.SetActive(false);
    }
}

public enum UnitStance
{
    Idle,
    Combat,
    Busy
}