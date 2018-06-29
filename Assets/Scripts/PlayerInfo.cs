using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Text Name;
    public Text Balance;
    public Button StartTrade;
    private Player _player;
    public Image Panel;
    private GameManager _gameManager;
    public Image Jail;
    public Text Bankrupt;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        Balance.text = string.Format("Баланс - {0}Р", _player.BalanceManager.Balance);
        Jail.gameObject.SetActive(_player.InJail);
        Bankrupt.gameObject.SetActive(_player.CurrentState == Player.State.Bankrupt);
    }

    public void SetPlayer(Player player)
    {
        gameObject.SetActive(true);
        _player = player;
        Panel.color = player.Color;
        Name.text = player.PlayerName;
    }

    public void Trade()
    {
        _gameManager.TradeManager.StartTrade(_gameManager.CurrentPlayer, _player);
    }
    
}
