using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandManager : MonoBehaviour
{
    public UnitMovementManager movementManager;
    public UnitAttackManager attackManager;  
    public UnitSelectionManager unitSelectionManager;

    private PlayerInputManager inputManager;
    private PlayerManager playerManager;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = PlayerInputManager.instance;
        inputManager.onUnitAbilityActivated += UseUnitAbility;
        inputManager.onUnitAttack += AttackUnitTarget;
        inputManager.onUnitMove += MoveUnit;
        inputManager.onUnitStopAllActions += StopAllUnitActions;

        playerManager = PlayerManager.instance;

        mainCamera = Camera.main;
    }

    public bool CancelCommand()
    {
        if(Input.GetMouseButtonDown(1))
            return true;
        return false;
    }

    public void UseUnitAbility()
    {
        if(unitSelectionManager.focusedUnit == null)
            return;
        StartCoroutine(UseAbility(unitSelectionManager.focusedUnit));
    }

    public void AttackUnitTarget()
    {
        if(unitSelectionManager.focusedUnit == null)
            return;
        StartCoroutine(AttackTarget(unitSelectionManager.focusedUnit));
    }

    public void MoveUnit()
    {
        if(unitSelectionManager.focusedUnit == null)
            return;
        StartCoroutine(Move(unitSelectionManager.focusedUnit));
    }

    public void StopAllUnitActions()
    {
        if(unitSelectionManager.focusedUnit == null)
            return;
        StartCoroutine(StopAllActions(unitSelectionManager.focusedUnit));
    }

    private IEnumerator UseAbility(GameObject unit)
    {
        Unit focusedUnit = unit.GetComponent<Unit>();

        focusedUnit.unitAbilityManager.DrawAbility(); // Drawing the ability Range
        focusedUnit.unitAbilityManager.DrawAoEArea(); // Drawing the ability Location
        GameObject abilityLoc = focusedUnit.unitAbilityManager.abilityLoc;


        playerManager.isGivingCommand = true;

        while(playerManager.isGivingCommand)
        {
            yield return null;

            if(Input.GetMouseButtonDown(0))
                break;


            while (!Input.GetMouseButtonDown(1))
            {
                // Updating the ability location
                Ray newRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(newRay, out RaycastHit newHit, Mathf.Infinity))
                {
                    if(newHit.collider.gameObject.TryGetComponent(out Interactable interactable))
                    {
                        if(interactable.interactableType == InteractableType.Ground && abilityLoc != null)
                        {
                            Vector3 mousePosition = newHit.point;
                            Vector3 dir = mousePosition - unit.transform.position;

                            // checking if the area location is within the ability range
                            if(dir.magnitude > focusedUnit.unitAbilityManager.unitAbility.abilityRange)
                                mousePosition = unit.transform.position + dir.normalized * focusedUnit.unitAbilityManager.unitAbility.abilityRange;

                            abilityLoc.transform.position = mousePosition;
                        }
                    }
                }
                 
                yield return null;
            }


            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                {
                    if(interactable.interactableType == focusedUnit.unitAbilityManager.unitAbility.requiredLayer && abilityLoc != null)
                        focusedUnit.unitAbilityManager.UseAbility(hit.collider.gameObject, abilityLoc.transform.position); // Activating the ability
                }
            }

            yield return null;
            break;
        }
        
        focusedUnit.unitAbilityManager.UnDrawAoEArea();
        focusedUnit.unitAbilityManager.UnDrawAbilityRange();
        playerManager.isGivingCommand = false;
    }

    private IEnumerator AttackTarget(GameObject unit)
    {
        playerManager.isGivingCommand = true;

        while(playerManager.isGivingCommand)
        {
            yield return null;

            if(Input.GetMouseButtonDown(0))
                break;

            while (!Input.GetMouseButtonDown(1))
                yield return null;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                {
                    if(interactable.interactableType == InteractableType.Unit && interactable.gameObject.GetComponent<IUnit>().unitFaction == UnitFaction.Enemy)
                        attackManager.InitiateAttack(unit, hit.collider.gameObject);
                }
            }
            break;
        }

        playerManager.isGivingCommand = false;
    }

    private IEnumerator Move(GameObject unit)
    {
        playerManager.isGivingCommand = true;
        while(playerManager.isGivingCommand)
        {
            yield return null;

            if(Input.GetMouseButtonDown(0))
                break;

            while (!Input.GetMouseButtonDown(1))
                yield return null;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                {
                    if(interactable.interactableType == InteractableType.Ground)
                        movementManager.MoveSelectedUnits(unit, hit.point);
                }
            }
            break;
        }

        playerManager.isGivingCommand = false;
    }

    private IEnumerator StopAllActions(GameObject unit)
    {
        playerManager.isGivingCommand = true;
        while(playerManager.isGivingCommand)
        {
            yield return null;

            movementManager.StopSelectedUnitsMovement();
            break;
        }

        playerManager.isGivingCommand = false;
    }
}