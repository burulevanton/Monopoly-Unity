using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jail : Field {
    public override void LandOn(Player player)
    {
        if(!player.InJail)
            Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
    }
}
