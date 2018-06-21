using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHouse : MonoBehaviour
{

	public Slider Slider;
	public Button button;
	private bool _isPressed;
	private Action<Street, int> _action;

	public void setAction(Action<Street, int> action)
	{
		_action = action;
	}
	void Awake()
	{
		_isPressed = false;
		button.onClick.AddListener(ButtonPressed);
		button.onClick.AddListener(Accept);
	}

	void Accept()
	{
		_action(null, (int) Slider.value);
	}
	private void ButtonPressed()
	{
		_isPressed = true;
	}
	public IEnumerator CreateForm(int maxValue)
	{
		this.gameObject.SetActive(true);
		Slider.maxValue = maxValue;
		yield return new WaitUntil((() => _isPressed==true));
		_isPressed = false;
		this.gameObject.SetActive(false);
		Slider.value = 0;
	}
}
