using UnityEngine;
using System.Collections;

public class UseCrosshair : Navigation {

	// Use this for initialization
	void Awake () {
		currentSetting = 1;
		maxSetting = 1;
	}
	
	public override void changeSetting (bool on)
	{
		base.changeSetting (on);
		Crosshair.thisObject.drawCrosshair (on);
	}
}
