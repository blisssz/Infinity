using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int weapon = Mathf.RoundToInt (Random.Range (1,4));
		switch(weapon){
		case 1: 
			selectLevel ();
			break;
		case 2:
			selectLevel ();
			break;
		case 3: 
			selectLevel ();
			break;
		default: 
			Debug.Log ("wrong number");
		}
	}
	
		void selectLevel(){
			int level = Mathf.RoundToInt(Random.value);
			if(level == 0){
				Application.LoadLevel ("Platforms");
			}
			if(level == 1){
				Application.LoadLevel ("Doolhof");
			}
		}
}
