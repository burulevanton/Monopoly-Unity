using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalanceManager : MonoBehaviour {

	public int Balance { get; private set; }
	private Player _player;
	private TextLog _textLog;
	private GameManager _gameManager;

	void Awake()
	{
		Balance = 1500;
		_player = gameObject.GetComponentInParent<Player>();
		_textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void GiveMoneyToPlayer(int money)
	{
		Balance += money;
		_textLog.LogText(string.Format("Игрок {0} получает {1}£",_player.PlayerName, money));
	}

	private bool CheckBalance(int money)
	{
		return Balance >= money;
	}
	public bool GetMoneyFromPlayer(int money)
	{
		if (CheckBalance(money))
		{
			_textLog.LogText(string.Format("{0} платит {1}£",_player.PlayerName, money));
			Balance -= money;
			return true;
		}
		else
		{
			_textLog.LogText(string.Format("{0} не в состоянии выплатить {1}£",_player.PlayerName, money));
			_player.CurrentState = Player.State.Bankrupt;
			StartCoroutine(_gameManager.BankruptByGame(_player));
			return false;
		}
	}

	public bool TransferMoneyToPlayer(Player toPlayer, int money)
	{
		if (CheckBalance(money))
		{
			_textLog.LogText(string.Format("Игрок {0} должен выплатить игроку {1} {2}£", _player.PlayerName, toPlayer.PlayerName, money));
			GetMoneyFromPlayer(money);
			toPlayer.BalanceManager.GiveMoneyToPlayer(money);
			return true;
		}
		else
		{
			_textLog.LogText(string.Format("{0} не в состоянии выплатить {1}£",_player.PlayerName, money));
			_player.CurrentState = Player.State.Bankrupt;
			_gameManager.BankruptByPlayer(_player, toPlayer);
			return false;
		}

	}
}
