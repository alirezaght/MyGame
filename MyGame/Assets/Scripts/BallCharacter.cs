using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BallCharacter : Character
{

	Queue<Vector3> path = new Queue <Vector3> ();
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
			Vector3 direction = path.Dequeue ();
			direction.y = 0;
			if (path.Count > 0)
				force /= path.Count;

			direction = direction.normalized * force;	
			//Log ("f = " + force + ", d = " + direction);
			Rigidbody rigidBody = GetRigidBody ();

			rigidBody.AddRelativeForce (direction, ForceMode.Force);

			EventBus.Post ("PlayerMoved", new object[]{ this });
			EventBus.Trigger ("PlayerMoved_" + this.id);
			EventBus.Lock ("PlayerMoved");
			return true;
		}
		return false;
	}

	public override bool MoveWithVector (float force, Queue<Vector3> path)
	{
		force *= SwipManager.Current.ForceMultiplier;
		if (force > SwipManager.Current.MaxForce) {
			force = SwipManager.Current.MaxForce;
		}
		if (force < SwipManager.Current.MinForce)
			return false;
		this.force = force;
		this.path = path;

		return Move ();

		
	}

	void Update ()
	{
		Move ();
	}

}
