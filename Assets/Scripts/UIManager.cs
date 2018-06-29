using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	private GameManager _gameManager;
	[SerializeField]private ItemList _itemList;
	[SerializeField] private BuyPropertyQuestion _buyPropertyQuestion;
	[SerializeField] private Button _upgradeButton;
	[SerializeField] private Button _sellButton;
	[SerializeField] private Button _mortgageButton;
	[SerializeField] private Button _redeemButton;
	[SerializeField] private Button _endTurn;
	private Button _rollDice;
	public BuyHouse BuyHouse;
	public SellHouse SellHouse;
	public JailUI JailUi;
	public MortgageProperty MortgagePropertyController;
	public RedeemProperty RedeemPropertyController;
	public PlayerInfoManager PlayerInfoManager;
	public GameObject PlayerInfoPrefab;
	public GameObject StartTurnNotification;
	public GameObject EndGameNotification;
	public GameObject StartMenu;

	// Use this for initialization
	void Awake ()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_rollDice = GameObject.Find("RollDice").GetComponent<Button>();
		StartMenu.SetActive(true);
	}

	void Update()
	{
		if (!_gameManager.GameInProgress) return;
		_upgradeButton.interactable = _gameManager.CanPlayerUpgradeAnything() &&
		                              _gameManager.CurrentPlayer.CurrentState == Player.State.Idle;
		

		_sellButton.interactable = _gameManager.CanPlayerSellAnything() &&
		                           _gameManager.CurrentPlayer.CurrentState == Player.State.Idle;
		_rollDice.interactable = _gameManager.CurrentPlayer.CurrentState == Player.State.StartTurn;
		_mortgageButton.interactable = (_gameManager.CurrentPlayer.CurrentState == Player.State.Idle ||
		                                _gameManager.CurrentPlayer.CurrentState == Player.State.StartTurn) &&
		                               _gameManager.CurrentPlayer.Owned.Count > 0;
		_redeemButton.interactable = (_gameManager.CurrentPlayer.CurrentState == Player.State.Idle ||
		                              _gameManager.CurrentPlayer.CurrentState == Player.State.StartTurn) &&
		                             _gameManager.CurrentPlayer.Mortgaged.Count > 0;
		_endTurn.interactable = _gameManager.CurrentPlayer.CurrentState == Player.State.Idle;
	}

	public IEnumerator StartTurn()
	{
		StartTurnNotification.SetActive(true);
		Text text = StartTurnNotification.GetComponentInChildren<Text>();
		text.text = string.Format("Ход игрока {0}", _gameManager.CurrentPlayer.PlayerName);
		yield return StartCoroutine(PlayerInfoManager.StartTurn());
		//yield return new WaitForSeconds(0.8f);
		StartTurnNotification.SetActive(false);
	}

	public void EndGame()
	{
		EndGameNotification.SetActive(true);
		Text text = EndGameNotification.GetComponentInChildren<Text>();
		text.text = string.Format("Игра окончена \n" +
		                          "Победитель - {0}", _gameManager.CurrentPlayer.PlayerName);
	}
	public void RollDice()
	{	
		//PlayerInfoManager.SetPlayerInfo();
		StartCoroutine(_gameManager.RollDice());
	}
	
	public void OfferBuyProperty()
	{
		_buyPropertyQuestion.gameObject.SetActive(true);
		_gameManager.CurrentPlayer.CurrentState = Player.State.OfferToBuyProperty;
		_buyPropertyQuestion.CreateForm();
	}
	public void MortgageProperty()
	{
		MortgagePropertyController.gameObject.SetActive(true);
		MortgagePropertyController.ShowChooseForm(_gameManager.CurrentPlayer);
	}
	public void RedeemProperty()
	{
		RedeemPropertyController.gameObject.SetActive(true);
		RedeemPropertyController.ShowChooseForm(_gameManager.CurrentPlayer);
	}

	public void OfferBuyHouse()
	{
		BuyHouse.gameObject.SetActive(true);
		BuyHouse.ChoosePropertyToUpgrade();
	}
	public void OfferSellHouse()
	{
		SellHouse.gameObject.SetActive(true); //TODO нормальный выбор
		SellHouse.ChoosePropertyToSellHouse();
	}
	public void EndTurn()
	{
		_gameManager.CurrentPlayer.CurrentState = Player.State.EndTurn;
	}
}
