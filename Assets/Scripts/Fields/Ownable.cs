using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Ownable : Field
{

	protected Player Owner;

	[SerializeField] private int _purchasePrice;
	private bool _isMortgage;

	public override IEnumerator LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
		yield return null;
	}

	protected abstract int Rent();
}
