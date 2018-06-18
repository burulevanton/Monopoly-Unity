using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
	public string PlayerName { get; set; }

	private int _accountBalance = 1500;

	public int AccountBalance
	{
		get { return _accountBalance; }
		set
		{
			_accountBalance = value;
			//TODO добавить что-то ещё
		}
	}

	private Field _currentLocation;

	public void MoveTo(Field location)
	{
		transform.DOMove(location.transform.position, 2f);
		location.LandOn(this);
	}
}
