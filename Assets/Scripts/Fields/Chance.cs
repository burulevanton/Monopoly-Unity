using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chance : Field
{

	private GameManager _gameManager;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	public override void LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
		var numCard = _gameManager.ChanceCards.Dequeue();
		var indexLocation = Array.IndexOf(_gameManager.Board, player.CurrentLocation);
		_gameManager.ChanceCards.Enqueue(numCard);
		//TODO добавить 200 за проход
		switch (numCard)
		{
			case 0:
				StartCoroutine(player.MoveTo(_gameManager.Board[39]));
				break;
			case 1:
				StartCoroutine(player.MoveTo(_gameManager.Board[11]));
				break;
			case 2:
			case 7:
				switch (indexLocation)
				{
					case 7:
						StartCoroutine(player.MoveTo(_gameManager.Board[15]));
						break;
					case 22:
						StartCoroutine(player.MoveTo(_gameManager.Board[25]));
						break;
					case 36:
						StartCoroutine(player.MoveTo(_gameManager.Board[5]));
						break;
				}
				break;
			case 3:
				player.BalanceManager.GiveMoneyToPlayer(50);
				break;
			case 4:
				switch (indexLocation)
				{
					case 7:
					case 36:
						StartCoroutine(player.MoveTo(_gameManager.Board[12]));
						break;
					case 22:
						StartCoroutine(player.MoveTo(_gameManager.Board[28]));
						break;
				}
				break;
			case 5:
				//TODO отправка в тюрьму
				_gameManager.JailManager.PutPLayerInJail();
				break;
			case 6:
				StartCoroutine(player.MoveTo(_gameManager.Board[5]));
				break;
			case 8:
				player.BalanceManager.GetMoneyFromPlayer(15);
				break; //TODO информирование о потере денег
			case 9:
				StartCoroutine(player.MoveTo(_gameManager.Board[20]));
				break;
			case 10:
				player.BalanceManager.GiveMoneyToPlayer(150);
				break;
			case 11:
				StartCoroutine(player.MoveTo(_gameManager.Board[24]));
				break;
			case 12:
				StartCoroutine(player.MoveTo(_gameManager.Board[indexLocation - 3]));
				break;
			case 13:
				StartCoroutine(player.MoveTo(_gameManager.Board[0]));
				break;
		}
	}
}
