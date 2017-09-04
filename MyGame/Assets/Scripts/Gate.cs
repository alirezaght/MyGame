using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gate : NPC
{
	public bool GotThrough = false;
	public ParticleSystem innerGate;


	LayerMask PlayerLayer;

	void Awake ()
	{
		PlayerLayer = LayerMask.NameToLayer ("Player");
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.layer == PlayerLayer) {
			GotThrough = true;
			var colorOverLifetime = innerGate.colorOverLifetime;
			var sizeOverLifetime = innerGate.sizeOverLifetime;
			var main = innerGate.main;
			var startLifetime = main.startLifetime;
			colorOverLifetime.enabled = false;
			sizeOverLifetime.enabled = false;
			startLifetime.constant = int.MaxValue;
			startLifetime.constantMin = int.MaxValue;
			startLifetime.constantMax = int.MaxValue;
			main.startLifetimeMultiplier = int.MaxValue;
		}
	}

}
