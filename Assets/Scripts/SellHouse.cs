using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SellHouse : MonoBehaviour
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
	//todo добавить отмену

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_itemList = gameObject.AddComponent<ItemList>();
		_itemList.element = Button;
		Accept.onClick.AddListener(AcceptOffer);
		_choosenProperty = null;
	}
	
	void AcceptOffer()
	{
		_gameManager.SellHouses(_choosenProperty, (int) Slider.value);
		ChooseAmount.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
		Slider.value = 0;
		_gameManager.CurrentPlayer.CurrentState = Player.State.Idle;
	}

	private void SetProperty(Ownable property)
	{
		_choosenProperty = (Street) property;
		ChooseProperty.gameObject.SetActive(false);
		CreateForm(_choosenProperty.CurrentUpgradeLevel);
	}

	public void ChoosePropertyToSellHouse()
	{
		_gameManager.CurrentPlayer.CurrentState = Player.State.OfferToSellHouse;
		this.gameObject.SetActive(true);
		ChooseProperty.gameObject.SetActive(true);
		_itemList.scroll = Scroll;
		_itemList.Clear();
		_itemList.SetAction(SetProperty, false);
		var sellProperties = _gameManager.CurrentPlayer.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street) x).ToList()
			.FindAll(x => x.CurrentUpgradeLevel > 0);
		foreach (var property in sellProperties)
		{
			_itemList.AddToList(property, true);
		}
	}
	private void CreateForm(int maxValue)
	{
		ChooseAmount.gameObject.SetActive(true);
		Slider.maxValue = maxValue;
		CheckValid();
	}
	public void CheckValid()
	{
		var numOfHouse = (int)Slider.value;
		var amount = numOfHouse * _choosenProperty.HousePrice / 2;
		Info.text = string.Format("Количество домов на продажу - {0} \n" +
		            "Вы получите - {1}Р", numOfHouse, amount);

	}
}
