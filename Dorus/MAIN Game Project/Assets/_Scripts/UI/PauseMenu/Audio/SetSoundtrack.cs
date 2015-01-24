using UnityEngine;
using System.Collections;

public class SetSoundtrack : NavigationSlider {
	
	
	// Use this for initialization
	void Awake () {
		currentSetting = 0.5f;
	}

	public override void changeSetting (bool up)
	{
		base.changeSetting (up);
		StartCoroutine (heldDown (up));
	}
	
	public override void setSlider (float value)
	{
		base.setSlider (value);
		AudioList.VolumeBackground( 4f * (value * value));
		//Debug.Log ("Still need to uncomment this!");
	}
	
	public IEnumerator heldDown(bool up){
		while(Input.GetMouseButton(0) == true){
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.01f));
			base.changeSetting (up);
		}
	}	
}
