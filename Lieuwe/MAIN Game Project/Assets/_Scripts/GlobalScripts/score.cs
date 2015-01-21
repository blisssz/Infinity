using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class score : MonoBehaviour {
	public static int gameScore;
	public static bool restart;
	public static int enemiesKilled;
	public static int platformsVisited;
	public Text thisScore;
	// Use this for initialization
	void Start () {
		thisScore = GetComponent<Text> ();
		gameScore = 0;
		thisScore.text = "Score: 0";
	}
	
	// Update is called once per frame
	void Update () {
		thisScore.text = "Score: " + (gameScore);
		if (restart){
			StartCoroutine(stopRestart());
		}
	}

	public static int getGameScore(){
		return gameScore;
	}

	public static void setToZero(){
		gameScore = 0;
	}

	IEnumerator stopRestart() {
		yield return new WaitForSeconds(1);
		restart = false;
	}

	public static void scoreUp(int x){
		gameScore += x;
		if (highScore.getHighScore() < gameScore){
			highScore.hiScore = gameScore;
		}
	}

}
