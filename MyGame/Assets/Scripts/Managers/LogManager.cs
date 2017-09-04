using UnityEngine;
using UnityEngine.UI;
using System;

public class LogManager : SingletonBehaviour<LogManager>
{
	
	public Text debugText;

	public void Log (String msg)
	{
		Debug.Log (msg);
		if (debugText != null)
			debugText.text = msg;
	}
}
