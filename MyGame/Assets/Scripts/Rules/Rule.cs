using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rule : MonoBehaviour
{
	public bool ShouldLoseIfNotValid = true;

	public abstract bool IsValid ();

	public bool DidWinTheGame = false;

}
