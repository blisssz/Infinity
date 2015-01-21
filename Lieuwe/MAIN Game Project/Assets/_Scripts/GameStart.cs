using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	private static int StartLifes=10;

	// Use this for initialization
	void Start () {
		int weapon = Mathf.RoundToInt (Random.Range (1,6));
		PlayerManager.useWeaponID = weapon;
		GameController.lifes=StartLifes;
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
		int weapon = Mathf.RoundToInt (Random.Range (1,6));
		PlayerManager.useWeaponID = weapon;
		GameController.lifes=StartLifes;
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
		switch (PlayerManager.useWeaponID) {
		case 1:
			Application.LoadLevel ("PogoStickBoss");
			break;
		case 2: 
			Application.LoadLevel ("GravityBoss");
			break;
		case 3:
			Application.LoadLevel ("GrapplingHookBoss");
			break;
		case 4:
			Application.LoadLevel ("PogoStickBoss");
			break;
		case 5:
			Application.LoadLevel ("PogoStickBoss");
			break;
		case 6:
			Application.LoadLevel ("GrapplingHookBoss");
			break;
		default: 
			Debug.Log ("wrong number");
			break;
		}
	}
}
