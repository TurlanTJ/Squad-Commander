using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaC Ability", menuName = "Ability/Point and Click")]
public class PointAndClickAbilitySO : AbilitySO
{
    public override GameObject DrawAbility()
    {
        return new GameObject();
    }
    
    public override void ActivateAbility(GameObject unit, GameObject target, Vector3 targetPos)
    {
        
    }
}
