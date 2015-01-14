using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Navigation : MonoBehaviour {

	public Text displayText;

	public int currentSetting;
	public int maxSetting;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Screen.GetResolution);
//		foreach (Resolution resolution in Screen.GetResolution) {
//			Debug.Log (resolution.width + "px x " + resolution.height + "px @" + resolution.refreshRate + "Hz");
//		}
		//Debug.Log ((Screen.GetResolution).Length);
	}

	public void setText (string text){
		displayText.text = text;	
	}

	public virtual void changeSetting (bool up){
		if(up == true){
			currentSetting++;
		}
		else{
			currentSetting--;	
		}
		currentSetting = Mathf.Clamp (currentSetting, 0, maxSetting);
	}
}
