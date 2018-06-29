using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityScript.Lang;
using Array = System.Array;

public class Player : MonoBehaviour
{
	public string PlayerName { get; set; }
	public Color Color;
	public float YOffset;
	public float XOffset;

//	private int _accountBalance = 1500;

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

	public void Awake()
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
		Idle,
		Bankrupt
	};

	public State CurrentState { get; set; }

	public IEnumerator MoveTo(Field location, bool canGiveMoney=true)
	{
		//todo добавить анимацию мб
		CurrentState = State.Moving;
		var startLocation = CurrentLocation;
		while (startLocation != location)
		{
			Vector3 startPosition = startLocation.transform.position + new Vector3(XOffset, YOffset,0);
			Vector3 endPosition = startLocation.NextField.transform.position + new Vector3(XOffset, YOffset, 0);
			Vector3 newDirection = (endPosition - startPosition);
			float x = newDirection.x;
			float y = newDirection.y;
			float rotationAngle = Mathf.Atan2 (y, x) * 180 / Mathf.PI;
			transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
			float pathLength = Vector2.Distance(startPosition, endPosition);
			float totalTimeForPath = pathLength / 10f;
			float lastSwitchTime = Time.time;
			while (transform.position != startLocation.NextField.transform.position + new Vector3(XOffset, YOffset, 0))
			{
				float currentTimeOnPath = Time.time - lastSwitchTime;
				transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
				yield return null;
			}

			if (startLocation.NextField.GetType() == typeof(Go) && canGiveMoney)
			{
				BalanceManager.GiveMoneyToPlayer(200);
			}
			startLocation = startLocation.NextField;
			yield return null;
		}
		this.CurrentLocation = location;
		location.LandOn(this);
		CurrentState = State.Idle;
	}
}
