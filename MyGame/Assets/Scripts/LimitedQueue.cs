using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

public class LimitedForceQueue
{
	Queue<Force> queue = new Queue<Force> ();

	public float SumOfForces = 0f;

	public int Count {
		get {
			return queue.Count;
		}
	}

	int capacity;

	public int Capacity {
		get {
			return capacity;
		}
	}

	public LimitedForceQueue (int capacity)
	{
		this.capacity = capacity;
	}

	public void Enqueue (Force item)
	{
		while (queue.Count > Capacity - 1)
			Dequeue ();
		SumOfForces += item.direction.magnitude;
		queue.Enqueue (item);
	}

	public Force Dequeue ()
	{
		Force tmp = queue.Dequeue ();
		if (tmp != null)
			SumOfForces -= tmp.direction.magnitude;
		return tmp;
	}

	public LimitedForceQueue Clone ()
	{
		LimitedForceQueue tmp = new LimitedForceQueue (Capacity);
		tmp.queue = new Queue<Force> (queue);
		tmp.SumOfForces = SumOfForces;
		return tmp;
	}

	public void Clear ()
	{
		queue.Clear ();
	}


}
