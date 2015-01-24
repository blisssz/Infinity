using UnityEngine;
using System.Collections;

public class SetThickness : NavigationSlider {
	
	// Use this for initialization
	void Awake () {
		currentSetting = 1f;
		maxSetting = 5f;
	}
	
	public override void changeSetting (bool up)
	{
		changeSettingUnique (up);
		StartCoroutine (heldDown (up));
	}
	
	public override void setSlider (float value)
	{
		value = Mathf.Clamp (value, 1f, maxSetting);
		base.setSlider (value);
		Crosshair.thisObject.setThickness (value);
		CrosshairPreview.thisObject.setThickness (value);
	}
	
	public IEnumerator heldDown(bool up){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.2f));
		while(Input.GetMouseButton(0) == true){
			changeSettingUnique (up);
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.2f));
		}
	}
	
	public void changeSettingUnique (bool up){
		if(up == true){
			currentSetting += 1f;
		}
		else{
			currentSetting -= 1f;	
		}
		currentSetting = Mathf.Clamp (currentSetting, 1f, maxSetting);
		setSlider (currentSetting);
	}
	
	
}
