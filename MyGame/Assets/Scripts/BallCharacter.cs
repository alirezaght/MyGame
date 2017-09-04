using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class BallCharacter : Character
{

	public AudioClip HitAudio;
	public AudioClip BounceAudio;




	AudioSource audioSource;
	new MeshRenderer meshRenderer;
	bool rangeIncrease = true;

	public override void Awake ()
	{
		base.Awake ();
		meshRenderer = GetComponent<MeshRenderer> ();
		audioSource = GetComponent<AudioSource> ();
		EventBus.Subscribe ("HitWall", OnHitWall);
		EventBus.Subscribe ("HitPlayer", OnHitPlayer);

	}

	void OnHitWall (object[] info)
	{
		if ((BallCharacter)info [0] == this) {
			PlayBounce ();
		}
	}

	void OnHitPlayer (object[] info)
	{
		if ((BallCharacter)info [0] == this) {
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
		meshRenderer.material.SetColor ("_EmissionColor", Color.black);
	}

	public override void DoDeSelectAnimation ()
	{
		DoStopAnimation ();
	}

	public override void DoStopAnimation ()
	{
		meshRenderer.material.SetColor ("_EmissionColor", Color.red);
	}

	public override void DoMoveAnimation ()
	{
		meshRenderer.material.SetColor ("_EmissionColor", Color.green);
		PlayHit ();
	}




}
