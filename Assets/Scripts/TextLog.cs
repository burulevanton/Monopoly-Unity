using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public class TextLog : MonoBehaviour
{

	public GameObject Text;

	public void LogText(string info)
	{
		GameObject text = Instantiate(Text) as GameObject;
		text.SetActive(true);

		text.GetComponent<Text>().text = info;
		text.transform.SetParent(Text.transform.parent);
	}
}
