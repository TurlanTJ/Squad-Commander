using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Directional Ability", menuName = "Ability/Directional")]
public class DirectionalAbilitySO : AbilitySO
{
    public override GameObject DrawAbility()
    {
        return new GameObject();
    }

    public override void ActivateAbility(GameObject unit, GameObject target, Vector3 targetPos)
    {
        
    }
}
