using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
        InitializeInputEvents();
    }

    public delegate void OnUnitAbilityActivated();
    public OnUnitAbilityActivated onUnitAbilityActivated;
    public delegate void OnUnitAttack();
    public OnUnitAttack onUnitAttack;
    public delegate void OnUnitMove();
    public OnUnitMove onUnitMove;
    public delegate void OnUnitStopAllActions();
    public OnUnitStopAllActions onUnitStopAllActions;

    public PlayerInputActions inputActions;

    void InitializeInputEvents()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Enable();
        inputActions.Player.UnitUseAbility.performed += UseAbility;
        inputActions.Player.UnitAttack.performed += AttackTarget;
        inputActions.Player.UnitMove.performed += MoveUnit;
        inputActions.Player.UnitStopEverything.performed += StopAllActions;
    }

    public Vector2 GetCameraMovementDirection()
    {
        Vector2 movementVector = inputActions.Player.CameraMovement.ReadValue<Vector2>().normalized;

        return movementVector;
    }

    public void UseAbility(InputAction.CallbackContext context)
    {
        onUnitAbilityActivated?.Invoke();
    }

    public void AttackTarget(InputAction.CallbackContext context)
    {
        onUnitAttack?.Invoke();
    }

    public void MoveUnit(InputAction.CallbackContext context)
    {
        onUnitMove?.Invoke();
    }

    public void StopAllActions(InputAction.CallbackContext context)
    {
        onUnitStopAllActions?.Invoke();
    }
}
