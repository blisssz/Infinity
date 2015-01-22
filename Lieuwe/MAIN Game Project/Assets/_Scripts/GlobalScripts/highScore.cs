using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class highScore : MonoBehaviour {
	public static int  hiScore;
	private static int totalEnemiesKilled;
	private static int totalCoinsPickedUp;
	private static int totalAmmoPickedUp;
	private static int totalHealthPickedUp;
	private static int totalGrapplingHookMisses;
	private static int totalDeaths;
	private static bool[] achievements;
	
	private static int[] noOfAchievements = new int[5] {3, 3, 3, 3, 3};

	// Use this for initialization
	void Start () {
		if(!ServerCommunication.loggedIn){
			if(PlayerPrefs.GetInt ("High Score") != 0){
				hiScore = PlayerPrefs.GetInt ("High Score");
			}
			else{ hiScore = 0;}
//			if (PlayerPrefsX.GetBoolArray ("Achievements") != null) {
//				achievements = PlayerPrefsX.GetBoolArray("Achievements");
//			}
//			else{ 
//				achievements = new bool[calcSum(noOfAchievements, 5)];
//			}
		}
		else{
			StartCoroutine(getStatus ());
		}

		if(PlayerPrefs.GetInt ("High Score") != 0){
			hiScore = PlayerPrefs.GetInt ("High Score");
		}
		else{ hiScore = 0;}
		if (PlayerPrefs.GetInt ("Total enemies killed") != 0) {
			totalEnemiesKilled = PlayerPrefs.GetInt ("Total enemies killed");		
		}
		else{ totalEnemiesKilled = 0; }
		if(PlayerPrefs.GetInt ("Total coins picked up") != 0){
			totalCoinsPickedUp = PlayerPrefs.GetInt ("Total coins picked up");
		}
		else{ totalHealthPickedUp = 0;}
		if(PlayerPrefs.GetInt ("Total healthPackages picked up") != 0){
			totalHealthPickedUp = PlayerPrefs.GetInt ("Total coins picked up");
		}
		else{ totalCoinsPickedUp = 0;}
		if(PlayerPrefs.GetInt ("Total ammoPackages picked up") != 0){
			totalAmmoPickedUp = PlayerPrefs.GetInt ("Total coins picked up");
		}
		else{ totalAmmoPickedUp = 0;}
		if(PlayerPrefs.GetInt ("Total grapplinghook misses") != 0){
			totalGrapplingHookMisses = PlayerPrefs.GetInt ("Total grapplinghook misses");
		}
		else{ totalGrapplingHookMisses = 0;}
		if (PlayerPrefs.GetInt ("Total deaths") != 0) {
			totalDeaths = PlayerPrefs.GetInt("Total deaths");	
		}
		else{ totalDeaths = 0;}
	}

	IEnumerator getStatus(){
		yield return new WaitForSeconds (3);
		hiScore = ServerCommunication.hiScore;
		achievements = ServerCommunication.achievements;
	}
	
	// Update is called once per frame
	void Update () {
		if (hiScore < score.getGameScore()){
			hiScore = score.getGameScore ();
		}
		if (hiScore >= 100) {
			achievements[0] = true;		
		}
	}


	public static int getHighScore(){
		return hiScore;
	}

	void OnDestroy(){
		PlayerPrefs.SetInt ("High Score", hiScore);
		PlayerPrefs.SetInt ("Total enemies killed", totalEnemiesKilled);
		PlayerPrefs.SetInt ("Total coins picked up", totalCoinsPickedUp);
		PlayerPrefs.SetInt ("Total healthpackages picked up", totalCoinsPickedUp);
		PlayerPrefs.SetInt ("Total ammopackages picked up", totalCoinsPickedUp);
		PlayerPrefs.SetInt ("Total grapplinghook misses", totalGrapplingHookMisses);
		PlayerPrefs.SetInt ("Total deaths", totalDeaths);
//		PlayerPrefsX.SetBoolArray ("Achievements", achievements);
	}

	public static void enemyKill(){
		totalEnemiesKilled += 1;
		if (totalEnemiesKilled >= 20) {
			achievements [0 + calcSum(noOfAchievements, 1)] = true;		
		}
	}

	public static void pickUpCoin(){
		totalCoinsPickedUp += 1;
		if(totalCoinsPickedUp >= 3) {
			achievements[0 + calcSum(noOfAchievements, 2)] = true;
		}
	}

	public static void pickUpHealth(){
		totalHealthPickedUp += 1;
	}

	public static void pickUpAmmo(){
		totalAmmoPickedUp += 1;
	}

	public static void grapplingHookMiss(){
		totalGrapplingHookMisses += 1;
		if(totalGrapplingHookMisses >= 20) {
			achievements[0 + calcSum(noOfAchievements, 3)] = true;
		}
	}

	public static void death(){
		totalDeaths += 1;
		if (totalDeaths >= 2) {
			achievements[0 + calcSum(noOfAchievements, 4)] = true;		
		}
	}

	public static int getTotalEnemiesKilled(){
		return totalEnemiesKilled;
	}

	public static int getTotalCoinsPickedUp(){
		return totalCoinsPickedUp;
	}

	public static int getTotalHealthPickedUp(){
		return totalCoinsPickedUp;
	}

	public static int getTotalAmmoPickedUp(){
		return totalCoinsPickedUp;
	}

	public static int getMissedGrapplingHook(){
		return totalGrapplingHookMisses;
	}

	public static int getTotalDeaths(){
		return totalDeaths;
	}
	
	public static string getAchievements() {
		string str = "";
		int j = 1;
		for (int i = 0; i < achievements.Length; i++) {
			if(achievements[i]){ str += "1";}
			else { str += "0";}
			if(i == calcSum(noOfAchievements, j) - 1 && j != 5){
				str += "2";
				j++;
			}
		}
		return str;
	}
	
	private static int calcSum(int[] rij, int i) {
		int som = 0;
		for (int k = 0; k < i; k++) {
			som += rij[k];
		}
		return som;
	}
	
	public static int getSum(){
		return calcSum(noOfAchievements, noOfAchievements.Length);
	}
}

