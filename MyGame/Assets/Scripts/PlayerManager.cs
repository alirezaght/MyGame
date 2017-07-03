using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public GameObject[] playerGameObjects;
	private Ball[] playerBalls;

	private GameObject movingBallGameObject = null;
	private Ball movingBall = null;
	private List<Ball> notMovingBalls = new List<Ball>();

	public bool BallCrossed = false;

	public static PlayerManager Current;
	public PlayerManager(){
		Current = this;
	}

	void Start ()
	{
		playerBalls = new Ball[playerGameObjects.Length];
		if (playerGameObjects.Length == 3) {
			for (int i = 0; i < 3; i++) {
				if (playerGameObjects [i] != null) {
					playerBalls [i] = playerGameObjects[i].GetComponent<Ball> ();
				}
			}
		}
	}


	void Update ()
	{
		if (this.movingBallGameObject == null || this.movingBall == null || this.notMovingBalls.Count != 2)
			return;

		if (!this.movingBall.IsMoving) {
			if (BallCrossed) {
				this.movingBallGameObject = null;
				this.movingBall = null;		
				notMovingBalls.Clear();
			} else {
				//player lost
				SwipManager.Current.Log("Player Lost...!");
				this.movingBallGameObject = null;
				this.movingBall = null;		
				notMovingBalls.Clear();
			}
			return;
		}

		Vector3 startBall, endBall;

		startBall = this.notMovingBalls [0].transform.position;
		endBall = this.notMovingBalls [1].transform.position;

		RaycastHit hit;
		if (Physics.Linecast (startBall, endBall, out hit)) {
			Ball hitBall = hit.transform.GetComponent<Ball> ();
			if (hitBall == null)
				return;
			if (hitBall.gameObject == this.movingBallGameObject) {
				BallCrossed = true;
				SwipManager.Current.Log ("Crossed.");
			}
		}
	}


	public void MoveBall (GameObject ball)
	{
		if (ball == null)
			return;
		Ball currentBall = ball.GetComponent<Ball> ();
		if (currentBall == null)
			return;

		SwipManager.Current.Log ("Moving Ball.");

		BallCrossed = false;
		currentBall.IsMoving = true;

		this.notMovingBalls.Clear ();
		for (int i = 0; i < 3; i++) {
			if (playerGameObjects[i] != ball) {
				this.notMovingBalls.Add (playerBalls[i]);
			}
		}

		this.movingBallGameObject = ball;
		this.movingBall = currentBall;

	}



}
