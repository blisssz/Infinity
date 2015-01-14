using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class highScore : MonoBehaviour {
	private static int  hiScore;
	private static int totalEnemiesKilled;
	private static int totalCoinsPickedUp;
	private static int totalAmmoPickedUp;
	private static int totalHealthPickedUp;
	private static int totalGrapplingHookMisses;
	private static int totalDeaths;

	// Use this for initialization
	void Start () {
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
			totalDeaths += 1;		
		}
	}

	// Update is called once per frame
	void Update () {
		if (hiScore < score.getGameScore()){
			hiScore = score.getGameScore ();
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
	}

	public static void enemyKill(){
		totalEnemiesKilled += 1;
	}

	public static void pickUpCoin(){
		totalCoinsPickedUp += 1;
	}

	public static void pickUpHealth(){
		totalHealthPickedUp += 1;
	}

	public static void pickUpAmmo(){
		totalAmmoPickedUp += 1;
	}

	public static void grapplingHookMiss(){
		totalGrapplingHookMisses += 1;
	}

	public static void death(){
		totalDeaths += 1;
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
}
