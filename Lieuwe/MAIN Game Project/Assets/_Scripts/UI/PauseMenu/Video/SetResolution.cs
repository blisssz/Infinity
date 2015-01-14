using UnityEngine;
using System.Collections;

public class SetResolution : Navigation {

	public SetFullScreen fullScreen;

	private Resolution[] resolutions;

	// Use this for initialization
	void Awake () {
		resolutions = Screen.GetResolution;
		currentSetting = Mathf.RoundToInt (resolutions.Length / 2);
		maxSetting = resolutions.Length -1;
		setText (resolutions [currentSetting].width + "X" + resolutions [currentSetting].height);
	}

	public override void changeSetting (bool on)
	{
		base.changeSetting (on);
		Screen.SetResolution (resolutions [currentSetting].width, resolutions [currentSetting].height, makeBool(fullScreen.currentSetting));
		setText (resolutions [currentSetting].width + "X" + resolutions [currentSetting].height);
	}

	private bool makeBool(int setting){
		if (setting == 1) {
			return true;
		}
		return false;
	}
}
