using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MortgageProperty : MonoBehaviour
{

	public ScrollRect Scroll;
	public GameObject ChooseProperty;
	public GameObject Question;
	public Text Info;
	public RectTransform Button;

	private Ownable _choosenProperty;
	private ItemList _itemList;
	private GameManager _gameManager;
	private Player _player;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void Exit()
	{
		_itemList.Clear();
		Destroy(_itemList);
		this.gameObject.SetActive(false);
		_player.CurrentState = Player.State.Idle;
	}

	public void Accept()
	{
		_gameManager.MortgageProperty(_choosenProperty);
		Question.gameObject.SetActive(false);
		_itemList.Clear();
		Destroy(_itemList);
		ShowChooseForm(_player);
	}

	public void Decline()
	{
		_itemList.Clear();
		Destroy(_itemList);
		Question.gameObject.SetActive(false);
		ShowChooseForm(_player);
	}

	private void ChoosePropertyByPlayer(Ownable property)
	{
		_choosenProperty = property;
		ChooseProperty.SetActive(false);
		Question.gameObject.SetActive(true);
		ShowQuestion();
	}

	private void InitScroll()
	{
		_itemList = gameObject.AddComponent<ItemList>();
		_itemList.scroll = Scroll;
		_itemList.element = Button;
		_itemList.SetAction(ChoosePropertyByPlayer, true);
	}
	public void ShowChooseForm(Player player)
	{
		ChooseProperty.SetActive(true);
		_player = player;
		_player.CurrentState = Player.State.OfferToMortgageProperty;
		InitScroll();
		foreach (var property in _player.Owned)
		{
			_itemList.AddToList(property, true);
		}
	}

	private void ShowQuestion()
	{
		Info.text = string.Format("Вы получите - {0}Р", _choosenProperty.PurchasePrice / 2);
	}
}
