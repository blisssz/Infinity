using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class highScoreText : MonoBehaviour {
	public Text highScoreTxt;
	private int hiScore;

	// Use this for initialization
	void Start () {
		highScoreTxt = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		hiScore = highScore.getHighScore ();
		highScoreTxt.text = "High Score: " + hiScore;
	}
	
	
}