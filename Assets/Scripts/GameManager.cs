using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{

	public Player[] Players;
	public Field[] Board;
	public GameObject[] Corners;
	public float speed = 1.0f;
	public float lastWaypointSwitchTime; 

	void Update()
	{
		foreach (var field in Board)
		{
			Players[0].MoveTo(field);
		}
	}
}
