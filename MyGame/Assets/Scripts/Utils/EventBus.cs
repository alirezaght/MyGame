using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus {
	public delegate void EventFunction(object[] info);
	public static Dictionary<string, int> triggers = new Dictionary<string, int>();

	static Dictionary<string, List<EventFunction>> subscriptions = new Dictionary<string, List<EventFunction>>();

	public static void Subscribe(string eventName, EventFunction func) {
		if (!subscriptions.ContainsKey (eventName)) {
			subscriptions.Add (eventName, new List<EventFunction> ());
		}
		subscriptions [eventName].Add (func);
	}
	public static void Post(string eventName, object[] info) {
		if (subscriptions.ContainsKey (eventName)) {
			foreach (EventFunction func in subscriptions[eventName]) {
				func (info);
			}
		}
	}
	public static void Trigger(string eventName) {
		triggers [eventName] = 1;
	}
	public static void Trigger(string eventName, int number) {
		triggers [eventName] = number;
	}
	public static bool Consume(string eventName) {
		var res = false;
		if (triggers.ContainsKey (eventName)) {
			triggers [eventName] -= 1; 
			res = triggers [eventName] >= 0;
		}
		return res;
	}
}
