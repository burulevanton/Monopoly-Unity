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
	public JailUI JailUi;

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

	public IEnumerator OfferBuyProperty()
	{
		_buyPropertyQuestion.gameObject.SetActive(true);
		_gameManager.CurrentPlayer.CurrentState = Player.State.OfferToBuyProperty;
		yield return StartCoroutine(_buyPropertyQuestion.WaitForPressed());
		_gameManager.CurrentPlayer.CurrentState = Player.State.Idle;
	}
	public void BuyProperty()
	{
		_gameManager.BuyProperty((Ownable)_gameManager.CurrentPlayer.CurrentLocation);
	}

	public void MortgagePropety()
	{
		_itemList.Clear();
		_itemList.SetAction(MortgageCurrentProperty,true);
		foreach (var property in _gameManager.CurrentPlayer.Owned)
		{
			_itemList.AddToList(property, true);	
		}
	}

	public void MortgageCurrentProperty(Ownable property)
	{
		_gameManager.MortgageProperty(property);
	}
	public void RedeemPropety()
	{
		_itemList.Clear();
		_itemList.SetAction(RedeemCurrentProperty,true);
		foreach (var property in _gameManager.CurrentPlayer.Mortgaged)
		{
			_itemList.AddToList(property, true);	
		}
	}

	public void RedeemCurrentProperty(Ownable property)
	{
		_gameManager.RedeemProperty(property);
	}

	public void OfferBuyHouse()
	{
		var property = (Street) _gameManager.CurrentPlayer.Owned[0];
		BuyHouse.setAction(BuyHouses);
		StartCoroutine(BuyHouse.ChoosePropertyToUpgrade());
		//StartCoroutine(BuyHouse.CreateForm(property.MaxHouseCanBeBuild()));
	}

	public void BuyHouses(Street property, int numOfHouses)
	{
		_gameManager.BuyHouses(property, numOfHouses);
	}
	public void OfferSellHouse()
	{
		var property = (Street) _gameManager.CurrentPlayer.Owned[0]; //TODO нормальный выбор
		BuyHouse.setAction(SellHouses);
		StartCoroutine(BuyHouse.ChoosePropertyToSellHouse());
	}

	public void SellHouses(Street property, int numOfHouses)
	{
		_gameManager.SellHouses(property, numOfHouses);
	}

	public void StartAuction()
	{
		StartCoroutine(_gameManager.StartAuction());
	}
	public void EndTurn()
	{
		_gameManager.CurrentPlayer.CurrentState = Player.State.EndTurn;
	}
}
