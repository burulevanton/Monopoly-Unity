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
		_gameManager.CurrentPlayer.InJail = true;
		StartCoroutine(_gameManager.CurrentPlayer.MoveTo(_gameManager.Board[10], false));
	}

	public void BuyOutOfJail()
	{
		_gameManager.CurrentPlayer.InJail = false;
		_gameManager.CurrentPlayer.NumOfTurnsInJail = 0;
		_gameManager.CurrentPlayer.BalanceManager.GetMoneyFromPlayer(50); //todo инфа о выходе и нормальное перемещение
		StartCoroutine(_gameManager.RollDice());
	}

	public void TryRoll()
	{
		_gameManager.DiceRoller.RollDice();
		if (_gameManager.DiceRoller.IsDouble())
		{
			_gameManager.CurrentPlayer.InJail = false;
			_gameManager.CurrentPlayer.NumOfTurnsInJail = 0;
			StartCoroutine(_gameManager.CurrentPlayer.MoveTo(_gameManager.Board[_gameManager.NextLocation()]));
		}
		else
			_gameManager.CurrentPlayer.NumOfTurnsInJail++;
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
