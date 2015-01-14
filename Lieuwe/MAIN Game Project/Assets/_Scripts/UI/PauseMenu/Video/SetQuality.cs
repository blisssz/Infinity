using UnityEngine;
using System.Collections;

public class SetQuality : Navigation {

	private string[] settings = new string[3];
	
	// Use this for initialization
	void Awake () {
		settings [0] = "LOW";
		settings [1] = "MEDIUM";
		settings [2] = "HIGH";
		currentSetting = 2;
		maxSetting = 2;
		setText (settings[currentSetting]);
	}
	
	public override void changeSetting (bool on)
	{
		base.changeSetting (on);
		QualitySettings.SetQualityLevel (currentSetting);
		setText (settings[currentSetting]);
	}

}
