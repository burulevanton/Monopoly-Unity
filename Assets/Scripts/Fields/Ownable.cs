using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public abstract class Ownable : Field
{

	protected Player Owner;
	public float YOffset;
	public float XOffset;

	public Player Owner1
	{
		get { return Owner; }
	}

	private TextLog _textLog;
	public GameObject OwnerColor;
	
	public void SetOwner(Player player)
	{
		OwnerColor.SetActive(true);
		OwnerColor.GetComponent<Renderer>().material.color = player.Color;
		Owner = player;
	}

	protected GameManager GameManager;
	void Start()
	{
		GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		Owner = null;
		IsMortgage = false;
		_textLog = GameObject.Find("TextLog").GetComponent<TextLog>();
	}

	[SerializeField] private int _purchasePrice;
	public Sprite Image;

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
		_textLog.LogText(string.Format("{0} попал на поле {1}",player.PlayerName, this.Name));
		if (this.Owner == null)
		{
			UIManager uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
			uiManager.OfferBuyProperty();
		}
		else
		{
			if(this.Owner == player)
				_textLog.LogText(string.Format("{0} попал на своё поле",player.PlayerName, this.Name));
			else
			{
				if (!this.IsMortgage)
					player.BalanceManager.TransferMoneyToPlayer(Owner, Rent()); //todo улучшить лог
			}
		}
	}

	public abstract int Rent();

	public ImageBoardInfo ImageBoardInfo;

	private void OnMouseEnter()
	{
		MouseEnter();
	}

	private void OnMouseExit()
	{
		MouseExit();
	}

	public void MouseEnter()
	{
		if (ImageBoardInfo.isActiveAndEnabled && ImageBoardInfo.Property == this || !GameManager.GameInProgress ||
		    (GameManager.CurrentPlayer.CurrentState != Player.State.Idle &&
		     GameManager.CurrentPlayer.CurrentState != Player.State.StartTurn) ||
			GameManager.State == GameManager.States.Auction || GameManager.State == GameManager.States.Trade) return;
		ImageBoardInfo.transform.position = Input.mousePosition + new Vector3(XOffset, YOffset, 0);
		ImageBoardInfo.SetProperty(this);
		ImageBoardInfo.gameObject.SetActive(true);
	}

	public void MouseExit()
	{
		ImageBoardInfo.gameObject.SetActive(false);
	}
}
