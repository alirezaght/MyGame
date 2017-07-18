using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : SingletonBehaviour<CameraManager>
{

	public float CenterThreshold;
	public float FieldViewThreshold;
	public float FieldViewFactor;

	public float MinFieldView;
	public float MaxFieldView;


	List<Character> players = new List<Character> ();


	void Start ()
	{
		players = PlayerManager.Current.GetPlayers ();
	}

	Vector3 CenterOfPlayers ()
	{		
		Vector3 average = Vector3.zero;
		for (int i = 0; i < players.Count; i++) {
			average += players [i].gameObject.transform.position;
		}
		return average / players.Count;
	}


	float MaxDistanceOfPlayers ()
	{		
		float max = 0;
		for (int i = 0; i < players.Count; i++) {
			for (int j = i; j < players.Count; j++) {
				max = Math.Max (max, (players [j].transform.position - players [i].transform.position).magnitude);
			}
		}
		return max;
	}
		
	
	// Update is called once per frame
	void Update ()
	{
		var center = CenterOfPlayers ();
		var maxDistance = MaxDistanceOfPlayers ();
		var cameraPos = new Vector3 (Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
		if ((cameraPos - center).magnitude > CenterThreshold)
			MoveCamera (center);

		if (Camera.main.orthographicSize < FieldViewFactor * maxDistance - FieldViewThreshold) {			
			ZoomCamera (maxDistance);
		} else if (Camera.main.orthographicSize > FieldViewFactor * maxDistance + FieldViewThreshold) {
			ZoomCamera (maxDistance);
		}

	}

	void ZoomCamera (float maxDistance)
	{
		var newValue = Mathf.Clamp (FieldViewFactor * maxDistance, MinFieldView, MaxFieldView);
		Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, newValue, 1 * Time.deltaTime);			
	}

	void MoveCamera (Vector3 position)
	{
		var pos = new Vector3 (position.x, Camera.main.transform.position.y, position.z);
		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, pos, 1f * Time.deltaTime);
	}
}
