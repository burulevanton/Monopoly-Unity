using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

	public GameObject[] Players;
	public GameObject[] Corners;
	public float speed = 1.0f;
	public float lastWaypointSwitchTime; 

	void Start()
	{
		lastWaypointSwitchTime = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{
//		Vector3 startPosition = Players[0].transform.position;
//		Vector3 destination = new Vector3(-0.479f, -3.5f, 0f);
//		float pathLength = Vector3.Distance(startPosition, destination);
//		float totalTimeForPath = pathLength / speed;
//		float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
//		Players[0].transform.position = Vector2.Lerp(startPosition, destination, currentTimeOnPath / totalTimeForPath);
	}

	void Awake()
	{
		Players[0].transform.DOMove(new Vector3(-1.856f, 2.054f, 0f), 4f);
	}
}
