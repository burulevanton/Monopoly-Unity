using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Street : Ownable
{

	[SerializeField] private int _housePrice;
	[SerializeField] private int[] _rents;
	[SerializeField] private string _color;
	[SerializeField] private List<Street> _streetsWithColor;

	public int HousePrice
	{
		get { return _housePrice; }
	}

	private int _currentUpgradeLevel;

	public int CurrentUpgradeLevel
	{
		get { return _currentUpgradeLevel; }
	}

	public void Awake()
	{
		_currentUpgradeLevel = 0;
		_streetsWithColor.Add(this);
	}
	
	public override int Rent()
	{
		return _rents[_currentUpgradeLevel] * (CanDouble() ? 2 : 1);
	}

	private bool CanDouble()
	{
		foreach (var street in _streetsWithColor)
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
	public bool CanUpgrade()
	{
		foreach (var street in _streetsWithColor)
		{
			if (!(Owner == street.Owner))
				return false;
			if (street.IsMortgage)
				return false;
		}

		return _currentUpgradeLevel <= 3;
	}

	public int MaxHouseCanBeBuild() //TODO ориентация на деньги игрока
	{
		return 4 - _currentUpgradeLevel;
	}

	public void BuildHouses(int numOfHouses)
	{
		_currentUpgradeLevel += numOfHouses;
	}

	public void SellHouse(int numOfHouses)
	{
		_currentUpgradeLevel -= numOfHouses;
	}
}
