using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
	public int id = 0;
	public bool IsMoving = false;


	Rigidbody rigidBody;
	LayerMask LostGroundLayer;


	public abstract void DoSelectAnimation ();

	public abstract void DoDeSelectAnimation ();

	public abstract void DoStopAnimation ();

	public abstract void DoMoveAnimation ();

	public abstract bool MoveWithVector (float force, Queue<Vector3> path);

	public void Awake ()
	{
		rigidBody = GetComponent<Rigidbody> ();
		LostGroundLayer = LayerMask.NameToLayer ("LostGround");
		EventBus.Subscribe ("PlayerMoved", OnPlayerMoved);
		EventBus.Subscribe ("PlayerStopped", OnPlayerStopped);
		EventBus.Subscribe ("PlayerSelected", OnPlayerSelected);
		EventBus.Subscribe ("PlayerDeSelected", OnPlayerDeSelected);
	}

	public Rigidbody GetRigidBody ()
	{
		return rigidBody;
	}

	void OnPlayerMoved (object[] info)
	{
		if (info [0] == this) {
			IsMoving = true;
			DoMoveAnimation ();
		}
			
	}

	void OnPlayerStopped (object[] info)
	{
		if (info [0] == this) {
			DoStopAnimation ();
//			DoDeSelectAnimation ();
		}
	}

	void OnPlayerSelected (object[] info)
	{
		if (info [0] == this)
			DoSelectAnimation ();
		else
			DoDeSelectAnimation ();
	}

	void OnPlayerDeSelected (object[] info)
	{
		if (info [0] == this)
			DoDeSelectAnimation ();
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.layer == LostGroundLayer) {
			EventBus.Post ("LostGround", new object[]{ this, other });
		}
	}

	void LateUpdate ()
	{	
		if (IsMoving) {	
			Vector3 velocity = new Vector3 (rigidBody.velocity.x, 0, rigidBody.velocity.z);
			if (velocity.magnitude > 0.06) {
				IsMoving = true;
			} else {
			
				if (!EventBus.Consume ("PlayerMoved_" + id, true)) {
					EventBus.Post ("PlayerStopped", new object[]{ this });
					EventBus.Unlock ("PlayerMoved");
					IsMoving = false;
				}
			
			}
		}

	}


		

}
