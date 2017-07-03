using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public int BallNo = 0;

	public bool IsMoving = false;

	private Rigidbody rigidBody;

	void Awake ()
	{
		rigidBody = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		if (rigidBody == null)
			return;
		if (IsMoving) {
			Vector3 velocity = new Vector3 (rigidBody.velocity.x, 0, rigidBody.velocity.z);
			if (velocity.magnitude <= 0.2f) {
				this.IsMoving = false;
				SwipManager.Current.Log ("ball stopped.");
				//SwipManager.Current.Log (rigidBody.velocity.magnitude.ToString ());
				return;
			}

			//SwipManager.Current.Log ("ball Moving.");
		}
	}
}
