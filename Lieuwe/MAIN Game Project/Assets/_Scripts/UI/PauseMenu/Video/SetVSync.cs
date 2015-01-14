using UnityEngine;
using System.Collections;

public class SetVSync : Navigation {
	
	
	// Use this for initialization
	void Awake () {
		currentSetting = 1;
		maxSetting = 1;
		setText ("ON");
	}
	
	public override void changeSetting (bool on)
	{
		base.changeSetting (on);
		QualitySettings.vSyncCount = boolToInt (on);
		if(on == true){
			setText ("ON");
		}else{
			setText ("OFF");
		}
	}

	private int boolToInt(bool boolean){
		if (boolean == true) {
			return 1;
		}
		return 0;
	}
}
