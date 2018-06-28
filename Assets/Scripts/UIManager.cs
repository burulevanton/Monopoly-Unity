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
	private Button _rollDice;
	public BuyHouse BuyHouse;
	public SellHouse SellHouse;
	public JailUI JailUi;
	public MortgageProperty MortgagePropertyController;
	public RedeemProperty RedeemPropertyController;

	// Use this for initialization
	void Start ()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_rollDice = GameObject.Find("RollDice").GetComponent<Button>();
	}

	void Update()
	{
		_upgradeButton.gameObject.SetActive(_gameManager.CanPlayerUpgradeAnything());
		_sellButton.gameObject.SetActive(_gameManager.CanPlayerSellAnything());
		_rollDice.interactable = _gameManager.CurrentPlayer.CurrentState == Player.State.StartTurn;
	}

	public void RollDice()
	{	
		StartCoroutine(_gameManager.RollDice());
	}

//	public IEnumerator Rolling()
//	{
//		Button b = GameObject.Find("RollDice").GetComponent<Button>();
//		b.gameObject.SetActive(false);
//		yield return StartCoroutine(_gameManager.RollDice());
//		b.gameObject.SetActive(true);
//	}

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
		//StartCoroutine(BuyHouse.CreateForm(property.MaxHouseCanBeBuild()));
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
