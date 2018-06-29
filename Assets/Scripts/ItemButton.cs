using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

	public Ownable property; // идентификатор объекта, который храниться в списке
	public Button mainButton; // основная кнопка, на который будет отображено название объекта
	public Text mainButtonText; // текст главной кнопки
	public Image Image;

	private void Update()
	{
//		if (_isMouseEnter)
//		{
//			if (Image.IsActive()) return;
//			Image.transform.position = Input.mousePosition + new Vector3(200, 0, 0);
//			Image.sprite = property.Image;
//			Image.gameObject.SetActive(true);
//		}
//		else
//		{
//			Image.gameObject.SetActive(false);
//		}
	}

	public void MouseEnter()
	{
		if (Image.IsActive()) return;
		Image.transform.position = Input.mousePosition + new Vector3(200, 0, 0);
		Image.sprite = property.Image;
		Image.gameObject.SetActive(true);
	}

	public void MouseExit()
	{
		Image.gameObject.SetActive(false);
	}

	public void MouseClick()
	{
		Image.gameObject.SetActive(false);
	}
}
