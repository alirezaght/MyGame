using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;


public class SwipManager : SingletonBehaviour<SwipManager>
{
	public float ForceMultiplier;
	public float MaxForce;
	public float MinForce;
	public TrailRenderer trail;

	int ignoreWallMask;
	float swipeTime = 0;
	Vector3 startSwipePosition = Vector3.zero;
	bool isSwiping = false;
	Character selectedCharacter = null;
	LimitedForceQueue path = new LimitedForceQueue (10);

	void Awake ()
	{					
//		LeanTouch.OnFingerSwipe += OnFingerSwipe;
		Physics.gravity = new Vector3 (0, -200, 0);

		ignoreWallMask = ~(1 << LayerMask.NameToLayer ("Wall"));

	}
	// Use this for initialization
	void Start ()
	{
		Input.simulateMouseWithTouches = true;
	}


	// Update is called once per frame
	Vector3 lastMousePosition = Vector3.zero;

	void Update ()
	{		
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}

		if (PlayerManager.Current.GameEnded)
			return;

		if (Input.touchCount <= 0) {
			Touch fake = new Touch ();
			fake.fingerId = 10;
			fake.position = Input.mousePosition;

			fake.deltaTime = Time.deltaTime;
			if (lastMousePosition == Vector3.zero)
				lastMousePosition = Input.mousePosition;
			fake.deltaPosition = Input.mousePosition - lastMousePosition;
			lastMousePosition = Input.mousePosition;
			if (Input.GetMouseButtonDown (0)) {
				fake.phase = TouchPhase.Began;
				HandleTouch (fake);
			}
			if (Input.GetMouseButton (0)) {
				fake.phase = TouchPhase.Moved;
				HandleTouch (fake);
			}
			if (Input.GetMouseButtonUp (0)) {				
				fake.phase = TouchPhase.Ended;
				HandleTouch (fake);
			}
			return;
		}

		Touch touch = Input.GetTouch (0);
		HandleTouch (touch);

	}

	void HandleSwiping (Touch touch)
	{
		if (touch.phase == TouchPhase.Began) {
			swipeTime = Time.deltaTime;
			startSwipePosition = touch.position;
			isSwiping = true;
			path.Clear ();
			this.selectedCharacter = null;
			Ray ray = Camera.main.ScreenPointToRay (startSwipePosition);
			RaycastHit hit;
			if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, ignoreWallMask)) {
				this.selectedCharacter = hit.rigidbody.gameObject.GetComponent <Character> ();
			}
			return;
		}
		if (touch.phase == TouchPhase.Moved && isSwiping) {
			swipeTime += Time.deltaTime;

			Vector3 position = touch.position;
			//			Vector3 distance = position - startSwipePosition;
			Vector3 delta = new Vector3 (touch.deltaPosition.x / Screen.width, 0, touch.deltaPosition.y / Screen.height);
			float a = delta.magnitude / swipeTime;
			//float a = distance.magnitude / swipeTime;

			Force force = new Force (touch.deltaPosition, a * ForceMultiplier);
			path.Enqueue (force);			

			if (this.selectedCharacter != null) {				
				selectedCharacter.MoveWithVector (path);
				isSwiping = false;
			} else {
				//Log (touch.position.ToString());
				Ray ray = Camera.main.ScreenPointToRay (position);
				//Log (ray.ToString ());

				RaycastHit hit;
				if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, ignoreWallMask)) {
					Character character = hit.rigidbody.gameObject.GetComponent <Character> ();
					character.MoveWithVector (path);
					isSwiping = false;
				} else {
					//					Camera.main.transform.rotation = Quaternion.Euler (Camera.main.transform.eulerAngles.x, ((startSwipePosition.x - touch.position.x) * 180 / Screen.width), Camera.main.transform.eulerAngles.z);
				}
			}			
		}
		if (touch.phase == TouchPhase.Ended) {
			isSwiping = false;
			selectedCharacter = null;
			return;
		}
	}



	void HandleSelectAndSwiping (Touch touch)
	{
		if (EventBus.HasLock ("PlayerMoved"))
			return;
		
		if (touch.phase == TouchPhase.Began) {
			startSwipePosition = touch.position;
			path.Clear ();
			swipeTime = Time.deltaTime;
			isSwiping = true;
			if (selectedCharacter != null) {
				Ray ray = Camera.main.ScreenPointToRay (startSwipePosition);
				RaycastHit hit;
				if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, ignoreWallMask)) {							
					Character character = hit.rigidbody.gameObject.GetComponent<Character> ();
					if (character != selectedCharacter) {
						selectedCharacter = null;
						EventBus.Post ("PlayerDeSelected", new object[]{ selectedCharacter });
					}
				}
			}
			return;

		}

		Vector3 delta = new Vector3 (touch.deltaPosition.x / Screen.width, 0, touch.deltaPosition.y / Screen.height);
		float a = delta.magnitude / swipeTime;

		if (touch.phase == TouchPhase.Moved && isSwiping && selectedCharacter != null) {			
			swipeTime += Time.deltaTime;
			if (a > 0) {
				Force force = new Force (touch.deltaPosition, a * ForceMultiplier);
				path.Enqueue (force);
			}

			if (trail != null) {
				Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
				trail.transform.position = new Vector3 (pos.x, trail.transform.position.y, pos.z + 7);
			}
			return;
		}
		if (touch.phase == TouchPhase.Ended) {						
			isSwiping = false;
			if (selectedCharacter != null) {						
				Vector3 position = touch.position;
				if (a > 0) {
					Force force = new Force (touch.deltaPosition, a * ForceMultiplier);
					path.Enqueue (force);				
				}

				if (selectedCharacter.MoveWithVector (path)) {
					selectedCharacter = null;
					EventBus.Post ("PlayerDeSelected", new object[]{ selectedCharacter });
				}	

			} else {				
				Ray ray = Camera.main.ScreenPointToRay (startSwipePosition);
				RaycastHit hit;
				if (Physics.SphereCast (ray, touch.radius, out hit, float.PositiveInfinity, ignoreWallMask)) {							
					selectedCharacter = hit.rigidbody.gameObject.GetComponent<Character> ();
					EventBus.Post ("PlayerSelected", new object[]{ selectedCharacter });
					EventBus.Unlock ("PlayerMoved");
				}
			}

			return;
		}
	}

	void HandleTouch (Touch touch)
	{
		HandleSelectAndSwiping (touch);	
	}


	bool MoveWithVector (Character character, Vector3 direction, Vector3 hitPoint)
	{						
		var force = direction.magnitude * ForceMultiplier;
		if (force > MaxForce) {
			force = MaxForce;
		}
		if (force < MinForce)
			return false;
		direction = direction.normalized * force;	
		//Log ("f = " + force + ", d = " + direction);
		Rigidbody rigidBody = character.GetRigidBody ();
		if (hitPoint == Vector3.zero) {
			rigidBody.AddForce (direction);
		} else {
			rigidBody.AddForceAtPosition (direction, hitPoint);
		}
		EventBus.Post ("PlayerMoved", new object[]{ character });
		EventBus.Trigger ("PlayerMoved_" + character.id);
		EventBus.Lock ("PlayerMoved");
		return true;

	}




}
