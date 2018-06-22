using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour {

	private int _dice1;
	private int _dice2;

	public void RollDice()
	{
		_dice1 = Random.Range(1, 7);
		_dice2 = Random.Range(1, 7);
		Debug.Log(string.Format("Вы выбросили {0} и {1}",_dice1, _dice2));
	}

	public int CurrentRoll()
	{
		return _dice1 + _dice2;
	}

	public bool IsDouble()
	{
		return _dice1 == _dice2;
	}
}
