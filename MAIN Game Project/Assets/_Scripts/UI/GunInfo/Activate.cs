//This script is just a temporary quick way to get this working right

using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour {

	private static GameObject thisObject;
	// Use this for initialization
	void Start () {
		thisObject = gameObject;
		setActiveCustom (false);
	}

	public static void setActiveCustom(bool active){
		thisObject.SetActive (active);
	}
}
