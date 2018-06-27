using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityScript.Steps;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{

	public Player[] Players;
	public Player CurrentPlayer;
	public Field[] Board;
	public Queue<int> ChanceCards;
	public Queue<int> PublicTreasuryCards;
	public DiceRoller DiceRoller;
	public JailManager JailManager;
	public AuctionManager AuctionManager;
	public TradeManager TradeManager;
	private bool _gameInProgress;
	private Queue<Player> _players;

	private enum States
	{
		Default,
		Auction,
		Trade
	};

	private States _state;

	private void Start()
	{
		ChanceCards = new Queue<int>();
		PublicTreasuryCards = new Queue<int>();
		for (var i = 0; i < 14; i++)
		{
			ChanceCards.Enqueue(i);
			PublicTreasuryCards.Enqueue(i);
		}
		_state = States.Default;
		_gameInProgress = true;
		_players = new Queue<Player>();
		foreach (var player in Players)
		{
			player.CurrentLocation = Board[0];
			_players.Enqueue(player);
		}

		CurrentPlayer = _players.Dequeue();
		StartCoroutine(Game());
		
	}

	void Update()
	{
//		if (!_gameInProgress)
//		{
//			_gameInProgress = true;
//			StartCoroutine(Game());
//		}
//		if (!_state.Equals(States.Auction))
//		{
//			StartCoroutine(AuctionManager.StartAuction((Ownable)Board[25],Players[0]));
//			_state = States.Auction;
//		}

//		if (!_state.Equals(States.Trade))
//		{
			
//			GivePropertyToPlayer(Players[1],Board[12] as Ownable);
//			GivePropertyToPlayer(Players[1],Board[13] as Ownable);
//			GivePropertyToPlayer(Players[1],Board[14] as Ownable);
//			GivePropertyToPlayer(Players[1],Board[15] as Ownable);
//			GivePropertyToPlayer(Players[1],Board[16] as Ownable);
//			GivePropertyToPlayer(Players[1],Board[18] as Ownable);
//			GivePropertyToPlayer(Players[1],Board[19] as Ownable);
//			_state = States.Trade;
//			TradeManager.StartTrade(Players[0], Players[1]);
//		}
	}

	private IEnumerator Game()
	{
//		yield return StartCoroutine(current_player.MoveTo(Board[1]));
//		yield return new WaitForSeconds(2);
//		yield return StartCoroutine(current_player.MoveTo(Board[3]));
//		yield return new WaitForSeconds(2);
		//yield return StartCoroutine(BankruptByGame(Players[0]));
		while (_gameInProgress)
		{
			if (_state == States.Default)
			{
				switch (CurrentPlayer.CurrentState)
				{
					case Player.State.Moving:
						yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.Moving);
						break;
					case Player.State.OfferToBuyProperty:
						yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToBuyProperty);
						break;
					case Player.State.OfferToBuyHouse:
						yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToBuyHouse);
						break;
					case Player.State.OfferToMortgageProperty:
						yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToMortgageProperty);
						break;
					case Player.State.OfferToRedeemProperty:
						yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToRedeemProperty);
						break;
					case Player.State.OfferToSellHouse:
						yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToSellHouse);
						break;
					case Player.State.EndTurn:
						_players.Enqueue(CurrentPlayer);
						CurrentPlayer = _players.Dequeue();
						CurrentPlayer.CurrentState = Player.State.StartTurn;
						break;
					case Player.State.Idle:
						if (DiceRoller.IsDouble())
							CurrentPlayer.CurrentState = Player.State.StartTurn;
						break;
				}
				if (CurrentPlayer.InJail && CurrentPlayer.CurrentState == Player.State.StartTurn)
					yield return StartCoroutine(JailManager.TurnInJail());
			}
			if(_state == States.Auction)
				yield return new WaitUntil((() => _state != States.Auction));
			if(_state == States.Trade)
				yield return new WaitUntil((() => _state != States.Trade));
			yield return null;
		}
	}

	public int NextLocation()
	{
		var currentLocation = Array.IndexOf(Board, CurrentPlayer.CurrentLocation);
		var nextLocation = currentLocation + DiceRoller.CurrentRoll();
		return nextLocation > 39 ? nextLocation - 40 : nextLocation;
	}
	public IEnumerator RollDice()
	{
		DiceRoller.RollDice();
		yield return StartCoroutine(CurrentPlayer.MoveTo(Board[NextLocation()]));
	}
	
	public void BuyProperty(Ownable property)
	{
		CurrentPlayer.BalanceManager.GetMoneyFromPlayer(property.PurchasePrice);
		CurrentPlayer.Owned.Add(property);
		property.SetOwner(CurrentPlayer);
		Debug.Log(CurrentPlayer.BalanceManager.Balance);
	}

	public void MortgageProperty(Ownable property)
	{
		CurrentPlayer.BalanceManager.GiveMoneyToPlayer(property.PurchasePrice/2);
		CurrentPlayer.Owned.Remove(property);
		CurrentPlayer.Mortgaged.Add(property);
		property.Mortgage();
		Debug.Log(CurrentPlayer.BalanceManager.Balance);
	}
	public void RedeemProperty(Ownable property)
	{
		CurrentPlayer.BalanceManager.GetMoneyFromPlayer((int)(property.PurchasePrice / 2 * 1.1f));
		CurrentPlayer.Owned.Add(property);
		CurrentPlayer.Mortgaged.Remove(property);
		property.UnMortgage();
		Debug.Log(CurrentPlayer.BalanceManager.Balance);
	}

	public bool CanPlayerUpgradeAnything()
	{
		var upgradeProperies = CurrentPlayer.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street)x).ToList()
			.FindAll(x=>x.CanUpgrade());
		return upgradeProperies.Count > 0;
	}

	public void BuyHouses(Street property, int numOfHouses)
	{
		CurrentPlayer.BalanceManager.GetMoneyFromPlayer(property.HousePrice * numOfHouses);
		property.BuildHouses(numOfHouses);
		Debug.Log(CurrentPlayer.BalanceManager.Balance);
	}

	public bool CanPlayerSellAnything()
	{
		return CurrentPlayer.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street)x).ToList()
			.FindAll(x=>x.CurrentUpgradeLevel > 0).Count > 0;
	}

	public void SellHouses(Street property, int numOfHouses)
	{
		CurrentPlayer.BalanceManager.GiveMoneyToPlayer((int) (property.HousePrice * numOfHouses / 2));
		property.SellHouse(numOfHouses);
		Debug.Log(CurrentPlayer.BalanceManager.Balance);
	}

	public IEnumerator StartAuction()
	{
		_state = States.Auction;
		yield return StartCoroutine(
			AuctionManager.StartAuction(CurrentPlayer.CurrentLocation as Ownable, CurrentPlayer));
		_state = States.Default;
	}
	public void GivePropertyToPlayer(Player player, Ownable property)
	{
		player.Owned.Add(property);
		property.SetOwner(player);
	}
	public void TransferPropertyBetweenPlayers(Player from, Player to, Ownable property)
	{
		from.Owned.Remove(property);
		to.Owned.Add(property);
		property.SetOwner(to);
		Debug.Log(string.Format("{0} передаётся от игрока {1} к игроку {2}",property.name, from.name, to.name));
	}

	public void TransfermortgagedPropertyBetweenPlayers(Player from, Player to, Ownable property)
	{
		from.Mortgaged.Remove(property);
		to.Mortgaged.Add(property);
		property.SetOwner(to);
		Debug.Log(string.Format("{0} передаётся от игрока {1} к игроку {2}",property.name, from.name, to.name));
	}
	public void BankruptByPlayer(Player bankruptPlayer, Player winPlayer)
	{
		var bankruptStreetsWithHouse = bankruptPlayer.Owned.FindAll(x => x.GetType() == typeof(Street))
			.Select(x => (Street) x).ToList()
			.FindAll(x => x.CurrentUpgradeLevel > 0);
		var amount = 0;
		foreach (var street in bankruptStreetsWithHouse)
		{
			amount += street.CurrentUpgradeLevel * street.HousePrice / 2;
			street.SellHouse(street.CurrentUpgradeLevel);
		}
		winPlayer.BalanceManager.GiveMoneyToPlayer(amount);
		foreach (var property in bankruptPlayer.Owned)
		{
			TransferPropertyBetweenPlayers(bankruptPlayer, winPlayer, property);
		}

		var amountToPay = 0;
		foreach (var property in bankruptPlayer.Mortgaged)
		{
			TransfermortgagedPropertyBetweenPlayers(bankruptPlayer, winPlayer, property);
			amountToPay += property.PurchasePrice / 10;
		}

		winPlayer.BalanceManager.GetMoneyFromPlayer(amount);
		//todo добавить удаление игрока
	}

	public IEnumerator BankruptByGame(Player bankruptPlayer)
	{
		foreach (var property in bankruptPlayer.Mortgaged)
		{
			property.UnMortgage();
			property.SetOwner(null);
		}
		var propertiesForAuction = new List<Ownable>(bankruptPlayer.Owned);
		foreach (var property in bankruptPlayer.Owned)
		{
			property.SetOwner(null);
		}

		foreach (var property in propertiesForAuction)
		{
			_state = States.Auction;
			yield return AuctionManager.StartAuction(property, bankruptPlayer);
			_state = States.Default;
		}
	}
}
