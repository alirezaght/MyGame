using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public void Start(){
		//Canvas.ForceUpdateCanvases ();
	}
	public void GoToLevel (int levelNo)
	{
		//Load by scene name
		/*
		string sceneName = levelNo == 0 ? "SelectLevelScene" : "Level" + levelNo.ToString();
		Scene scene = SceneManager.GetSceneByName (sceneName);
		if (scene.IsValid()) {
			SceneManager.LoadScene (sceneName);
		} else {
			Debug.Log ("Coming soon ...");	
		}
		*/

		//MainScene = 0
		//LevelScene = 1
		//Level1 = 2 
		//,...
		SceneManager.LoadScene (levelNo + 1);
	}

	public void GoToMain(){
		SceneManager.LoadScene ("MainScene");
	}
}
