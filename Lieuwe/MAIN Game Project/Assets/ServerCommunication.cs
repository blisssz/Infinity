using System.Collections;
using UnityEngine;

public class ServerCommunication : MonoBehaviour {
	static WWWForm form;
	static WWWForm loginForm;
	static WWWForm achievementForm;
	static WWW test;
	static string url;
	public static string username = "54455";
	public static string password = "ik";
	public static bool loggedIn;
	public static bool shouldI;
	public static bool goRegister;

	public static int hiScore;
	public static bool[] achievements = new bool[highScore.getSum()];


	void Start() {
		goRegister = true;
	}

	void Update() {
		if (shouldI) {
			StartCoroutine(login(username, password));
			if (ServerCommunication.loggedIn) {
				ServerCommunication.sendHighscore();
				ServerCommunication.sendAchievements();
			}
			shouldI = false;
		}
		if (goRegister) {
			StartCoroutine(register(username, password));
			goRegister = false;
		}
	}

	void Awake() {
		if (shouldI) {
			StartCoroutine(login(username, password));
			StartCoroutine (getHighscore());
			StartCoroutine (getAchievements());
			print (hiScore);
			shouldI = false;
		}
	}

	public IEnumerator register(string username, string password){
		form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);
		url = "drproject.twi.tudelft.nl:8088/new_account";
		test = new WWW (url, form);
		yield return test;
		if(int.Parse(test.text) == 1){
			loggedIn = true;
		}
	}

	public IEnumerator login(string username, string password) {
		loginForm = new WWWForm ();
		loginForm.AddField ("username", username);
		loginForm.AddField ("password", password);
		url = "drproject.twi.tudelft.nl:8088/unity_login";
		test = new WWW (url, loginForm);
		yield return test;
		print (test.text + " want david is een koning");
		if (bool.Parse (test.text)) {
			loggedIn = true;
		}
	}

	public static void sendHighscore() {
		form = new WWWForm ();
		form.AddField ("Highscore", highScore.getHighScore());
		form.AddField ("Username", username);
		//form.AddField ("Achievements", highScore.getAchievements ());
		url = "drproject.twi.tudelft.nl:8088/unity";
		test = new WWW(url, form);
	}

	public IEnumerator getHighscore() {
		form = new WWWForm ();
		form.AddField ("Username", username);
		url = "drproject.twi.tudelft.nl:8088/getHighscore";
		test = new WWW (url, form);
		yield return test;
		//print (test.text);
		hiScore = (int.Parse (test.text));
	}

	public IEnumerator getAchievements() {
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

	public static void sendAchievements() {
		achievementForm = new WWWForm ();
		achievementForm.AddField ("Achievement", highScore.getAchievements ());
		achievementForm.AddField ("Username", username);
		url = "drproject.twi.tudelft.nl:8088/unityAchievements";
		test = new WWW (url, achievementForm);
	}

	private void startRoutines(){
		StartCoroutine (getHighscore ());
		StartCoroutine (getAchievements ());
	}
}