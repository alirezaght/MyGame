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
	List<Gate> gates = new List<Gate> ();
	List<Rule> rules = new List<Rule> ();


	public List<Character> GetPlayers ()
	{
		return playerCharacters;
	}

	public List<Gate> GetGates ()
	{
		return gates;
	}

	void Awake ()
	{		
		foreach (Character obj in FindObjectsOfType (typeof(Character))) {						
			playerCharacters.Add (obj);
		}
		foreach (Rule rule in FindObjectsOfType(typeof(Rule))) {
			rules.Add (rule);
		}
		foreach (Gate obj in FindObjectsOfType (typeof(Gate))) {						
			gates.Add (obj);
		}

	}



	void Update ()
	{
		if (GameEnded)
			return;
		
		foreach (Rule rule in rules) {
			if (rule.isActiveAndEnabled)
			if (!rule.IsValid ()) {
				if (rule.ShouldLoseIfNotValid)
					Lost (rule);
			} else {
				if (rule.DidWinTheGame)
					Win (rule);
			}

		}

	}

	void Lost (Rule rule)
	{
//		GameEnded = true;
//		LostAnimator.SetTrigger("Lost");

		LogManager.Current.Log ("You Lost By rule = " + rule.ToString ());
	}

	void Win (Rule rule)
	{
		//		GameEnded = true;
		//		LostAnimator.SetTrigger("Lost");

		LogManager.Current.Log ("You Won ");
	}

	public void TryAgain ()
	{
		GameEnded = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}



}
