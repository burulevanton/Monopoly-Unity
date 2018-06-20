using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToJail : Field {
    public override void LandOn(Player player)
    {
        Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
        Field[] Board = GameObject.Find("GameManager").GetComponent<GameManager>().Board;
        player.GoToJail();
        StartCoroutine(player.MoveTo(Board[10]));
    }
}
