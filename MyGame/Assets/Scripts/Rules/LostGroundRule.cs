using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostGroundRule : Rule {

	bool lostGroundCollision = false;

	public LostGroundRule() {
		EventBus.Subscribe ("LostGround", OnLostGround);
	}
	public void OnLostGround(object[] info) {
		lostGroundCollision = true;
	}
	public override bool IsValid ()
	{
		return !lostGroundCollision;
	}

	void Update() {
		// IMPORTANT: should have this function to see enable/disable checkbox in unity editor inspector
	}


}
