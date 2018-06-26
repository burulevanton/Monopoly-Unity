using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string PlayerName { get; set; }

	private int _accountBalance = 1500;

//	public int AccountBalance
//	{
//		get { return _accountBalance; }
//		set
//		{
//			_accountBalance = value;
//			//TODO добавить что-то ещё
//		}
//	}

	public BalanceManager BalanceManager;
	
	public Field CurrentLocation { get; set; }

//	private bool isMoving;
//
//	public bool IsMoving
//	{
//		get { return isMoving; }
//	}

	public List<Ownable> Owned{ get; private set; }
	
	public List<Ownable> Mortgaged { get; private set; }

	public int NumOfTurnsInJail { get; set; }
	
	public bool InJail { get; set; }

	public void Start()
	{
		Owned = new List<Ownable>();
		Mortgaged = new List<Ownable>();
		NumOfTurnsInJail = 0;
		InJail = false;
		CurrentState = State.StartTurn;
		PlayerName = string.Format("{0}",Random.Range(1, 100));
		BalanceManager = gameObject.AddComponent<BalanceManager>();
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
		CurrentState = State.Moving;
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
		CurrentState = State.Idle;
	}
}
