using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : IUnit
{
	public override void TakeDamage(int damage)
	{
		Debug.Log($"{unitData.unitName} recieved {damage}");
	}
}
