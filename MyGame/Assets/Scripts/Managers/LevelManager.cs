﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

	public void GoToLevel (int levelNo)
	{
		string sceneName = levelNo == 0 ? "SelectLevelScene" : "Level" + levelNo.ToString ();
		Scene scene = SceneManager.GetSceneByName (sceneName);
		if (scene.IsValid ()) {
			SceneManager.LoadScene (sceneName);
		} else {
			Debug.Log ("Coming soon ...");	
		}
	}

	public void GoToMain ()
	{
		SceneManager.LoadScene ("MainScene");
	}
}
