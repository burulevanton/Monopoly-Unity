using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToJail : Field
{
    private GameManager _gameManager;
    private TextLog _textLog;
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
    }
    public override void LandOn(Player player)
    {
        _textLog.LogText(string.Format("{0} попал на поле {1}",player.PlayerName, this.Name));
        _gameManager.JailManager.PutPLayerInJail();
    }
}
