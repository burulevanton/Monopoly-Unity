using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailManager : MonoBehaviour
{

	private GameManager _gameManager;
	[SerializeField]private JailUI _jailUi;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void PutPLayerInJail()
	{
		_gameManager.current_player.InJail = true;
		StartCoroutine(_gameManager.current_player.MoveTo(_gameManager.Board[10]));
	}

	public void BuyOutOfJail()
	{
		_gameManager.current_player.InJail = false;
		_gameManager.current_player.NumOfTurnsInJail = 0;
		_gameManager.current_player.AccountBalance -= 50;
		StartCoroutine(_gameManager.RollDice());
	}

	public void TryRoll()
	{
		_gameManager.DiceRoller.RollDice();
		if (_gameManager.DiceRoller.IsDouble())
		{
			_gameManager.current_player.InJail = false;
			_gameManager.current_player.NumOfTurnsInJail = 0;
			StartCoroutine(_gameManager.current_player.MoveTo(_gameManager.Board[_gameManager.NextLocation()]));
		}
		else
			_gameManager.current_player.NumOfTurnsInJail++;
	}

	public IEnumerator TurnInJail()
	{
		_gameManager.current_player.CurrentState = Player.State.InJail;
		if(_gameManager.current_player.NumOfTurnsInJail == 3)
			BuyOutOfJail();
		else
		{
			_jailUi.gameObject.SetActive(true);
			yield return StartCoroutine(_jailUi.WaitForPressed());
		}

		_gameManager.current_player.CurrentState = Player.State.StartTurn;
	}
}
