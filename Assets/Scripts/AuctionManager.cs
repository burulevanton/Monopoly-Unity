using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionManager : MonoBehaviour
{

    private int _maxBid;
    private Ownable _property;
    private List<Player> _activePlayers;
    private List<Player> _removedPlayers;
    private GameManager _gameManager;
    private Player _currentPlayer;
    private bool _isPressed;

    public Text Offer;
    public Text MaxBid;
    public InputField Input;
    public Button Accept;
    public Button Exit;
    
    //TODO добавить возникновение
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Accept.onClick.AddListener(ChangeBid);
        Exit.onClick.AddListener(RemovePlayer);
        _activePlayers = new List<Player>();
        _removedPlayers = new List<Player>();
        _isPressed = false;
    }

    public IEnumerator StartAuction(Ownable property, Player abandonedPlayer)
    {
        _property = property;
        _maxBid = property.PurchasePrice;
        MaxBid.text = string.Format("Текущая стоимость: {0}Р", _maxBid);
        foreach (var player in _gameManager.Players)
        {
            if(!player.Equals(abandonedPlayer))
            _activePlayers.Add(player);
        }
        yield return StartCoroutine(StartBidding());
    }

    public void RemovePlayer()
    {
        _removedPlayers.Add(_currentPlayer);
        _isPressed = true;
    }

    public void ChangeBid()
    {
        var bid = Convert.ToInt32(Input.text);
        if (bid > _maxBid)
        {
            _maxBid = bid;
            MaxBid.text = string.Format("Текущая стоимость: {0}Р", _maxBid);
        }

        _isPressed = true;
    }

    private void RemovePlayers()
    {
        foreach (var player in _removedPlayers)
        {
            _activePlayers.Remove(player);
        }
        _removedPlayers.Clear();
    }
    public void ChangeOffer()
    {
        Offer.text = string.Format("Игрок {0} введите вашу ставку за данное поле", _currentPlayer.PlayerName);
    }

    public IEnumerator StartBidding()
    {
        while (_activePlayers.Count > 1)
        {
            foreach (var player in _activePlayers)
            {
                _currentPlayer = player;
                ChangeOffer();
                Input.text = "";
                yield return new WaitUntil(()=>_isPressed==true);
                _isPressed = false;
            }
            RemovePlayers();
        }

        Debug.Log(_activePlayers.Count > 0
            ? string.Format("Победитель аукциона - {0}", _activePlayers[0].PlayerName)
            : "Все игроки отказались от покупки");
    }
    
}
