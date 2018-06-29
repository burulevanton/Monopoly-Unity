using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go : Field {
	
	private TextLog _textLog;

	private void Awake()
	{
		_textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
	}

	public override void LandOn(Player player)
	{
		_textLog.LogText(string.Format("Вы попали на поле {0}",this.Name));
	}
}
