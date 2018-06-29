using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailManager : MonoBehaviour
{

	private GameManager _gameManager;
	private TextLog _textLog;
	[SerializeField]private JailUI _jailUi;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
	}

	public void PutPLayerInJail()
	{
		_gameManager.CurrentPlayer.InJail = true;
		StartCoroutine(_gameManager.CurrentPlayer.MoveTo(_gameManager.Board[10], false));
	}

	public void BuyOutOfJail()
	{
		_gameManager.CurrentPlayer.InJail = false;
		_gameManager.CurrentPlayer.NumOfTurnsInJail = 0;
		if (!_gameManager.CurrentPlayer.BalanceManager.GetMoneyFromPlayer(50)) return; //todo инфа о выходе и нормальное перемещение
		_textLog.LogText(string.Format("{0} выходит из тюрьмы", _gameManager.CurrentPlayer.PlayerName));
		StartCoroutine(_gameManager.RollDice());
	}

	public void TryRoll()
	{
		_gameManager.DiceRoller.RollDice();
		_textLog.LogText(string.Format("{0} выбросил {1} и {2}", _gameManager.CurrentPlayer,
			_gameManager.DiceRoller.Dice1, _gameManager.DiceRoller.Dice2));
		if (_gameManager.DiceRoller.IsDouble())
		{
			_gameManager.CurrentPlayer.InJail = false;
			_gameManager.CurrentPlayer.NumOfTurnsInJail = 0;
			_textLog.LogText(string.Format("{0} выходит из тюрьмы", _gameManager.CurrentPlayer.PlayerName));
			StartCoroutine(_gameManager.CurrentPlayer.MoveTo(_gameManager.Board[_gameManager.NextLocation()]));
		}
		else
		{
			_gameManager.CurrentPlayer.NumOfTurnsInJail++;
			_textLog.LogText(string.Format("{0} провёл в тюрьме уже {1} хода", _gameManager.CurrentPlayer.PlayerName, _gameManager.CurrentPlayer.NumOfTurnsInJail));
		}
	}

	public IEnumerator TurnInJail()
	{
		_gameManager.CurrentPlayer.CurrentState = Player.State.InJail;
		if(_gameManager.CurrentPlayer.NumOfTurnsInJail == 3)
			BuyOutOfJail();
		else
		{
			_jailUi.gameObject.SetActive(true);
			yield return StartCoroutine(_jailUi.WaitForPressed());
		}

		_gameManager.CurrentPlayer.CurrentState = Player.State.Idle;
	}
}
