using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : Ownable
{

	[SerializeField] private Utility[] _otherUtilities;

	public override int Rent()
	{
		var multiplier = _otherUtilities[0].Owner == this.Owner && !_otherUtilities[0].IsMortgage ? 10 : 4;
		return multiplier * GameManager.DiceRoller.CurrentRoll();
	}
}
