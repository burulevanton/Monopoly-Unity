using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chance : Field
{

	private GameManager _gameManager;
	private TextLog _textLog;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
	}
	
	public override void LandOn(Player player)
	{
		_gameManager.TextLog.LogText(string.Format("{0} попал на поле {1}",_gameManager.CurrentPlayer.PlayerName, this.Name));
		var numCard = _gameManager.ChanceCards.Dequeue();
		var indexLocation = Array.IndexOf(_gameManager.Board, player.CurrentLocation);
		_gameManager.ChanceCards.Enqueue(numCard);
		switch (numCard)
		{
			case 0:
				_textLog.LogText("Отправляйтесь на Mayfair");
				StartCoroutine(player.MoveTo(_gameManager.Board[39]));
				break;
			case 1:
				_textLog.LogText("Отправляйтесь на Pall Mall");
				StartCoroutine(player.MoveTo(_gameManager.Board[11]));
				break;
			case 2:
			case 7:
				_textLog.LogText("Отправляйтесь на ближайшую железнодорожную станцию");
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
				_textLog.LogText("Получите 50£");
				player.BalanceManager.GiveMoneyToPlayer(50);
				break;
			case 4:
				_textLog.LogText("Отправляйтесь на ближайшее коммунальное предприятие");
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
				_textLog.LogText("Отправляйтесь в тюрьму");
				_gameManager.JailManager.PutPLayerInJail();
				break;
			case 6:
				_textLog.LogText("Отправляйтесь на Kings Cross Station");
				StartCoroutine(player.MoveTo(_gameManager.Board[5]));
				break;
			case 8:
				_textLog.LogText("Отдайте 15£");
				player.BalanceManager.GetMoneyFromPlayer(15);
				break; //TODO информирование о потере денег
			case 9:
				_textLog.LogText("Отправляйтесь на бесплатную стоянку");
				StartCoroutine(player.MoveTo(_gameManager.Board[20]));
				break;
			case 10:
				_textLog.LogText("Получите 150£");
				player.BalanceManager.GiveMoneyToPlayer(150);
				break;
			case 11:
				_textLog.LogText("Отправляйтесь на Trafalgar Square");
				StartCoroutine(player.MoveTo(_gameManager.Board[24]));
				break;
			case 12:
				_textLog.LogText("Идите на 3 ячейки назад");
				StartCoroutine(player.MoveTo(_gameManager.Board[indexLocation - 3]));
				break;
			case 13:
				_textLog.LogText("Отправляйтесь на поле Вперёд");
				StartCoroutine(player.MoveTo(_gameManager.Board[0]));
				break;
		}
	}
}
