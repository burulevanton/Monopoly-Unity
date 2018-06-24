using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuyHouse : MonoBehaviour
{

	public Slider Slider;
	public ScrollRect Scroll;
	public Button Button;
	private bool _isPressed;
	private Action<Street, int> _action;
	private Street _choosenProperty;
	public ItemList ItemList;
	public GameObject ChooseProperty;
	public GameObject ChooseAmount;
	private GameManager _gameManager;


	public void setAction(Action<Street, int> action)
	{
		_action = action;
	}
	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_isPressed = false;
		Button.onClick.AddListener(ButtonPressed);
		Button.onClick.AddListener(Accept);
		_choosenProperty = null;
	}
	
	void Accept()
	{
		_action(_choosenProperty, (int) Slider.value);
	}
	private void ButtonPressed()
	{
		_isPressed = true;
	}

	private void SetProperty(Ownable property)
	{
		_choosenProperty = (Street) property;
		_isPressed = true;
	}
	public IEnumerator ChoosePropertyToUpgrade()
	{
		this.gameObject.SetActive(true);
		ChooseProperty.gameObject.SetActive(true);
		ItemList.scroll = Scroll;
		ItemList.Clear();
		ItemList.SetAction(SetProperty, false);
		var upgradeProperies = _gameManager.current_player.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street)x).ToList()
			.FindAll(x=>x.CanUpgrade());
		foreach (var property in upgradeProperies)
		{
			ItemList.AddToList(property, true);
		}
		yield return new WaitUntil(()=>_isPressed==true);
		ChooseProperty.gameObject.SetActive(false);
		if (_choosenProperty != null)
			yield return CreateForm(_choosenProperty.MaxHouseCanBeBuild());
	}

	public IEnumerator ChoosePropertyToSellHouse()
	{
		this.gameObject.SetActive(true);
		ChooseProperty.gameObject.SetActive(true);
		ItemList.scroll = Scroll;
		ItemList.Clear();
		ItemList.SetAction(SetProperty, false);
		var sellProperties = _gameManager.current_player.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street) x).ToList()
			.FindAll(x => x.CurrentUpgradeLevel > 0);
		foreach (var property in sellProperties)
		{
			ItemList.AddToList(property, true);
		}
		yield return new WaitUntil(()=>_isPressed==true);
		ChooseProperty.gameObject.SetActive(false);
		if (_choosenProperty != null)
			yield return CreateForm(_choosenProperty.CurrentUpgradeLevel);
	}
	public IEnumerator CreateForm(int maxValue)
	{
		ChooseAmount.gameObject.SetActive(true);
		Slider.maxValue = maxValue;
		_isPressed = false;
		yield return new WaitUntil((() => _isPressed==true));
		_isPressed = false;
		ChooseAmount.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
		Slider.value = 0;
	}
}
