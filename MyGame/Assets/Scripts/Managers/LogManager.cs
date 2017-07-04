using UnityEngine;
using UnityEngine.UI;
using System;

public class LogManager : SingletonBehaviour<LogManager> {
	
	public Text debugText;

	public void Log(String msg) {		
		if (debugText != null) 
			debugText.text = msg;
	}
}
