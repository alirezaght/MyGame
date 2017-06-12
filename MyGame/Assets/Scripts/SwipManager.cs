using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using Lean.Touch;

public class SwipManager : MonoBehaviour
{

	public float ForceMultiplier = 10.0f;
	public float MaxForce = 2600f;


	Rigidbody rigidBody;

	void Awake ()
	{				
		rigidBody = GetComponent<Rigidbody> ();
		LeanTouch.OnFingerSwipe += OnFingerSwipe;
	}
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnFingerSwipe (LeanFinger finger)
	{				
		var ray = finger.GetStartRay(Camera.main);

		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit)) {
			return;
		}
			


		// Get the rigidbody component
		var rigidbody = hit.rigidbody;

		// Is the rigidbody attached to this GameObject?
		if (rigidbody != null && rigidbody.gameObject == gameObject) {
			// Add force to the rigidbody based on the swipe force
			MoveWithVector(finger.SwipeScaledDelta);

		}

	}


	void MoveWithVector (Vector2 direction)
	{				
		var force = direction.magnitude * ForceMultiplier;
		if (force > MaxForce) {
			force = MaxForce;
		}
		direction = direction.normalized * force;
		rigidBody.AddForce (direction);

	}

	void OnCollisionEnter (Collision collision)
	{		
		if (collision.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			var contact = collision.contacts [0];
			var velocity = Vector3.Normalize (rigidBody.velocity + transform.position) * rigidBody.velocity.magnitude;
			var n = Vector3.Normalize (contact.normal);
			velocity = 2 * (Vector3.Dot (velocity, n)) * n - velocity; 
			velocity *= -1; 
			rigidBody.AddForce (velocity, ForceMode.Impulse);

		}
					 
	}


}
