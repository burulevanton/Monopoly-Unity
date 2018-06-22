using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CommunityChest : Field {

	private GameManager _gameManager;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	public override void LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
		var numCard = _gameManager.ChanceCards.Dequeue();
		_gameManager.ChanceCards.Enqueue(numCard);
		switch (numCard)
		{
			case 0:
				_gameManager.JailManager.PutPLayerInJail();
				break;
			case 1:
				StartCoroutine(player.MoveTo(_gameManager.Board[0]));
				break;
			case 2:
				player.AccountBalance -= 50;
				break;
			case 3:
				player.AccountBalance += 10;
				break;
			case 4:
				player.AccountBalance += 50;
				break;
			case 5:
				player.AccountBalance += 200;
				break;
			case 6:
				player.AccountBalance -= 100;
				break;
			case 7:
				player.AccountBalance += 100;
				break;
			case 8:
				StartCoroutine(player.MoveTo(_gameManager.Board[20]));
				break;
			case 9:
				player.AccountBalance += 20;
				break;
			case 10:
			case 12:
				player.AccountBalance += 100;
				break;
			case 11:
				player.AccountBalance -= 50;
				break;
			case 13:
				player.AccountBalance += 25;
				break;
		}
	}
}
