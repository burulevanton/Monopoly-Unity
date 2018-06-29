using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public InputField[] Inputs;
	public GameObject[] Players;
	public Button[] AddPlayersButtons;
	private GameManager _gameManager;

	private void AddPlayer(int i)
	{
		AddPlayersButtons[i-2].gameObject.SetActive(false);
		Players[i].SetActive(true);
	}
	private void Awake()
	{
		AddPlayersButtons[0].onClick.AddListener((() => AddPlayer(2)));
		AddPlayersButtons[1].onClick.AddListener((() => AddPlayer(3)));
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void StartGame()
	{
		for (int i = 0; i < Players.Length; i++)
		{
			if (Players[i].active)
			{
				_gameManager.SetPlayer(i, Inputs[i].text);
			}
		}
		_gameManager.InitComponents();
		gameObject.SetActive(false);
	}
}
