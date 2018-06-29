using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour {
	private void Awake()
	{
		DoublesInARow = 0;
	}

	public void RollDice()
	{
		Dice1 = Random.Range(1, 7);
		Dice2 = Random.Range(1, 7);
		if (IsDouble())
		{
			DoublesInARow++;
		}
		else
		{
			DoublesInARow = 0;
		}
	}

	public int Dice1 { get; private set; }

	public int Dice2 { get; private set; }

	public int DoublesInARow { get;  set; }

	public int CurrentRoll()
	{
		return Dice1 + Dice2;
	}

	public bool IsDouble()
	{
		return Dice1 == Dice2;
	}
}
