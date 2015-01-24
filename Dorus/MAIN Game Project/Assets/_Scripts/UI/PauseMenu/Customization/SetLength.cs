using UnityEngine;
using System.Collections;

public class SetLength : NavigationSlider {
	
	// Use this for initialization
	void Awake () {
		currentSetting = 4f;
		maxSetting = 15f;
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
		Crosshair.thisObject.setLength (value);
		CrosshairPreview.thisObject.setLength (value);
	}
	
	public IEnumerator heldDown(bool up){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.15f));
		while(Input.GetMouseButton(0) == true){
			changeSettingUnique (up);
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.15f));
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
