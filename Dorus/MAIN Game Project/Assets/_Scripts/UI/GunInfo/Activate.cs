//This script is just a temporary quick way to get this working right

using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour {

	private static GameObject thisObject;
	// Use this for initialization
	void Awake () {
		thisObject = gameObject;
		setActiveCustom (false);
	}

	public static void setActiveCustom(bool active){
		if(thisObject!=null){
		thisObject.SetActive (active);
		}
	}

	public static void SetCrossHair(float amount){
		if(thisObject!=null){
		Crosshair.thisObject.setSpread(amount);
		}
	}
}
