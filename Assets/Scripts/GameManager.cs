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
		if (!Players[0].IsMoving)
		{
			var roll1 = Random.Range(1, 7);
			var roll2 = Random.Range(1, 7);
			var currentLocation = Array.IndexOf(Board, Players[0].CurrentLocation);
			var nextLocation = currentLocation + roll1 + roll2;
			if (nextLocation > 39)
			{
				nextLocation -= 40;
			}
			StartCoroutine(Players[0].MoveTo(Board[nextLocation]));
		}
	}

	public IEnumerator PlayGame()
	{
		var i = 0;
		while (i < 35)
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
				Debug.Log(string.Format("i={0}",i));
				Debug.Log(string.Format("roll1={0}",roll1));
				Debug.Log(string.Format("roll2={0}",roll2));
			i++;
			yield return null;
		}
	}
}
