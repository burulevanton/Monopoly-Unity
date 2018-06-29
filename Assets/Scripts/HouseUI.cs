using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseUI : MonoBehaviour
{

	public Street Street;
	public SpriteRenderer[] Spites;

	private void Update()
	{
		if (Street.CurrentUpgradeLevel > 0)
		{
			for (var i = 0; i < Street.CurrentUpgradeLevel; i++)
			{
				Spites[i].gameObject.SetActive(true);
				Spites[i].color = Street.Owner1.Color;
			}	
		}
		for (var i = Street.CurrentUpgradeLevel; i < Spites.Length; i++)
		{
			Spites[i].gameObject.SetActive(false);
		}
	}
}
