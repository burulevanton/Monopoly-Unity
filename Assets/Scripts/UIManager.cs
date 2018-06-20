using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	private GameManager _gameManager;
	[SerializeField]private ItemList _itemList;

	// Use this for initialization
	void Start ()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

	public void BuyProperty()
	{
		_gameManager.BuyProperty((Ownable)_gameManager.current_player.CurrentLocation);
	}

	public void MortgagePropety()
	{
		_itemList.Clear();
		_itemList.SetAction(MortgageCurrentProperty);
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
		_itemList.SetAction(RedeemCurrentProperty);
		foreach (var property in _gameManager.current_player.Mortgaged)
		{
			_itemList.AddToList(property, true);	
		}
	}

	public void RedeemCurrentProperty(Ownable property)
	{
		_gameManager.RedeemProperty(property);
	}
	
}
