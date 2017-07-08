using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Force
{

	public Vector3 direction;


	public Force (Vector2 direction, float force)
	{
		if (force < SwipManager.Current.MinForce)
			force = SwipManager.Current.MinForce;
		if (force > SwipManager.Current.MaxForce)
			force = SwipManager.Current.MaxForce;		
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
