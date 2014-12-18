using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int level = Mathf.RoundToInt(Random.value);
		if(level == 0){
			Application.LoadLevel ("Platforms");
		}
		if(level == 1){
			Application.LoadLevel ("Platforms");
		}
	}
}
