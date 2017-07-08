using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BallCharacter : Character
{

	LimitedForceQueue path = new LimitedForceQueue (10);
	float force = 0f;
	Renderer renderer;

	void Awake ()
	{
		base.Awake ();
		renderer = GetComponent<Renderer> ();
	}

	public override void DoSelectAnimation ()
	{
		this.renderer.material.color = Color.blue;
	}

	public override void DoDeSelectAnimation ()
	{
		this.renderer.material.color = Color.red;
		path.Clear ();
	}

	public override void DoStopAnimation ()
	{
		this.renderer.material.color = Color.red;
		path.Clear ();
	}

	public override void DoMoveAnimation ()
	{
		this.renderer.material.color = Color.green;
	}

	bool Move ()
	{
		if (path.Count > 0) {
			Force force = path.Dequeue ();

			//Log ("f = " + force + ", d = " + direction);
			Rigidbody rigidBody = GetRigidBody ();

			rigidBody.AddRelativeForce (force.NormalizeForce (path.SumOfForces), ForceMode.Force);

			EventBus.Post ("PlayerMoved", new object[]{ this });
			EventBus.Trigger ("PlayerMoved_" + this.id);
			EventBus.Lock ("PlayerMoved");
			return true;
		}
		return false;
	}

	public override bool MoveWithVector (LimitedForceQueue path)
	{				
		this.path.Clear ();
		this.path = path.Clone ();
		return Move ();

		
	}

	void FixedUpdate ()
	{
		Move ();
	}

}
