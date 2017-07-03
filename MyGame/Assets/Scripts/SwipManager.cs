﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SwipManager : MonoBehaviour
{

	public float ForceMultiplier;
	public float MaxForce;
	public float MinForce;
	public Text debugText;
	int ignorePlayerMask;
	int ignoreWallMask;

	float swipeTime = 0;
	Vector3 startSwipePosition = Vector2.zero;
	bool isSwiping = false;

	float screenGhotr  = 1f;

	void Awake ()
	{					
//		LeanTouch.OnFingerSwipe += OnFingerSwipe;
		Physics.gravity = new Vector3(0, -200, 0);
		ignorePlayerMask = ~(1 << LayerMask.NameToLayer("Player"));
		ignoreWallMask = ~(1 << LayerMask.NameToLayer("Wall"));
		screenGhotr = (float)Math.Sqrt (Math.Pow (Screen.width, 2) + Math.Pow (Screen.height, 2));
	}
	// Use this for initialization
	void Start ()
	{
		
	}

	Rigidbody beganHitRigidBody = null;
	// Update is called once per frame
	Vector3 lastMousePosition = Vector3.zero;
	void Update ()
	{
		if (Input.GetMouseButton(0) ) {
			Touch fakeTouch = new Touch();
			fakeTouch.fingerId = 10;
			fakeTouch.position = Input.mousePosition;
			fakeTouch.deltaTime = Time.deltaTime;
			if (lastMousePosition == Vector3.zero)
				lastMousePosition = Input.mousePosition;
			fakeTouch.deltaPosition = Input.mousePosition - lastMousePosition;
			lastMousePosition = Input.mousePosition;
			fakeTouch.phase =    (Input.GetMouseButtonDown(0) ? TouchPhase.Began : 
				(fakeTouch.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary) );
			fakeTouch.tapCount = 1;

			HandleTouch(fakeTouch);
		}

		if (Input.touchCount <= 0) {
			return;
		}

		Touch touch = Input.GetTouch (0);
		HandleTouch (touch);

	}
	void HandleTouch(Touch touch) {
		if (touch.phase == TouchPhase.Began) {
			swipeTime = 0f;
			startSwipePosition = touch.position;
			isSwiping = true;

			this.beganHitRigidBody = null;
			Ray ray = Camera.main.ScreenPointToRay (startSwipePosition);
			RaycastHit hit;
			if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, ignoreWallMask)) {
				this.beganHitRigidBody = hit.rigidbody;
			}
			return;
		}
		if (touch.phase == TouchPhase.Moved && isSwiping) {
			swipeTime += Time.deltaTime;
			Vector3 position = touch.position;
			Vector3 distance = position - startSwipePosition;
			Vector3 delta = new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y);
			float a = 2f * touch.deltaPosition.magnitude / (float)Math.Pow(touch.deltaTime, 2);
			//float a = distance.magnitude / swipeTime;


			if (this.beganHitRigidBody != null) {				
				MoveWithVector (this.beganHitRigidBody, delta.normalized * a * 1.2f, Vector3.zero);
				isSwiping = false;
			} else {
				Log (touch.position.ToString());
				Ray ray = Camera.main.ScreenPointToRay (position);
				Log (ray.ToString ());

				RaycastHit hit;
				if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, ignoreWallMask)) {
					MoveWithVector (hit.rigidbody, delta.normalized * a, hit.point);
					isSwiping = false;
				} else {
//					Camera.main.transform.rotation = Quaternion.Euler (Camera.main.transform.eulerAngles.x, ((startSwipePosition.x - touch.position.x) * 180 / Screen.width), Camera.main.transform.eulerAngles.z);
				}
			}			
		}
		if (touch.phase == TouchPhase.Ended) {
			isSwiping = false;
			beganHitRigidBody = null;
			return;
		}
	}
	void Log(String msg) {		
		Debug.Log (msg);
		if (debugText != null) 
			debugText.text = msg;
	}
	long count = 0;
//	void OnFingerSwipe (LeanFinger finger)
//	{			
//		Log ("Swipe " + count);
//		var startPoint = finger.GetStartRay().origin;
//		var endpoint = finger.GetRay().origin;
//		var direction = (endpoint - startPoint);
//		var hit = default(RaycastHit);
//		if (Physics.Raycast (new Vector3 (startPoint.x, startPoint.y, -10f), new Vector3 (0f, 0f, 1f), out hit, float.PositiveInfinity, wallMask) == true) {
//			Log ("Hit");
//			var rigidbody = hit.rigidbody;
//			var hitobject = hit.collider.gameObject;
//			// Is the rigidbody attached to this GameObject?
//			if (rigidbody != null) {
//				foreach (GameObject player in players) {
//					if (rigidbody.gameObject == player) {
//						Log ("Move" + count);
//						MoveWithVector (rigidbody, finger.SwipeScaledDelta);
//					}
//				}
//				// Add force to the rigidbody based on the swipe force
//			}
//		} else {
//			
//			if (Physics.SphereCast (new Vector3 (startPoint.x, startPoint.y, 0f), 1f, new Vector3 (direction.normalized.x, direction.normalized.y, 0f), out hit, direction.magnitude, wallMask) == true) {
//				Log ("Hit");
//				var rigidbody = hit.rigidbody;
//				var hitobject = hit.collider.gameObject;
//				// Is the rigidbody attached to this GameObject?
//				if (rigidbody != null) {
//					foreach (GameObject player in players) {
//						if (rigidbody.gameObject == player) {
//							Log ("Move" + count);
//							MoveWithVector (rigidbody, finger.SwipeScaledDelta);
//						}
//					}
//					// Add force to the rigidbody based on the swipe force
//				}
//			}
//		}
//		count++;
//	}


	void MoveWithVector (Rigidbody rigidBody, Vector3 direction, Vector3 hitPoint)
	{						
		var force = direction.magnitude * ForceMultiplier;
		if (force > MaxForce) {
			force = MaxForce;
		}
		if (force < MinForce)
			force = MinForce;
		direction = direction.normalized * force;	
		Log ("f = " + force + ", d = " + direction);
		if (hitPoint == Vector3.zero) {
			rigidBody.AddForce (direction);
		} else {
			rigidBody.AddForceAtPosition (direction, hitPoint);
		}

	}




}
