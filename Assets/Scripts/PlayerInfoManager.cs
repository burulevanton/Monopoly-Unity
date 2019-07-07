using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour {

	private GameManager _gameManager;
	public PlayerInfo[] PlayerInfo;
	public GameObject Location;

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

	public IEnumerator StartTurn()
	{
		var _playerInfo = PlayerInfo[_gameManager.ActivePlayers.IndexOf(_gameManager.CurrentPlayer)];
		_playerInfo.GetComponentInChildren<Button>().gameObject.SetActive(false);
		_gameManager.CurrentPlayer.CurrentState = Player.State.Moving;
		Vector3 startPosition = _playerInfo.transform.position;
		Vector3 endPosition = Location.transform.position;
		float pathLength = Vector2.Distance(startPosition, endPosition);
		float totalTimeForPath = pathLength / 1500f;
		float lastSwitchTime = Time.time;
		while (_playerInfo.transform.position != Location.transform.position)
		{
			float currentTimeOnPath = Time.time - lastSwitchTime;
			_playerInfo.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
			yield return null;
		}
		
			yield return null;
		_gameManager.CurrentPlayer.CurrentState = Player.State.StartTurn;
	}

	public IEnumerator EndTurn()
	{
		var _playerInfo = PlayerInfo[_gameManager.ActivePlayers.IndexOf(_gameManager.CurrentPlayer)];
		var currentState = _gameManager.CurrentPlayer.CurrentState;
		_gameManager.CurrentPlayer.CurrentState = Player.State.Moving;
		Vector3 startPosition = Location.transform.position;
		Vector3 endPosition = _playerInfo.Location;
		float pathLength = Vector2.Distance(startPosition, endPosition);
		float totalTimeForPath = pathLength / 1500f;
		float lastSwitchTime = Time.time;
		while (_playerInfo.Location != _playerInfo.transform.position)
		{
			float currentTimeOnPath = Time.time - lastSwitchTime;
			_playerInfo.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
			yield return null;
		}
		
		yield return null;
		_playerInfo.StartTrade.gameObject.SetActive(true);
		_gameManager.CurrentPlayer.CurrentState = currentState;
	}
}
