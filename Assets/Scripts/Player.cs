using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string PlayerName { get; set; }

	private int _accountBalance = 1500;

	public int AccountBalance
	{
		get { return _accountBalance; }
		set
		{
			_accountBalance = value;
			//TODO добавить что-то ещё
		}
	}

	public Field CurrentLocation { get; set; }

	public IEnumerator MoveTo(Field location)
	{
		Vector3 startPosition = CurrentLocation.transform.position;
		Vector3 endPosition = location.transform.position;
		float pathLength = Vector2.Distance(startPosition, endPosition);
		float totalTimeForPath = pathLength / 1f;
		float lastSwitchTime = Time.time;
		while (transform.position != location.transform.position)
		{
			float currentTimeOnPath = Time.time - lastSwitchTime;
			transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
			yield return null;
		}

		yield return new WaitForSeconds(2f);
		yield return location.LandOn(this);
	}
}
