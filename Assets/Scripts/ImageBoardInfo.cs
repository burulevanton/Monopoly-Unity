using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBoardInfo : MonoBehaviour
{
	public Text Owner;
	public Text Rent;
	public Image Image;
	public Ownable Property;

	public void SetProperty(Ownable property)
	{
		Property = property;
		if (property.Owner1 != null)
		{
			Owner.text = string.Format("Владелец - {0}", property.Owner1.PlayerName);
			Rent.text = string.Format("Текущая аренда - {0}", property.Rent());
		}
		else
		{
			Owner.text = "Нет владельца";
			Rent.text = string.Format("Минимальная аренда - {0}", property.Rent());
		}
		Image.sprite = Property.Image;
	}
}
