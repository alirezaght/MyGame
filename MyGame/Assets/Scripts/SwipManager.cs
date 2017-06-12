using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using Lean.Touch;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SwipManager : MonoBehaviour
{

	public float ForceMultiplier = 10.0f;
	public float MaxForce = 3000f;
	public Text debugText;
	public GameObject[] players;
	int playerMask;
	int wallMask;

	void Awake ()
	{					
		LeanTouch.OnFingerSwipe += OnFingerSwipe;
		playerMask = ~(1 << LayerMask.NameToLayer("Player"));
		wallMask = ~(1 << LayerMask.NameToLayer("Wall"));
	}
	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	void Log(String msg) {
		for (int i = 0; i < players.Length; i++) {			
			msg += " player" + i + "=" + players[i].transform.position;
		}
		Debug.Log (msg);
		if (debugText != null) 
			debugText.text = msg;
	}
	long count = 0;
	void OnFingerSwipe (LeanFinger finger)
	{			
		Log ("Swipe " + count);
		var startPoint = finger.GetStartRay().origin;
		var endpoint = finger.GetRay().origin;
		var direction = (endpoint - startPoint);
		var hit = default(RaycastHit);
		if (Physics.Raycast (new Vector3 (startPoint.x, startPoint.y, -10f), new Vector3 (0f, 0f, 1f), out hit, float.PositiveInfinity, wallMask) == true) {
			Log ("Hit");
			var rigidbody = hit.rigidbody;
			var hitobject = hit.collider.gameObject;
			// Is the rigidbody attached to this GameObject?
			if (rigidbody != null) {
				foreach (GameObject player in players) {
					if (rigidbody.gameObject == player) {
						Log ("Move" + count);
						MoveWithVector (rigidbody, finger.SwipeScaledDelta);
					}
				}
				// Add force to the rigidbody based on the swipe force
			}
		} else {
			
			if (Physics.SphereCast (new Vector3 (startPoint.x, startPoint.y, 0f), 1f, new Vector3 (direction.normalized.x, direction.normalized.y, 0f), out hit, direction.magnitude, wallMask) == true) {
				Log ("Hit");
				var rigidbody = hit.rigidbody;
				var hitobject = hit.collider.gameObject;
				// Is the rigidbody attached to this GameObject?
				if (rigidbody != null) {
					foreach (GameObject player in players) {
						if (rigidbody.gameObject == player) {
							Log ("Move" + count);
							MoveWithVector (rigidbody, finger.SwipeScaledDelta);
						}
					}
					// Add force to the rigidbody based on the swipe force
				}
			}
		}
		count++;
	}


	void MoveWithVector (Rigidbody rigidBody, Vector2 direction)
	{				
		var force = direction.magnitude * ForceMultiplier;
		if (force > MaxForce) {
			force = MaxForce;
		}
		direction = direction.normalized * force;
		rigidBody.AddForce (direction);

	}




}
