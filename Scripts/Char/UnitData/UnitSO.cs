using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit/New Unit")]
public class UnitSO : ScriptableObject
{
    public int unitId;
    public string unitName;
    public string unitDescription;
    public int unitHealthMax;
    public int unitEnergyMax;
    public float unitRange;
    public int unitAttackRPM;
    public int unitDamage;
    public float unitArmor;

    public Sprite unitIcon;
    public LayerMask targetLayerMask;
}
