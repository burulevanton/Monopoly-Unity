using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuyPropertyQuestion : MonoBehaviour
{

	public Button Accept;
	public Button Decline;
	public bool isPressed;

	void Awake()
	{
		Accept.onClick.AddListener(ButtonPressed);
		Decline.onClick.AddListener(ButtonPressed);
		
	}

	void ButtonPressed()
	{
		isPressed = true;
	}
	public IEnumerator WaitForPressed()
	{
		yield return new WaitUntil(() => isPressed == true);
		
		this.gameObject.SetActive(false);
		this.isPressed = false;
	}
}
