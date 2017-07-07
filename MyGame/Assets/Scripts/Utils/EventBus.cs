using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class EventBus
{
	public delegate void EventFunction (object[] info);

	protected static Dictionary<string, int[]> triggers = new Dictionary<string, int[]> ();
	protected static Dictionary<string, object> globals = new Dictionary<string, object> ();
	protected static Dictionary<string, List<EventFunction>> subscriptions = new Dictionary<string, List<EventFunction>> ();
	protected static HashSet<string> lockSet = new HashSet<string> ();


	public static void Subscribe (string eventName, EventFunction func)
	{
		if (!subscriptions.ContainsKey (eventName)) {
			subscriptions.Add (eventName, new List<EventFunction> ());
		}
		subscriptions [eventName].Add (func);
	}

	public static void Post (string eventName, object[] info)
	{
		if (subscriptions.ContainsKey (eventName)) {
			foreach (EventFunction func in subscriptions[eventName]) {
				func (info);
			}
		}
	}

	public static void Trigger (string eventName)
	{
		triggers [eventName] = new int[]{ 1, Time.frameCount };
	}

	public static void Trigger (string eventName, int number)
	{
		triggers [eventName] = new int[]{ number, Time.frameCount };
	}

	public static bool Consume (string eventName)
	{
		return Consume (eventName, false);
	}

	public static bool Consume (string eventName, bool onlyAfterFrameCount)
	{		
		
		if (triggers.ContainsKey (eventName)) {
			int[] tuple = triggers [eventName];
			int frameCount = Time.frameCount;
			if (tuple [1] < frameCount || !onlyAfterFrameCount) {
				tuple [0] -= 1; 
				bool res = tuple [0] >= 0;
				if (!res)
					triggers.Remove (eventName);
				return res;
			} else {
				return true;
			}
		} 
		return false;


	}

	public static void SetGlobal (string key, object value)
	{
		globals [key] = value;
	}

	public static object GetGlobal (string key)
	{
		return globals [key];
	}

	[MethodImpl (MethodImplOptions.Synchronized)]
	public static bool Lock (string name)
	{
		if (lockSet.Contains (name))
			return false;
		lockSet.Add (name);
		return true;
	}

	[MethodImpl (MethodImplOptions.Synchronized)]
	public static void Unlock (string name)
	{
		lockSet.Remove (name);
	}

	public static bool HasLock (string name)
	{
		return lockSet.Contains (name);
	}
}
