using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NPC : MonoBehaviour, IComparable
{
	public int id = 0;

	int IComparable.CompareTo (object obj)
	{
		var other = (NPC)obj;
		return id.CompareTo (other.id);
	}
}
