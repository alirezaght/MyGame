using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SwipManager : MonoBehaviour
{
	public GameObject DummyPrefab;
	private GameObject activeDummy;

	public float ForceMultiplier = 10.0f;
	public float MaxForce = 3000f;
	public Text debugText;
	public GameObject[] players;
	int playerMask;
	int wallMask;

	float swipeTime = 0;
	Vector2 startSwipePosition = Vector2.zero;
	bool isSwiping = false;

	void Awake ()
	{					
//		LeanTouch.OnFingerSwipe += OnFingerSwipe;
		playerMask = ~(1 << LayerMask.NameToLayer("Player"));
		wallMask = ~(1 << LayerMask.NameToLayer("Wall"));
	}
	// Use this for initialization
	void Start ()
	{
		
	}

	Rigidbody beganHitRigidBody = null;
	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount <= 0) {
			return;
		}

		Touch touch = Input.GetTouch (0);
	
		Log (touch.position.ToString ());
		if (touch.phase == TouchPhase.Began) {
			activeDummy = GameObject.Instantiate (DummyPrefab);
			activeDummy.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,0));

			return;
			swipeTime = 0;
			startSwipePosition = touch.position;
			isSwiping = true;

			this.beganHitRigidBody = null;
			Ray ray = Camera.main.ScreenPointToRay (touch.position);
			RaycastHit hit;
			if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, wallMask)) {
				this.beganHitRigidBody = hit.rigidbody;
			}
			return;
		}
		if (touch.phase == TouchPhase.Moved && activeDummy != null) {
			activeDummy.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,0));
			return;
		}
		if (touch.phase == TouchPhase.Ended && activeDummy != null) {
			Destroy (activeDummy);
			activeDummy = null;
		}

		return;
		if (touch.phase == TouchPhase.Moved && isSwiping) {
			swipeTime += Time.deltaTime;

			if (this.beganHitRigidBody != null) {
				Vector2 distance = touch.position - startSwipePosition;
				float a = distance.magnitude * 5 / (float)(Math.Pow (swipeTime, 2));

				MoveWithVector (this.beganHitRigidBody, distance.normalized * a, Vector3.zero);
				isSwiping = false;
			} else {
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, wallMask)) {
					Vector2 distance = touch.position - startSwipePosition;
					float a = distance.magnitude * 5 / (float)(Math.Pow (swipeTime, 2));
					 
					MoveWithVector (hit.rigidbody, distance.normalized * a, Camera.main.WorldToScreenPoint(hit.point));
					isSwiping = false;
				}
			}
			
		}
		if (touch.phase == TouchPhase.Ended) {
			isSwiping = false;
			return;
		}
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


	void MoveWithVector (Rigidbody rigidBody, Vector2 direction, Vector3 hitPoint)
	{						
		var force = direction.magnitude * ForceMultiplier;
		if (force > MaxForce) {
			force = MaxForce;
		}
		direction = direction.normalized * force;	
		if (hitPoint == Vector3.zero) {
			rigidBody.AddForce (direction);
		} else {
			rigidBody.AddForceAtPosition (direction, hitPoint);
		}

	}




}
