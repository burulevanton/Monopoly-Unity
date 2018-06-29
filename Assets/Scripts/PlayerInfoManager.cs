using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour {

	private GameManager _gameManager;
	public PlayerInfo[] PlayerInfo;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public IEnumerator SetPlayerInfo()
	{
		//todo поменять
		for (var index = 0; index < PlayerInfo.Length; index++)
		{
			if (!_gameManager.ActivePlayers.Contains(_gameManager.Players[index]))
				yield break;
			var playerInfo = PlayerInfo[index];
			playerInfo.SetPlayer(_gameManager.ActivePlayers[index]);
			yield return null;
		}
	}
}
