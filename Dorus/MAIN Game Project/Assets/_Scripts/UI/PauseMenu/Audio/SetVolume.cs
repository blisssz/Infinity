using UnityEngine;
using System.Collections;

public class SetVolume : NavigationSlider {

	// Use this for initialization
	void Start () {
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
		AudioListener.volume = 4f * (value * value);
	}

	public IEnumerator heldDown(bool up){
		while(Input.GetMouseButton(0) == true){
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.01f));
			base.changeSetting (up);
		}
	}


}
