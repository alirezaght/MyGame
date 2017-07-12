using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class BallCharacter : Character
{

	public AudioSource HitAudio;
	public AudioSource BounceAudio;

	new Renderer renderer;


	public override void Awake ()
	{
		base.Awake ();
		renderer = GetComponent<Renderer> ();
		EventBus.Subscribe ("HitWall", OnHitWall);
		EventBus.Subscribe ("HitPlayer", OnHitPlayer);
	}

	void OnHitWall (object[] info)
	{
		if (info [0] == this) {
			PlayBounce ();
		}
	}

	void OnHitPlayer (object[] info)
	{
		if (info [0] == this) {
			PlayHit ();
		}
	}

	void PlayBounce ()
	{
		BounceAudio.Play ();
	}

	void PlayHit ()
	{
		HitAudio.Play ();
	}

	public override void DoSelectAnimation ()
	{
		this.renderer.material.color = Color.blue;
	}

	public override void DoDeSelectAnimation ()
	{
		this.renderer.material.color = Color.red;
	}

	public override void DoStopAnimation ()
	{
		this.renderer.material.color = Color.red;
	}

	public override void DoMoveAnimation ()
	{
		this.renderer.material.color = Color.green;
		PlayHit ();
	}




}
