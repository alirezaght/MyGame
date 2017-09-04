using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePassingRule : Rule {

	int nextGate = 1;
	bool passRightGate = true;


	public GatePassingRule() {
		EventBus.Subscribe ("GateEntered", OnGateEntered);
	}
	public void OnGateEntered(object[] info) {
		Debug.Log ("Gate Entered.");
		GameObject gate = ((Collision)info [0]).gameObject;
		GateCollidor gateCollidor = gate.GetComponent<GateCollidor> ();

		if (gateCollidor != null) {
			Debug.Log ("Gate Collidor is not null");
			Debug.Log ("Gate no = " + gateCollidor.GateNo);
			Debug.Log ("next gate = " + nextGate);
			if (nextGate > gateCollidor.GateNo) {
				Debug.Log ("passed gate.");
				return;
			}
			if (nextGate == gateCollidor.GateNo) {
				Debug.Log ("go to the next gate");
				passRightGate = true;
				nextGate++;
				return;
			}
		}

		passRightGate = false;
	}
	public override bool IsValid ()
	{
		return passRightGate;
	}
}
