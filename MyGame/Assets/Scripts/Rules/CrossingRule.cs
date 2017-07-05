using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRule : Rule {
	
	List<Character> playerCharacters = new List<Character>();
	Character movingBall = null;
	List<Character> notMovingBalls = new List<Character>();
	bool BallCrossed = false;

	public CrossingRule() {				
		EventBus.Subscribe ("PlayerMoved", OnPlayerMoved);
	}

	void Start() {
		playerCharacters = PlayerManager.Current.GetPlayers ();
	}

	void OnPlayerMoved (object[] info)
	{
		Character character = (Character)info [0];
		if (character == null)
			return;		

		LogManager.Current.Log ("Moving Ball.");

		BallCrossed = false;


		this.notMovingBalls.Clear ();
		for (int i = 0; i < playerCharacters.Count; i++) {
			if (playerCharacters[i] != character) {
				this.notMovingBalls.Add (playerCharacters[i]);
			}
		}

		this.movingBall = character;

	}

	override public bool IsValid() {
		if (this.movingBall == null || this.notMovingBalls.Count != 2)
			return true;

		Vector3 startBall, endBall;

		startBall = this.notMovingBalls [0].transform.position;
		endBall = this.notMovingBalls [1].transform.position;

		RaycastHit hit;
		if (Physics.Linecast (startBall, endBall, out hit)) {
			Character hitBall = hit.transform.GetComponent<Character> ();
			if (hitBall == null) {
				if (movingBall.IsMoving)
					return true;				
			}
			if (hitBall == this.movingBall) {
				BallCrossed = true;
			}
		}

		if (!this.movingBall.IsMoving) {	
			EventBus.Post ("PlayerStopped", new object[]{ movingBall });
			return BallCrossed;
		} else {
			return true;
		}
	
	}

}
