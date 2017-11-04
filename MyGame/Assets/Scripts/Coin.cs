using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : NPC
{
	public bool GotThrough = false;

	public double GiftAmount = 10;

	LayerMask PlayerLayer;
	Renderer renderer;

	void Awake ()
	{
		PlayerLayer = LayerMask.NameToLayer ("Player");
		renderer = GetComponent<Renderer> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (GotThrough) {
			return;
		}
		if (other.gameObject.layer == PlayerLayer) {
			GotThrough = true;
		}
	}

	void Update ()
	{
		if (GotThrough) {
			renderer.enabled = false;
		}
	}
}
