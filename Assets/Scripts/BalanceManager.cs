using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalanceManager : MonoBehaviour {

	public int Balance { get; private set; }
	private Player _player;

	void Awake()
	{
		Balance = 1500;
		_player = gameObject.GetComponentInParent<Player>();
	}

	public void GiveMoneyToPlayer(int money)
	{
		Balance += money;
		Debug.Log(string.Format("Игрок {0} получает {1}Р",_player.PlayerName, money));
	}

	public bool GetMoneyFromPlayer(int money)
	{
		if (Balance < money)
		{
			Debug.Log("Мало денег");
			return false;
		}	
		else
		{
			Balance -= money;
			return true;
		}
	}

	public bool TransferMoneyToPlayer(Player toPlayer, int money)
	{
		Debug.Log(string.Format("Игрок {0} платит игроку {1} {2}Р", _player.PlayerName, toPlayer.PlayerName, money));
		GetMoneyFromPlayer(money);
		toPlayer.BalanceManager.GiveMoneyToPlayer(money);
		return true;
	}
}
