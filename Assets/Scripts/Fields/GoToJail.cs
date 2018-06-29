using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToJail : Field
{
    private GameManager _gameManager;
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public override void LandOn(Player player)
    {
        _gameManager.TextLog.LogText(string.Format("Вы попали на поле {0}",this.Name));
        _gameManager.JailManager.PutPLayerInJail();
    }
}
