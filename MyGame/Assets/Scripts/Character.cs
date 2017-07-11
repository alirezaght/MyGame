using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
	public int id = 0;
	public bool IsMoving = false;

	LimitedForceQueue path = new LimitedForceQueue (10);
	Rigidbody rigidBody;
	LayerMask LostGroundLayer;


	public abstract void DoSelectAnimation ();

	public abstract void DoDeSelectAnimation ();

	public abstract void DoStopAnimation ();

	public abstract void DoMoveAnimation ();


	public virtual void Awake ()
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
		if ((Character)info [0] == this) {
			IsMoving = true;
			DoMoveAnimation ();
		}
			
	}

	void OnPlayerStopped (object[] info)
	{
		if ((Character)info [0] == this) {
			DoStopAnimation ();
//			DoDeSelectAnimation ();
		}
	}

	void OnPlayerSelected (object[] info)
	{
		if ((Character)info [0] == this)
			DoSelectAnimation ();
		else
			DoDeSelectAnimation ();
	}

	void OnPlayerDeSelected (object[] info)
	{
		if ((Character)info [0] == this)
			DoDeSelectAnimation ();
	}

	public bool MoveWithVector (LimitedForceQueue path)
	{				
		if (path.SumOfForces < SwipManager.Current.MinForce)
			return false;		
		this.path.Clear ();
		this.path = path.Clone ();
		return Move (true);


	}

	bool Move (bool postEvent)
	{
		if (path.Count > 0) {
			Force force = path.Dequeue ();
//			Debug.Log (force.direction);
			//Log ("f = " + force + ", d = " + direction);
			Rigidbody rigidBody = GetRigidBody ();
			Vector3 direction = force.NormalizeForce (path.SumOfForces);
			rigidBody.AddRelativeForce (direction, ForceMode.Force);

			if (postEvent) {
				EventBus.Post ("PlayerMoved", new object[]{ this });
				EventBus.Trigger ("PlayerMoved_" + this.id);
				EventBus.Lock ("PlayerMoved");
				postEvent = false;
			} 
			return true;
		}
		return false;
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




	public virtual void FixedUpdate ()
	{
		Move (false);
	}


		

}
