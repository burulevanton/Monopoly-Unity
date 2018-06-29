using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public class TextLog : MonoBehaviour
{

	public GameObject Text;
	public Scrollbar Scrollbar;
	public ScrollRect ScrollRect;

	public void LogText(string info)
	{
		ScrollRect.normalizedPosition = new Vector2(ScrollRect.normalizedPosition.y, 0.0f);
		GameObject text = Instantiate(Text) as GameObject;
		text.SetActive(true);

		text.GetComponent<Text>().text = info;
		text.transform.SetParent(Text.transform.parent);
		//Scrollbar.value = 0f;
		ScrollRect.normalizedPosition = new Vector2(ScrollRect.normalizedPosition.y, 0.0f);
	}
}
