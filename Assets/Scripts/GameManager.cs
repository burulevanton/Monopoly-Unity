using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{

	public Player[] Players;
	public Player current_player;
	public Field[] Board;
	public GameObject[] Corners;
	public float speed = 1.0f;
	public float lastWaypointSwitchTime;

	private void Start()
	{
		Players[0].CurrentLocation = Board[0];
		current_player = Players[0];
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
}
