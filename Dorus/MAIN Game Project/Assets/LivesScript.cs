using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LivesScript : MonoBehaviour {

	private static Text ThisText;
	public static int lifes;

	// Use this for initialization
	void Awake () {
		ThisText=this.GetComponent<Text>();
	
	}

	public static void SetLives(int Count){
		lifes=Count;
		if(ThisText!=null){
		ThisText.text=Count + "x";
		}
	}

	public static void AddLives(int Count){
		lifes=Count +lifes;
		ThisText.text=lifes + "x";
	}

}
