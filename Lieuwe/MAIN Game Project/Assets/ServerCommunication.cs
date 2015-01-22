using System.Collections;
using UnityEngine;

public class ServerCommunication : MonoBehaviour {
	static WWWForm form;
	static WWWForm loginForm;
	static WWWForm achievementForm;
	static WWW test;
	static string url;
	static string username = "David";
	static string password = "ik";
	public static bool loggedIn = true;

	public static int hiScore;
	public static bool[] achievements = new bool[highScore.getSum()];

	void Start() {
//		sendHighscore ();
//		StartCoroutine (sendAchievements ());
//		StartCoroutine(login ());
//		StartCoroutine (getHighscore ());
		StartCoroutine (getAchievements ());
	}

	public static void sendHighscore() {
		form = new WWWForm ();
		form.AddField ("Highscore", highScore.getHighScore());
		form.AddField ("Username", username);
		//form.AddField ("Achievements", highScore.getAchievements ());
		url = "drproject.twi.tudelft.nl:8088/unity";
		test = new WWW(url, form);
	}

	public static IEnumerator getHighscore() {
		form = new WWWForm ();
		form.AddField ("Username", username);
		url = "drproject.twi.tudelft.nl:8088/getHighscore";
		test = new WWW (url, form);
		yield return test;
		//print (test.text);
		hiScore = (int.Parse (test.text));
	}

	public static IEnumerator getAchievements() {
		achievementForm = new WWWForm ();
		achievementForm.AddField ("Username", username);
		url = "drproject.twi.tudelft.nl:8088/getAchievements";
		test = new WWW (url, achievementForm);
		yield return test;
		int j = 0;
		for(int i = 0; i < test.text.Length; i++){
			print ((int)char.GetNumericValue(test.text[i]));
			if((int)char.GetNumericValue(test.text[i]) == 1){
				achievements[j] = true;	
				j++;
			}
			else { if((int)char.GetNumericValue(test.text[i]) == 0){
				achievements[j] = false;
				j++;
			}}
		}
	}

	public static IEnumerator sendAchievements() {
		yield return new WaitForSeconds (0.5f);
		achievementForm = new WWWForm ();
		achievementForm.AddField ("Achievement", highScore.getAchievements ());
		achievementForm.AddField ("Username", username);
		url = "drproject.twi.tudelft.nl:8088/unityAchievements";
		test = new WWW (url, achievementForm);
	}

	public static IEnumerator login() {
		loginForm = new WWWForm ();
		loginForm.AddField ("username", username);
		loginForm.AddField ("password", password);
		url = "drproject.twi.tudelft.nl:8088/unity_login";
		test = new WWW (url, loginForm);
		yield return test;
		print (test.text + " want david is een koning");
		if (bool.Parse (test.text)) {
			loggedIn = true;
			print (loggedIn);
		}
	}
	

	public static void setUsername(string str) {
		username = str;
	}

	public static void setPassword(string str) {
		password = str;
	}
}