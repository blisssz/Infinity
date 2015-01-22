using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	private static int StartLifes=10;
	public static int currentBossScene =1;

	// Use this for initialization
	void Start () {
		int weapon = Mathf.RoundToInt (Random.Range (1,6));
		PlayerManager.useWeaponID = weapon;
		GameController.ResetAll();
		currentBossScene =0;
		switch(weapon){
		case 1: 
			Application.LoadLevel ("Platforms");
			break;
		case 2:
			Application.LoadLevel ("Doolhof");
			break;
		case 3: 
			selectLevel ();
			break;
		case 4:
			selectLevel ();
			break;
		case 5:
			selectLevel ();
			break;
		case 6:
			selectLevel ();
			break;
		default: 
			Debug.Log ("wrong number");
			break;
		}
	}

	public static void loadNormal () {
		currentBossScene =0;
		GameController.ResetAll();
		int weapon = Mathf.RoundToInt (Random.Range (1,6));
		PlayerManager.useWeaponID = weapon;
		switch(weapon){
		case 1: 
			Application.LoadLevel ("Platforms");
			break;
		case 2:
			Application.LoadLevel ("Doolhof");
			break;
		case 3: 
			selectLevel ();
			break;
		case 4:
			selectLevel ();
			break;
		case 5:
			selectLevel ();
			break;
		case 6:
			Application.LoadLevel ("Platforms");
			break;
		default: 
			Debug.Log ("wrong number");
			break;
		}
	}
	
		static void selectLevel(){
			int level = Mathf.RoundToInt(Random.value);
			if(level == 0){
				Application.LoadLevel ("Platforms");
			}
			if(level == 1){
				Application.LoadLevel ("Doolhof");
			}
		}

	public static void loadBoss(){
		GameController.ResetAll();
		switch (PlayerManager.useWeaponID) {
		case 1:
			currentBossScene =1;
			Application.LoadLevel ("PogoStickBoss");
			break;
		case 2: 
			currentBossScene =2;
			Application.LoadLevel ("GravityBoss");
			break;
		case 3:
			currentBossScene =3;
			Application.LoadLevel ("GrapplingHookBoss");
			break;
		case 4:
			currentBossScene =1;
			Application.LoadLevel ("PogoStickBoss");
			break;
		case 5:
			currentBossScene =1;
			Application.LoadLevel ("PogoStickBoss");
			break;
		case 6:
			currentBossScene =3;
			Application.LoadLevel ("GrapplingHookBoss");
			break;
		default: 
			Debug.Log ("wrong number");
			break;
		}
	}
}
