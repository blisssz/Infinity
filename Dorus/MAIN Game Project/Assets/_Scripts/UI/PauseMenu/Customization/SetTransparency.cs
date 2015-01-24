using UnityEngine;
using System.Collections;

public class SetTransparency : NavigationSlider {
	
	// Use this for initialization
	void Awake () {
		currentSetting = 0.7843137255f;
	}
	
	public override void changeSetting (bool up)
	{
		base.changeSetting (up);
		StartCoroutine (heldDown (up));
	}
	
	public override void setSlider (float value)
	{
		base.setSlider (value);
		Crosshair.thisObject.setTransparency (currentSetting);
		CrosshairPreview.thisObject.setTransparency (currentSetting);
	}
	
	public IEnumerator heldDown(bool up){
		while(Input.GetMouseButton(0) == true){
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.01f));
			base.changeSetting (up);
		}
	}
	
	
}
