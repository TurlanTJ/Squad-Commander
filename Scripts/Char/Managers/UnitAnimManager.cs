using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UnitAnimManager : MonoBehaviour
{
    [SerializeField] private Animator _unitAnimator;
    [SerializeField] private Unit _unit;

    private string _isAttacking = "IsAttacking";
    private string _isMoving = "IsMoving";

    // Start is called before the first frame update
    void Start()
    {
        _unit.onUnitMovement += UpdateMovementStatus;
        _unit.onUnitAttack += UpdateUnitCombatStatus;
    }

    private void UpdateMovementStatus(bool status)
    {
        _unitAnimator.SetBool(_isMoving, status);
    }

    private void UpdateUnitCombatStatus(bool status)
    {
        _unitAnimator.SetBool(_isAttacking, status);
    }
}
