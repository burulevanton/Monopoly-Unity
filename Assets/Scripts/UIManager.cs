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
	public BuyHouse BuyHouse;
	public JailUI JailUi;

	// Use this for initialization
	void Start ()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Update()
	{
		_upgradeButton.gameObject.SetActive(_gameManager.CanPlayerUpgradeAnything());
		_sellButton.gameObject.SetActive(_gameManager.CanPlayerSellAnything());
	}

	public void RollDice()
	{	
		StartCoroutine(Rolling());
	}

	public IEnumerator Rolling()
	{
		Button b = GameObject.Find("RollDice").GetComponent<Button>();
		b.gameObject.SetActive(false);
		yield return StartCoroutine(_gameManager.RollDice());
		b.gameObject.SetActive(true);
	}

	public void OfferBuyProperty()
	{
		_buyPropertyQuestion.gameObject.SetActive(true);
		StartCoroutine(_buyPropertyQuestion.WaitForPressed());
	}
	public void BuyProperty()
	{
		_gameManager.BuyProperty((Ownable)_gameManager.current_player.CurrentLocation);
	}

	public void MortgagePropety()
	{
		_itemList.Clear();
		_itemList.SetAction(MortgageCurrentProperty,true);
		foreach (var property in _gameManager.current_player.Owned)
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
		foreach (var property in _gameManager.current_player.Mortgaged)
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
		var property = (Street) _gameManager.current_player.Owned[0];
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
		var property = (Street) _gameManager.current_player.Owned[0]; //TODO нормальный выбор
		BuyHouse.setAction(SellHouses);
		StartCoroutine(BuyHouse.ChoosePropertyToSellHouse());
	}

	public void SellHouses(Street property, int numOfHouses)
	{
		_gameManager.SellHouses(property, numOfHouses);
	}
}
