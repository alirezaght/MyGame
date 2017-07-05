using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
	public int id = 0;
	public bool IsMoving {
		get {
			Rigidbody rigidBody = GetRigidBody ();
			Vector3 velocity = new Vector3 (rigidBody.velocity.x, 0, rigidBody.velocity.z);
			if (EventBus.Consume ("PlayerMoved_" + id))
				return true;
			else return velocity.magnitude > 0.06f;
		}
	}


	Rigidbody rigidBody;
	LayerMask LostGroundLayer;

	void Awake ()
	{
		rigidBody = GetComponent<Rigidbody> ();
		LostGroundLayer = LayerMask.NameToLayer ("LostGround");
	}

	public Rigidbody GetRigidBody() {
		return rigidBody;
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == LostGroundLayer) {
			EventBus.Post ("LostGround", new object[]{this, other});
		}
	}


}
