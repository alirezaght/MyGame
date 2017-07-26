﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class BallCharacter : Character
{

	public AudioClip HitAudio;
	public AudioClip BounceAudio;


	Light GlowLight;

	AudioSource audioSource;
	new Renderer renderer;
	bool rangeIncrease = true;

	public override void Awake ()
	{
		base.Awake ();
		renderer = GetComponent<Renderer> ();
		audioSource = GetComponent<AudioSource> ();
		EventBus.Subscribe ("HitWall", OnHitWall);
		EventBus.Subscribe ("HitPlayer", OnHitPlayer);
		GlowLight = GetComponentInChildren<Light> ();
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
		audioSource.PlayOneShot (BounceAudio);
	}

	void PlayHit ()
	{
		audioSource.PlayOneShot (HitAudio);
	}

	public override void DoSelectAnimation ()
	{
		if (GlowLight != null) {
			GlowLight.intensity = 20;
			GlowLight.color = Color.red;
		}
	}

	public override void DoDeSelectAnimation ()
	{
		if (GlowLight != null) {
			GlowLight.intensity = 5;
			GlowLight.color = Color.yellow;
		}
	}

	public override void DoStopAnimation ()
	{
		if (GlowLight != null) {
			GlowLight.intensity = 7;
			GlowLight.color = Color.yellow;
		}
	}

	public override void DoMoveAnimation ()
	{
		if (GlowLight != null) {
			GlowLight.intensity = 10;
			GlowLight.color = Color.green;
		}
		PlayHit ();
	}

	void Update ()
	{
		if (GlowLight != null) {
			if (rangeIncrease)
				GlowLight.range += UnityEngine.Random.Range (0.5f, 1f) * Time.deltaTime * 10;
			else
				GlowLight.range -= UnityEngine.Random.Range (0.01f, 0.5f) * Time.deltaTime * 7;
			if (GlowLight.range >= 7) {
				rangeIncrease = false;
			}
			if (GlowLight.range <= 3) {
				rangeIncrease = true;
			}

		}
	}


}
