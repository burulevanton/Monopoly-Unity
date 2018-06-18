using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : Ownable
{

	[SerializeField] private Utility[] _otherUtilities;

	protected override int Rent()
	{
		throw new System.NotImplementedException();
	}
}
