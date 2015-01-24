using UnityEngine;
using System.Collections;

public class SetFullScreen : Navigation {

	
	// Use this for initialization
	void Awake () {
		currentSetting = 1;
		maxSetting = 1;
	}

	public override void changeSetting (bool on)
	{
		base.changeSetting (on);
		Screen.fullScreen = on;
	}
}
