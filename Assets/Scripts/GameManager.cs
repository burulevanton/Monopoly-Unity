using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{

	public Player[] Players;
	public Player current_player;
	public Field[] Board;
	public GameObject[] Corners; //TODO круговой маршрут
	public Queue<int> ChanceCards;
	public Queue<int> PublicTreasuryCards;

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
	}

	void Update()
	{
		
	}

	public IEnumerator RollDice()
	{
		var roll1 = Random.Range(1, 7);
		var roll2 = Random.Range(1, 7);
		var currentLocation = Array.IndexOf(Board, Players[0].CurrentLocation);
		var nextLocation = currentLocation + roll1 + roll2;
		if (nextLocation > 39)
		{
			nextLocation -= 40;
		}
		yield return StartCoroutine(Players[0].MoveTo(Board[nextLocation]));
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
		Debug.Log(current_player.AccountBalance);
	}
	public void RedeemProperty(Ownable property)
	{
		current_player.AccountBalance -= (int)(property.PurchasePrice / 2 * 1.1f);
		current_player.Owned.Add(property);
		current_player.Mortgaged.Remove(property);
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
