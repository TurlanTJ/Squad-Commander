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
        playerManager.isGivingCommand = true;

        while(playerManager.isGivingCommand)
        {
            yield return null;

            if(Input.GetMouseButtonDown(0))
                break;

            while (!Input.GetMouseButtonDown(1))
            {

                yield return null;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 
                    unit.GetComponent<Unit>().unitAbility.unitAbility.requiredLayer))
            {
                unit.GetComponent<Unit>().unitAbility.UseAbility(hit.collider.gameObject, hit.point);
            }

            yield return null;
            break;
        }

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
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unit.GetComponent<Unit>().unitData.targetLayerMask))
                attackManager.InitiateAttack(unit, hit.collider.gameObject);
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
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, playerManager.layerGround))
                movementManager.MoveSelectedUnits(unit, hit.point);
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