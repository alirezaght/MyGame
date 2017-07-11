﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BallCharacter : Character
{


	new Renderer renderer;


	public override void Awake ()
	{
		base.Awake ();
		renderer = GetComponent<Renderer> ();
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
	}




}