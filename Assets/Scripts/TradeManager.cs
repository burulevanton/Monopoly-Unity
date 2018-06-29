using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{

	public Text Player1Name;
	public Text Player1Balance;
	public ScrollRect Player1Properties;

	public InputField Player1OfferMoney;
	public ScrollRect Player1OfferProperties;
	
	
	public Text Player2Name;
	public Text Player2Balance;
	public ScrollRect Player2Properties;

	public InputField Player2OfferMoney;
	public ScrollRect Player2OfferProperties;

	private int _player1OfferMoney;
	private List<Ownable> _player1OfferProperties;

	private int _player2OfferMoney;
	private List<Ownable> _player2OfferProperties;

	private ItemList _itemListPlayer1PropertiesLeft;
	private ItemList _itemListPlayer1PropertiesOffer;

	private ItemList _itemListPlayer2PropertiesLeft;
	private ItemList _itemListPlayer2PropertiesOffer;

	private Player _player1;
	private Player _player2;

	[SerializeField]private GameManager _gameManager;
	public RectTransform Button;

	public Button CreateOffer;
	public Button DeclineTrade;

	public Button AcceptOffer;
	public Button CounterOffer;
	public Button DeclineOffer;

	public GameObject TradeCreateOffer;
	public GameObject TradeAnswerToOffer;

	public ScrollRect UGetProperties;
	public ScrollRect ULoseProperties;

	public Text UGetMoney;
	public Text ULoseMoney;

	public Text ErrorPlayer1;
	public Text ErrorPlayer2;

	private bool _isOfferValidForPlayer1;
	private bool _isOfferValidForPlayer2;

	void Update()
	{
		ErrorPlayer1.gameObject.SetActive(!_isOfferValidForPlayer1);
		ErrorPlayer2.gameObject.SetActive(!_isOfferValidForPlayer2);
		if (CreateOffer.IsActive())
			CreateOffer.interactable = _isOfferValidForPlayer1 && _isOfferValidForPlayer2;
	}

	private void Start()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Awake()
	{
		_isOfferValidForPlayer1 = true;
		_isOfferValidForPlayer2 = true;
		
		_player1OfferProperties = new List<Ownable>();
		_player2OfferProperties = new List<Ownable>();
		DeclineTrade.onClick.AddListener(CancelTrade);
		CreateOffer.onClick.AddListener(CreateTradeOffer);
		AcceptOffer.onClick.AddListener(AcceptTradeOffer);
		DeclineOffer.onClick.AddListener(DeclineTradeOffer);
		CounterOffer.onClick.AddListener(CounterOfferTradeOffer);
	}
	
	private void AddToPlayer1Offer(Ownable property)
	{
		_player1OfferProperties.Add(property);
		_itemListPlayer1PropertiesOffer.AddToList(property, true);
	}

	private void AddToPlayer2Offer(Ownable property)
	{
		_player2OfferProperties.Add(property);
		_itemListPlayer2PropertiesOffer.AddToList(property, true);
	}

	private void RemoveFromPlayer1Offer(Ownable property)
	{
		_player1OfferProperties.Remove(property);
		_itemListPlayer1PropertiesLeft.AddToList(property, true);
	}

	private void RemoveFromPlayer2Offer(Ownable property)
	{
		_player2OfferProperties.Remove(property);
		_itemListPlayer2PropertiesLeft.AddToList(property, true);
	}

	private void ShowInfo(Ownable property)
	{
		Debug.Log(property.name);
	}
	private void InitComponentsCreateTrade()
	{
		Player1Name.text = string.Format("Игрок {0}",_player1.PlayerName); 
		Player2Name.text = string.Format("Игрок {0}",_player2.PlayerName);
		Player1Balance.text = string.Format("Баланс: {0}Р", _player1.BalanceManager.Balance);
		Player2Balance.text = string.Format("Баланс: {0}Р", _player2.BalanceManager.Balance);
		_itemListPlayer1PropertiesLeft = this.gameObject.AddComponent<ItemList>();
		_itemListPlayer1PropertiesOffer = this.gameObject.AddComponent<ItemList>();
		_itemListPlayer2PropertiesLeft = this.gameObject.AddComponent<ItemList>();
		_itemListPlayer2PropertiesOffer = this.gameObject.AddComponent<ItemList>();
		_itemListPlayer1PropertiesLeft.element = Button;
		_itemListPlayer1PropertiesOffer.element = Button;
		_itemListPlayer2PropertiesLeft.element = Button;
		_itemListPlayer2PropertiesOffer.element = Button;
		_itemListPlayer1PropertiesLeft.scroll = Player1Properties;
		_itemListPlayer1PropertiesOffer.scroll = Player1OfferProperties;
		_itemListPlayer2PropertiesLeft.scroll = Player2Properties;
		_itemListPlayer2PropertiesOffer.scroll = Player2OfferProperties;
		_itemListPlayer1PropertiesLeft.SetAction(AddToPlayer1Offer, true);
		_itemListPlayer1PropertiesOffer.SetAction(RemoveFromPlayer1Offer, true);
		_itemListPlayer2PropertiesLeft.SetAction(AddToPlayer2Offer, true);
		_itemListPlayer2PropertiesOffer.SetAction(RemoveFromPlayer2Offer, true);
	}

	private void DestroyComponentsCreateTrade()
	{
		_itemListPlayer1PropertiesLeft.Clear();
		_itemListPlayer1PropertiesOffer.Clear();
		_itemListPlayer2PropertiesLeft.Clear();
		_itemListPlayer2PropertiesOffer.Clear();
		Destroy(_itemListPlayer1PropertiesLeft);
		Destroy(_itemListPlayer1PropertiesOffer);
		Destroy(_itemListPlayer2PropertiesLeft);
		Destroy(_itemListPlayer2PropertiesOffer);
	}
	private void InitComponentsTradeOffer()
	{
		DestroyComponentsCreateTrade();
		UGetMoney.text = String.Format("Денег: {0}Р", _player1OfferMoney);
		ULoseMoney.text = String.Format("Денег: {0}Р", _player2OfferMoney);
		_itemListPlayer1PropertiesOffer = this.gameObject.AddComponent<ItemList>();
		_itemListPlayer2PropertiesOffer = this.gameObject.AddComponent<ItemList>();
		_itemListPlayer1PropertiesOffer.element = Button;
		_itemListPlayer2PropertiesOffer.element = Button;
		_itemListPlayer1PropertiesOffer.scroll = UGetProperties;
		_itemListPlayer2PropertiesOffer.scroll = ULoseProperties;
		_itemListPlayer1PropertiesOffer.SetAction(ShowInfo, false);
		_itemListPlayer2PropertiesOffer.SetAction(ShowInfo, false);
	}

	private void DestroyComponentsTradeOffer()
	{
		_itemListPlayer1PropertiesOffer.Clear();
		_itemListPlayer2PropertiesOffer.Clear();
		Destroy(_itemListPlayer1PropertiesOffer);
		Destroy(_itemListPlayer2PropertiesOffer);
	}
	public void StartTrade(Player player1, Player player2)
	{
		_player1 = player1;
		_player2 = player2;
		_gameManager.State = GameManager.States.Trade;
		this.gameObject.SetActive(true);
		InitComponentsCreateTrade();
		foreach (var property in player1.Owned)
		{
			_itemListPlayer1PropertiesLeft.AddToList(property, true);
		}

		foreach (var property in player2.Owned)
		{
			_itemListPlayer2PropertiesLeft.AddToList(property, true);
		}
	}

	private void CancelTrade()
	{
		_player1OfferProperties.Clear();
		_player2OfferProperties.Clear();
		DestroyComponentsCreateTrade();
		_gameManager.State = GameManager.States.Default;
		this.gameObject.SetActive(false);
	}

	private void CreateTradeOffer()
	{
		TradeCreateOffer.SetActive(false);
		TradeAnswerToOffer.SetActive(true);
		_player1OfferMoney = Convert.ToInt32(Player1OfferMoney.text);
		_player2OfferMoney = Convert.ToInt32(Player2OfferMoney.text);
		InitComponentsTradeOffer();
		foreach (var property in _player1OfferProperties)
		{
			_itemListPlayer1PropertiesOffer.AddToList(property, true);
		}
		foreach (var property in _player2OfferProperties)
		{
			_itemListPlayer2PropertiesOffer.AddToList(property, true);
		}
	}

	private void AcceptTradeOffer()
	{
		this.gameObject.SetActive(false);
		DestroyComponentsTradeOffer();
		foreach (var property in _player1OfferProperties)
		{
			_gameManager.TransferPropertyBetweenPlayers(_player1, _player2, property);
		}

		foreach (var property in _player2OfferProperties)
		{
			_gameManager.TransferPropertyBetweenPlayers(_player2, _player1, property);
		}

		_player1.BalanceManager.TransferMoneyToPlayer(_player2, _player1OfferMoney);
		_player2.BalanceManager.TransferMoneyToPlayer(_player1, _player2OfferMoney);
		_gameManager.State = GameManager.States.Default;
	}

	private void DeclineTradeOffer()
	{
		this.gameObject.SetActive(false);
		_player1OfferProperties.Clear();
		_player2OfferProperties.Clear();
		DestroyComponentsTradeOffer();
		_gameManager.State = GameManager.States.Default;
	}

	private void CounterOfferTradeOffer()
	{
		TradeAnswerToOffer.SetActive(false);
		TradeCreateOffer.SetActive(true);
		DestroyComponentsTradeOffer();
		var tempPlayer = _player1;
		_player1 = _player2;
		_player2 = tempPlayer;
		var tempList = new List<Ownable>(_player1OfferProperties);
		_player1OfferProperties = new List<Ownable>(_player2OfferProperties);
		_player2OfferProperties = new List<Ownable>(tempList);
		var tempMoney = _player1OfferMoney;
		_player1OfferMoney = _player2OfferMoney;
		_player2OfferMoney = tempMoney;
		InitComponentsCreateTrade();
		Player1OfferMoney.text = Convert.ToString(_player1OfferMoney);
		Player2OfferMoney.text = Convert.ToString(_player2OfferMoney);
		foreach (var property in _player1.Owned)
		{
			if (!_player1OfferProperties.Contains(property))
			{
				_itemListPlayer1PropertiesLeft.AddToList(property, true);
			}
		}

		foreach (var property in _player2.Owned)
		{
			if(!_player2OfferProperties.Contains(property))
				_itemListPlayer2PropertiesLeft.AddToList(property, true);
		}

		foreach (var property in _player1OfferProperties)
		{
			_itemListPlayer1PropertiesOffer.AddToList(property, true);
		}

		foreach (var property in _player2OfferProperties)
		{
			_itemListPlayer2PropertiesOffer.AddToList(property, true);
		}
	}

	public void CheckValidForPlayer1()
	{
		var player1OfferMoney = Convert.ToInt32(Player1OfferMoney.text);
		_isOfferValidForPlayer1 = player1OfferMoney < _player1.BalanceManager.Balance;
	}
	public void CheckValidForPlayer2()
	{
		var player2OfferMoney = Convert.ToInt32(Player2OfferMoney.text);
		_isOfferValidForPlayer2 = player2OfferMoney < _player2.BalanceManager.Balance;
	}
}
