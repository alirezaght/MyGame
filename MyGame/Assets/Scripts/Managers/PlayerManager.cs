using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{

	public Animator LostAnimator;
	public bool GameEnded = false;

	List<Character> playerCharacters = new List<Character> ();
	List<Rule> rules = new List<Rule> ();


	public List<Character> GetPlayers ()
	{
		return playerCharacters;
	}

	void Awake ()
	{
		var objects = FindObjectsOfType (typeof(Character));
		foreach (Character obj in objects) {						
			playerCharacters.Add (obj);
		}
		foreach (Rule rule in FindObjectsOfType(typeof(Rule))) {
			rules.Add (rule);
		}

	}



	void Update ()
	{
		foreach (Rule rule in rules) {
			if (rule.isActiveAndEnabled)
			if (!rule.IsValid ())
			if (rule.ShouldLoseIfNotValid)
				Lost ();
		}


	}

	void Lost ()
	{
//		GameEnded = true;
//		LostAnimator.SetTrigger("Lost");
		LogManager.Current.Log ("You Lost ");
	}

	public void TryAgain ()
	{
		GameEnded = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}



}
