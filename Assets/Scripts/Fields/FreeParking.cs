﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeParking : Field {
	
	public override void LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
	}
}
