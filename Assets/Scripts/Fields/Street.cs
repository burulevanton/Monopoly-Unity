using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Street : Ownable
{

	[SerializeField] private int _housePrice;
	[SerializeField] private int[] _rents;
	[SerializeField] private string _color;
	[SerializeField] private Street[] _streets_with_color;

	private int _currentUpgradeLevel;

	public void Awake()
	{
		_currentUpgradeLevel = 0;
	}
	
	protected override int Rent()
	{
		return _rents[_currentUpgradeLevel];
	}

	private bool canDouble()
	{
		foreach (var street in _streets_with_color)
		{
			if (!(Owner == street.Owner))
				return false;
			if (IsMortgage)
				return false;
			if (street._currentUpgradeLevel > 0)
				return false;
		}
		return true;
	}
}
