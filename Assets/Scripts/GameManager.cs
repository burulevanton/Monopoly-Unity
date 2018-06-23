using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityScript.Steps;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{

	public Player[] Players;
	public Player current_player;
	public Field[] Board;
	public GameObject[] Corners; //TODO круговой маршрут
	public Queue<int> ChanceCards;
	public Queue<int> PublicTreasuryCards;
	public DiceRoller DiceRoller;
	public JailManager JailManager;
	public AuctionManager AuctionManager;
	private bool _gameInProgress;

	private enum States
	{
		Default,
		Auction,
		Trade
	};

	private States _state;

	private void Start()
	{
		Players[0].CurrentLocation = Board[0];
		current_player = Players[0];
		ChanceCards = new Queue<int>();
		PublicTreasuryCards = new Queue<int>();
		for (var i = 0; i < 14; i++)
		{
			ChanceCards.Enqueue(i);
			PublicTreasuryCards.Enqueue(i);
		}
		_state = States.Default;
		_gameInProgress = false;
	}

	void Update()
	{
//		if (!_gameInProgress)
//		{
//			_gameInProgress = true;
//			StartCoroutine(Game());
//		}
		if (!_state.Equals(States.Auction))
		{
			StartCoroutine(AuctionManager.StartAuction((Ownable)Board[25],Players[0]));
			_state = States.Auction;
		}
	}

	private IEnumerator Game()
	{
		yield return StartCoroutine(current_player.MoveTo(Board[1]));
		yield return new WaitForSeconds(2);
		yield return StartCoroutine(current_player.MoveTo(Board[3]));
		yield return new WaitForSeconds(2);
		while (_gameInProgress)
		{
			if (current_player.InJail && current_player.CurrentState == Player.State.StartTurn)
				yield return StartCoroutine(JailManager.TurnInJail());
			yield return null;
		}
	}

	public int NextLocation()
	{
		var currentLocation = Array.IndexOf(Board, Players[0].CurrentLocation);
		var nextLocation = currentLocation + DiceRoller.CurrentRoll();
		return nextLocation > 39 ? nextLocation - 40 : nextLocation;
	}
	public IEnumerator RollDice()
	{
		DiceRoller.RollDice();
		yield return StartCoroutine(Players[0].MoveTo(Board[NextLocation()]));
	}
	
	public void BuyProperty(Ownable property)
	{
		current_player.AccountBalance -= property.PurchasePrice;
		current_player.Owned.Add(property);
		property.setOwner(current_player);
		Debug.Log(current_player.AccountBalance);
	}

	public void MortgageProperty(Ownable property)
	{
		current_player.AccountBalance += property.PurchasePrice / 2;
		current_player.Owned.Remove(property);
		current_player.Mortgaged.Add(property);
		property.Mortgage();
		Debug.Log(current_player.AccountBalance);
	}
	public void RedeemProperty(Ownable property)
	{
		current_player.AccountBalance -= (int)(property.PurchasePrice / 2 * 1.1f);
		current_player.Owned.Add(property);
		current_player.Mortgaged.Remove(property);
		property.UnMortgage();
		Debug.Log(current_player.AccountBalance);
	}

	public bool CanPlayerUpgradeAnything()
	{
		var upgradeProperies = current_player.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street)x).ToList()
			.FindAll(x=>x.CanUpgrade());
		return upgradeProperies.Count > 0;
	}

	public void BuyHouses(Street property, int numOfHouses)
	{
		current_player.AccountBalance -= property.HousePrice * numOfHouses;
		property.BuildHouses(numOfHouses);
		Debug.Log(current_player.AccountBalance);
	}

	public bool CanPlayerSellAnything()
	{
		return current_player.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street)x).ToList()
			.FindAll(x=>x.CurrentUpgradeLevel > 0).Count > 0;
	}

	public void SellHouses(Street property, int numOfHouses)
	{
		current_player.AccountBalance += (int) (property.HousePrice * numOfHouses / 2);
		property.SellHouse(numOfHouses);
		Debug.Log(current_player.AccountBalance);
	}
}
