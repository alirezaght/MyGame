﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force
{

	public Vector3 direction;


	public Force (Vector2 direction, float force)
	{				
		this.direction = direction.normalized * force;
		this.direction = new Vector3 (this.direction.x, 0, this.direction.y);
	}

	public Vector3 NormalizeForce (float max)
	{
		if (max > SwipManager.Current.MaxForce)
			return direction.normalized * direction.magnitude * SwipManager.Current.MaxForce / max;
		else
			return direction;
	}


}
