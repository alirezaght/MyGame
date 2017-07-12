using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

	public void GoToLevel (int levelNo)
	{
		switch (levelNo) {
		case 0:
			SceneManager.LoadScene ("SelectLevelScene");
			break;
		case 1:
			SceneManager.LoadScene ("Level1");
			break;
		case 2:
			SceneManager.LoadScene ("Level2");
			break;
		default:
			SceneManager.LoadScene ("SelectLevelScene");
			break;
		}

	}

	public void GoToMain(){
		SceneManager.LoadScene ("MainScene");
	}
}
