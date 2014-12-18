using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class highScore : MonoBehaviour {
	public Text highScoreTxt;
	public int  hiScore;
	public static int totalEnemiesKilled;
	public static int totalPlatformsVisited;
	public static int missedGrapplingHook;
	// Use this for initialization
	void Start () {
		highScoreTxt = GetComponent<Text> ();
		hiScore = score.getGameScore ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hiScore < score.getGameScore()){
			hiScore = score.getGameScore ();
		}
		highScoreTxt.text = "High Score: " + hiScore;
	}


}
