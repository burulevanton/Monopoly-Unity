using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JailUI : MonoBehaviour
{

	public Text Text;
	public Button BuyButton;
	public Button RollButton;

	private bool _isPressed;
	
	void Awake()
	{
		BuyButton.onClick.AddListener(ButtonPressed);
		RollButton.onClick.AddListener(ButtonPressed);
		
	}

	void ButtonPressed()
	{
		_isPressed = true;
	}
	public IEnumerator WaitForPressed()
	{
		yield return new WaitUntil(() => _isPressed == true);
		this.gameObject.SetActive(false);
		this._isPressed = false;
	}
}
