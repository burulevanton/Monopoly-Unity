using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CommunityChest : Field {

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
		_gameManager.ChanceCards.Enqueue(numCard);
		switch (numCard)
		{
			case 0:
				_textLog.LogText("Отправляйтесь в тюрьму");
				_gameManager.JailManager.PutPLayerInJail();
				break;
			case 1:
				_textLog.LogText("Отправляйтесь на поле Вперёд");
				StartCoroutine(player.MoveTo(_gameManager.Board[0]));
				break;
			case 2:
				_textLog.LogText("Заплатите 50£");
				player.BalanceManager.GetMoneyFromPlayer(50);
				break;
			case 3:
				_textLog.LogText("Получите 10£");
				player.BalanceManager.GiveMoneyToPlayer(10);
				break;
			case 4:
				_textLog.LogText("Получите 50£");
				player.BalanceManager.GiveMoneyToPlayer(50);
				break;
			case 5:
				_textLog.LogText("Получите 200£");
				player.BalanceManager.GiveMoneyToPlayer(200);
				break;
			case 6:
				_textLog.LogText("Заплатите 100£");
				player.BalanceManager.GetMoneyFromPlayer(100);
				break;
			case 7:
				_textLog.LogText("Получите 100£");
				player.BalanceManager.GiveMoneyToPlayer(100);
				break;
			case 8:
				_textLog.LogText("Отправляйтесь на бесплатную стоянку");
				StartCoroutine(player.MoveTo(_gameManager.Board[20]));
				break;
			case 9:
				_textLog.LogText("Получите 20£");
				player.BalanceManager.GiveMoneyToPlayer(20);
				break;
			case 10:
			case 12:
				_textLog.LogText("Получите 100£");
				player.BalanceManager.GiveMoneyToPlayer(100);
				break;
			case 11:
				_textLog.LogText("Заплатите 50£");
				player.BalanceManager.GetMoneyFromPlayer(50);
				break;
			case 13:
				_textLog.LogText("Получите 25£");
				player.BalanceManager.GiveMoneyToPlayer(25);
				break;
		}
	}
}
