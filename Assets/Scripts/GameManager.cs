using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
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
	public TextLog TextLog;
	public bool GameInProgress = false;
	private Queue<Player> _players;
	private UIManager _uIManager;
	public List<Player> ActivePlayers;

	public enum States
	{
		Default,
		Auction,
		Trade
	};

	
	public States State { get; set; }

	private void Start()
	{
		_uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
		
	}

	public void SetPlayer(int i, string name)
	{
		Players[i].gameObject.SetActive(true);
		ActivePlayers.Add(Players[i]);
		Players[i].PlayerName = name;
	}

	public void InitComponents()
	{
		ChanceCards = new Queue<int>();
		PublicTreasuryCards = new Queue<int>();
		for (var i = 0; i < 14; i++)
		{
			ChanceCards.Enqueue(i);
			PublicTreasuryCards.Enqueue(i);
		}
		State = States.Default;
		_players = new Queue<Player>();
		foreach (var player in ActivePlayers)
		{
			player.CurrentLocation = Board[0];
			_players.Enqueue(player);
		}
		CurrentPlayer = _players.Dequeue();
		GameInProgress = true;
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
//			GivePropertyToPlayer(CurrentPlayer,Board[12] as Ownable);
//			GivePropertyToPlayer(CurrentPlayer,Board[13] as Ownable);
//			GivePropertyToPlayer(CurrentPlayer,Board[14] as Ownable);
//			GivePropertyToPlayer(CurrentPlayer,Board[15] as Ownable);
//			GivePropertyToPlayer(CurrentPlayer,Board[16] as Ownable);
//			GivePropertyToPlayer(CurrentPlayer,Board[18] as Ownable);
//			GivePropertyToPlayer(CurrentPlayer,Board[19] as Ownable);
//			_state = States.Trade;
//			TradeManager.StartTrade(Players[0], Players[1]);
//		}
	}

	private IEnumerator Game()
	{
		//todo отправка в тюрьму после больше 3 раз дублей
//		yield return StartCoroutine(current_player.MoveTo(Board[1]));
//		yield return new WaitForSeconds(2);
//		yield return StartCoroutine(current_player.MoveTo(Board[3]));
//		yield return new WaitForSeconds(2);
		//yield return StartCoroutine(BankruptByGame(Players[0]));
//		GivePropertyToPlayer(CurrentPlayer,Board[12] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer,Board[13] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer,Board[14] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer,Board[15] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer,Board[16] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer,Board[18] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer,Board[19] as Ownable);
		//CurrentPlayer.BalanceManager.GetMoneyFromPlayer(1500);
//		ActivePlayers[1].BalanceManager.GetMoneyFromPlayer(1500);
//		ActivePlayers[2].BalanceManager.GetMoneyFromPlayer(1500);
//		GivePropertyToPlayer(CurrentPlayer, Board[1] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[3] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[6] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[5] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[8] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[9] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[11] as Ownable);
//		GivePropertyToPlayer(CurrentPlayer, Board[12] as Ownable);
		yield return StartCoroutine(_uIManager.PlayerInfoManager.SetPlayerInfo());
		yield return StartCoroutine(_uIManager.StartTurn());
		while (GameInProgress)
		{
			switch (State)
			{
				case States.Default:
					switch (CurrentPlayer.CurrentState)
					{
//						case Player.State.Moving:
//							yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.Moving);
//							break;
//						case Player.State.OfferToBuyProperty:
//							yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToBuyProperty);
//							Debug.Log("САНЯ ХУЙ СОСИ");
//							break;
//						case Player.State.OfferToBuyHouse:
//							yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToBuyHouse);
//							break;
//						case Player.State.OfferToMortgageProperty:
//							yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToMortgageProperty);
//							break;
//						case Player.State.OfferToRedeemProperty:
//							yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToRedeemProperty);
//							break;
//						case Player.State.OfferToSellHouse:
//							yield return new WaitUntil(() => CurrentPlayer.CurrentState!=Player.State.OfferToSellHouse);
//							break;
						case Player.State.EndTurn:
							yield return StartCoroutine(_uIManager.PlayerInfoManager.EndTurn());
							_players.Enqueue(CurrentPlayer);
							CurrentPlayer = _players.Dequeue();
							CurrentPlayer.CurrentState = Player.State.StartTurn;
							yield return StartCoroutine(_uIManager.StartTurn());
							break;
						case Player.State.Idle:
							if (DiceRoller.IsDouble())
							{
								if (DiceRoller.DoublesInARow == 3)
								{
									JailManager.PutPLayerInJail();
									DiceRoller.DoublesInARow = 0;
								}
								else
								{
									CurrentPlayer.CurrentState = Player.State.StartTurn;
								}
							}

							break;
						case Player.State.Bankrupt:
							yield return StartCoroutine(_uIManager.PlayerInfoManager.EndTurn());
							CurrentPlayer = _players.Dequeue();
							CurrentPlayer.CurrentState = Player.State.StartTurn;
							if (_players.Count==0)
							{
								GameInProgress = false;
								_uIManager.EndGame();
							}
							else
								yield return StartCoroutine(_uIManager.StartTurn());
							break;
					}

					if (CurrentPlayer.InJail && CurrentPlayer.CurrentState == Player.State.StartTurn)
						yield return StartCoroutine(JailManager.TurnInJail());
					break;
				case States.Auction:
					yield return new WaitWhile(() => State == States.Auction);
					break;
				case States.Trade:
					yield return new WaitWhile(() => State == States.Trade);
					break;
			}

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
		TextLog.LogText(string.Format("{0} выбросил {1} и {2}",CurrentPlayer.PlayerName, DiceRoller.Dice1, DiceRoller.Dice2));
		yield return StartCoroutine(CurrentPlayer.MoveTo(Board[NextLocation()]));
	}
	
	public void BuyProperty(Ownable property)
	{
		if (!CurrentPlayer.BalanceManager.GetMoneyFromPlayer(property.PurchasePrice)) return;
		TextLog.LogText(string.Format("{0} покупает {1}",CurrentPlayer.PlayerName, property.name));
		CurrentPlayer.Owned.Add(property);
		property.SetOwner(CurrentPlayer);
	}

	public void MortgageProperty(Ownable property)
	{
		CurrentPlayer.BalanceManager.GiveMoneyToPlayer(property.PurchasePrice/2);
		CurrentPlayer.Owned.Remove(property);
		CurrentPlayer.Mortgaged.Add(property);
		property.Mortgage();
		TextLog.LogText(string.Format("{0} закладывает {1}",CurrentPlayer.PlayerName, property.name));
	}
	public void RedeemProperty(Ownable property)
	{
		if(!CurrentPlayer.BalanceManager.GetMoneyFromPlayer((int)(property.PurchasePrice / 2 * 1.1f))) return;
		CurrentPlayer.Owned.Add(property);
		CurrentPlayer.Mortgaged.Remove(property);
		property.UnMortgage();
		TextLog.LogText(string.Format("{0} выкупает {1}",CurrentPlayer.PlayerName, property.name));
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
		if (!CurrentPlayer.BalanceManager.GetMoneyFromPlayer(property.HousePrice * numOfHouses)) return;
		property.BuildHouses(numOfHouses);
		TextLog.LogText(string.Format("{0} покупает {1} дом(а) на {2}",CurrentPlayer.PlayerName,numOfHouses, property.name));
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
		TextLog.LogText(string.Format("{0} продаёт {1} дом(а) на {2}",CurrentPlayer.PlayerName,numOfHouses, property.name));
	}

	public void StartAuction()
	{
		State = States.Auction;
		TextLog.LogText(string.Format("{0} отказался от покупки, начинается аукцион",CurrentPlayer.PlayerName));
		StartCoroutine(AuctionManager.StartAuction((Ownable)CurrentPlayer.CurrentLocation, CurrentPlayer));
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
		TextLog.LogText(string.Format("{0} передаётся от игрока {1} к игроку {2}",property.name, from.PlayerName, to.PlayerName));
	}

	public void TransfermortgagedPropertyBetweenPlayers(Player from, Player to, Ownable property)
	{
		from.Mortgaged.Remove(property);
		to.Mortgaged.Add(property);
		property.SetOwner(to);
		TextLog.LogText(string.Format("{0} передаётся от игрока {1} к игроку {2}",property.name, from.PlayerName, to.PlayerName));
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
		TextLog.LogText(string.Format("{0} обанкротил игрока {1}", winPlayer.PlayerName, bankruptPlayer.PlayerName));
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
			State = States.Auction;
			yield return AuctionManager.StartAuction(property, bankruptPlayer);
			State = States.Default;
		}
		TextLog.LogText(string.Format("Банк обанкротил игрока {0}", bankruptPlayer.PlayerName));
	}
}
