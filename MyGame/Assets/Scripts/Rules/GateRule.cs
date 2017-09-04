using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class GateRule : Rule
{
	


	public GateRule ()
	{				
		
	}





	override public bool IsValid ()
	{
		var gates = PlayerManager.Current.GetGates ();
		gates.Sort ();
		var win = true;
		var pointer = -1;
		foreach (Gate g in gates) {
			if (g.GotThrough) {
				if (pointer == g.id - 1) {
					pointer = g.id;
				} else {
					return false;
				}
			} else {
				win = false;

			}

		}
		DidWinTheGame = win;
		return true;
	
	}

}
