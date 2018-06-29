using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeParking : Field
{

	private TextLog _textLog;

	private void Awake()
	{
		_textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
	}

	public override void LandOn(Player player)
	{
		_textLog.LogText(string.Format("{0} попал на поле {1}",player.PlayerName, this.Name));
	}
}
