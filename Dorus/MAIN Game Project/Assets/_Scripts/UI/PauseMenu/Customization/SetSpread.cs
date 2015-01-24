using UnityEngine;
using System.Collections;

public class SetSpread : NavigationSlider {
	
	// Use this for initialization
	void Awake () {
		maxSetting = 30f;
		if(PlayerManager.useWeaponID == 5){
			currentSetting = 25f;
		}
		else {
			currentSetting = 4f;
		}
	}

	void Start (){
		if (currentSetting == 25f) {
			setSlider (currentSetting);
		}
	}
	
	public override void changeSetting (bool up)
	{
		changeSettingUnique (up);
		StartCoroutine (heldDown (up));
	}
	
	public override void setSlider (float value)
	{
		base.setSlider (value);
		Crosshair.thisObject.setSpread (currentSetting);
		CrosshairPreview.thisObject.setSpread (currentSetting);
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
		currentSetting = Mathf.Clamp (currentSetting, 0, maxSetting);
		setSlider (currentSetting);
	}
	
	
}
