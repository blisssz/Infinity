using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LivesScript : MonoBehaviour {

	private static Text ThisText;

	// Use this for initialization
	void Awake () {
		ThisText=this.GetComponent<Text>();
	
	}

	public static void SetLives(int Count){
		ThisText.text=Count + "x";
	}

}
