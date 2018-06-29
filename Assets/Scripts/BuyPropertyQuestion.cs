using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuyPropertyQuestion : MonoBehaviour
{

	public Button Accept;
	public Button Decline;
	public Text Text;
	public Image Image;

	private GameManager _gameManager;
	

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void CreateForm()
	{
		var property = (Ownable) _gameManager.CurrentPlayer.CurrentLocation;
		Text.text = string.Format("Стоимость - {0}Р", property.PurchasePrice);
		Image.sprite = property.Image;
	}
	public void BuyProperty()
	{
		_gameManager.BuyProperty((Ownable)_gameManager.CurrentPlayer.CurrentLocation);
		gameObject.SetActive(false);
		if (_gameManager.CurrentPlayer.CurrentState != Player.State.Bankrupt)
		{
			_gameManager.CurrentPlayer.CurrentState = Player.State.Idle;
		}
	}
	public void StartAuction()
	{
		_gameManager.StartAuction();
		gameObject.SetActive(false);
		_gameManager.CurrentPlayer.CurrentState = Player.State.Idle;
	}
}
