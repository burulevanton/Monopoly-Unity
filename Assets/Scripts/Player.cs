using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public Field CurrentLocation { get; set; }

	private bool isMoving;

	public bool IsMoving
	{
		get { return isMoving; }
	}

	public List<Ownable> Owned{ get; private set; }
	
	public List<Ownable> Mortgaged { get; private set; }

	public int NumOfTurnsInJail { get; set; }
	
	public bool InJail { get; set; }

	public void Start()
	{
		isMoving = false;
		Owned = new List<Ownable>();
		Mortgaged = new List<Ownable>();
		NumOfTurnsInJail = 0;
		InJail = false;
		CurrentState = State.StartTurn;
	}

	public enum State
	{
		StartTurn,
		Moving,
		OfferToBuyProperty,
		OfferToMortgageProperty,
		OfferToRedeemProperty,
		OfferToBuyHouse,
		OfferToSellHouse,
		InJail,
		EndTurn,
		Idle
	};

	public State CurrentState { get; set; }

	public IEnumerator MoveTo(Field location)
	{
		isMoving = true;
		Vector3 startPosition = CurrentLocation.transform.position;
		Vector3 endPosition = location.transform.position;
		float pathLength = Vector2.Distance(startPosition, endPosition);
		float totalTimeForPath = pathLength / 100f;
		float lastSwitchTime = Time.time;
		while (transform.position != location.transform.position)
		{
			float currentTimeOnPath = Time.time - lastSwitchTime;
			transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
			yield return null;
		}
		this.CurrentLocation = location;
		location.LandOn(this);
		isMoving = false;
	}
}
