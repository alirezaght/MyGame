using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class BallCharacter : Character
{

	public AudioClip HitAudio;
	public AudioClip BounceAudio;



	ParticleSystem particleSystem;
	AudioSource audioSource;
	new MeshRenderer renderer;
	bool rangeIncrease = true;
	ParticleSystem.MinMaxGradient startColor;

	public override void Awake ()
	{
		base.Awake ();
		renderer = GetComponent<MeshRenderer> ();
		audioSource = GetComponent<AudioSource> ();
		EventBus.Subscribe ("HitWall", OnHitWall);
		EventBus.Subscribe ("HitPlayer", OnHitPlayer);
		particleSystem = GetComponentInChildren<ParticleSystem> ();
		startColor = particleSystem.colorOverLifetime.color;
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
		if (particleSystem != null) {			
			var overtime = particleSystem.colorOverLifetime;
			overtime.color = new ParticleSystem.MinMaxGradient (Color.red);

		}
	}

	public override void DoDeSelectAnimation ()
	{
		DoStopAnimation ();
	}

	public override void DoStopAnimation ()
	{
		if (particleSystem != null) {	
			var overtime = particleSystem.colorOverLifetime;
			overtime.color = startColor;

		}
	}

	public override void DoMoveAnimation ()
	{
		if (particleSystem != null) {
			var overtime = particleSystem.colorOverLifetime;
			overtime.color = new ParticleSystem.MinMaxGradient (Color.green);
		}
		PlayHit ();
	}




}
