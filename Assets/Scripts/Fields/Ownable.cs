using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Ownable : Field
{

	protected Player Owner;

	public void SetOwner(Player player)
	{
		Owner = player;
	}

	void Awake()
	{
		Owner = null;
		IsMortgage = false;
	}

	[SerializeField] private int _purchasePrice;

	public int PurchasePrice
	{
		get { return _purchasePrice; }
	}

	protected bool IsMortgage;

	public void Mortgage()
	{
		IsMortgage = true;
	}

	public void UnMortgage()
	{
		IsMortgage = false;
	}

	public override void LandOn(Player player)
	{
		Debug.Log(string.Format("Вы попали на поле {0}",this.Name));
		if (this.Owner == null)
		{
			UIManager uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
			StartCoroutine(uiManager.OfferBuyProperty());
		}
		else
		{
			if(this.Owner == player)
				Debug.Log("Вы уже купили данное поле" + string.Format("текущая аренда {0}",Rent()));
			else
			{
				if (!this.IsMortgage)
					player.BalanceManager.TransferMoneyToPlayer(Owner, Rent()); //todo улучшить лог
			}
		}
	}

	protected abstract int Rent();
}
