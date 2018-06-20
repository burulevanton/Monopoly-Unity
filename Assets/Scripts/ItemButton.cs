using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

	public Ownable property; // идентификатор объекта, который храниться в списке
	public Button mainButton; // основная кнопка, на который будет отображено название объекта
	public Text mainButtonText; // текст главной кнопки
}
