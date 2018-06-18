using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : Ownable
{

	[SerializeField] private int _housePrice;
	[SerializeField] private int[] _rents;

	private int _currentUpgradeLevel;

	public void Awake()
	{
		_currentUpgradeLevel = 0;
	}
	
	protected override int Rent()
	{
		return _rents[_currentUpgradeLevel];
	}
}
