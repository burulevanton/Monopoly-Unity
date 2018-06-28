﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuyHouse : MonoBehaviour
{

	public Slider Slider;
	public ScrollRect Scroll;
	public Button Accept;
	public RectTransform Button;
	private Street _choosenProperty;
	private ItemList _itemList;
	public GameObject ChooseProperty;
	public GameObject ChooseAmount;
	private GameManager _gameManager;
	public Text Info;
	public Text Error;
	private bool _isValid;
	//todo добавить отмену

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_itemList = gameObject.AddComponent<ItemList>();
		_itemList.element = Button;
		_isValid = true;
		Accept.onClick.AddListener(AcceptOffer);
		_choosenProperty = null;
	}

	void Update()
	{
		Error.gameObject.SetActive(!_isValid);
		Accept.interactable = _isValid;
	}
	
	void AcceptOffer()
	{
		_gameManager.BuyHouses(_choosenProperty, (int) Slider.value);
		ChooseAmount.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
		Slider.value = 0;
		_gameManager.CurrentPlayer.CurrentState = Player.State.Idle;
	}

	private void SetProperty(Ownable property)
	{
		_choosenProperty = (Street) property;
		ChooseProperty.gameObject.SetActive(false);
		CreateForm(_choosenProperty.MaxHouseCanBeBuild());
	}
	public void ChoosePropertyToUpgrade()
	{
		_gameManager.CurrentPlayer.CurrentState = Player.State.OfferToBuyHouse;
		this.gameObject.SetActive(true);
		ChooseProperty.gameObject.SetActive(true);
		_itemList.scroll = Scroll;
		_itemList.Clear();
		_itemList.SetAction(SetProperty, false);
		var upgradeProperies = _gameManager.CurrentPlayer.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street)x).ToList()
			.FindAll(x=>x.CanUpgrade());
		foreach (var property in upgradeProperies)
		{
			_itemList.AddToList(property, true);
		}
	}

//	public void ChoosePropertyToSellHouse()
//	{
//		_gameManager.CurrentPlayer.CurrentState = Player.State.OfferToSellHouse;
//		this.gameObject.SetActive(true);
//		ChooseProperty.gameObject.SetActive(true);
//		_itemList.scroll = Scroll;
//		_itemList.Clear();
//		_itemList.SetAction(SetProperty, false);
//		var sellProperties = _gameManager.CurrentPlayer.Owned.FindAll(x => x.GetType() == typeof(Street))
//			.Select(x => (Street) x).ToList()
//			.FindAll(x => x.CurrentUpgradeLevel > 0);
//		foreach (var property in sellProperties)
//		{
//			_itemList.AddToList(property, true);
//		}
//		ChooseProperty.gameObject.SetActive(false);
//		if (_choosenProperty != null)
//			yield return CreateForm(_choosenProperty.CurrentUpgradeLevel);
//	}
	private void CreateForm(int maxValue)
	{
		ChooseAmount.gameObject.SetActive(true);
		Slider.maxValue = maxValue;
		CheckValid();
	}
	public void CheckValid()
	{
		var numOfHouse = (int)Slider.value;
		var costOfUpgrade = numOfHouse * _choosenProperty.HousePrice;
		if (costOfUpgrade < _gameManager.CurrentPlayer.BalanceManager.Balance)
		{
			_isValid = true;
			Info.gameObject.SetActive(true);
			Info.text = string.Format("Количество домов - {0} \n " +
			                          "Стоимость - {1}", numOfHouse, costOfUpgrade);
			Error.gameObject.SetActive(false);
		}
		else
		{
			_isValid = false;
			Info.gameObject.SetActive(false);
			Error.gameObject.SetActive(true);
		}
	}
}
