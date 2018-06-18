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
	public Field[] Board;
	public GameObject[] Corners;
	public float speed = 1.0f;
	public float lastWaypointSwitchTime;

	private void Start()
	{
		Players[0].CurrentLocation = Board[0];
	}

	void Update()
	{
		StartCoroutine(PlayGame());
	}

	public IEnumerator PlayGame()
	{
//		for (var i = 0; i < 35; i++)
//		{
//			foreach (var player in Players)
//			{
//				var roll1 = Random.Range(1, 7);
//				var roll2 = Random.Range(1, 7);
//				var currentLocation = Array.IndexOf(Board, player.CurrentLocation);
//				var nextLocation = currentLocation + roll1 + roll2;
//				if (nextLocation > 39)
//				{
//					nextLocation -= 40;
//				}
//				
//				yield return player.MoveTo(Board[nextLocation]);
//				player.CurrentLocation = Board[nextLocation];
//				yield return null;
//			}
//		}
		int count = 0;
		if (count == 0)
		{
			yield return Players[0].MoveTo(Board[25]);
			count += 1;
		}

		if (count == 1)
		{
			yield return Players[0].MoveTo(Board[37]);
			count += 1;
		}
		yield return new WaitForSeconds(15f);
		Debug.Log("Sanya huy sosi");
	}
}
