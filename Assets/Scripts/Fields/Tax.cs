using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tax : Field
{

	[SerializeField] private int _taxCost;

	public override void LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
		player.BalanceManager.GetMoneyFromPlayer(_taxCost);
	}
}
