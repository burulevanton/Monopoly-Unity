using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : Ownable
{

	[SerializeField] private Utility[] _otherUtilities;
	private GameManager _gameManager;
	
	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	protected override int Rent()
	{
		var multiplier = _otherUtilities[0].Owner == this.Owner && !_otherUtilities[0].IsMortgage ? 10 : 4;
		return multiplier * _gameManager.DiceRoller.CurrentRoll();
	}
}
