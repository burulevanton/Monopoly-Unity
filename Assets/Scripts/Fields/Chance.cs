﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chance : Field {
	

	public override void LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
	}
}
