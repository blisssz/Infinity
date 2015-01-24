using UnityEngine;
using System.Collections;

public class NavSliderButton : NavButton {

	public AudioClip soundClick;
	public int minimumvalue;

	private bool activated = false;
	private bool stateOnMouseDown;

	public override void MouseEnter () {
		if((getCurrentSetting() == minimumvalue && leftArrow == true) || (getCurrentSetting() == getMaxSetting() && leftArrow == false)){
			text.color = colorDisabled;
		}
		else{
			text.color = colorHighlighted;
			playAudio(soundHover);
		}
	}

	public override void CheckState ()
	{
		if((getCurrentSetting() == minimumvalue && leftArrow == true) || (getCurrentSetting() == getMaxSetting() && leftArrow == false)){
			text.color = colorDisabled;
		}
		else if(Input.GetMouseButton(0) == true && activated == true){
			text.color = colorHighlighted;
		}
		else{
			text.color = colorNormal;
		}
	}

	public  void CheckStateDisable ()
	{
		if((getCurrentSetting() == minimumvalue && leftArrow == true) || (getCurrentSetting() == getMaxSetting() && leftArrow == false)){
			text.color = colorDisabled;
		}
	}

	public void setActiveBool (bool status){
		activated = status;
	}

	public void setStateOnMouseDown(){
		if ((getCurrentSetting () == minimumvalue && leftArrow == true) || (getCurrentSetting () == getMaxSetting () && leftArrow == false)) {
			stateOnMouseDown = false;
		} else{
			stateOnMouseDown = true;
		}
	}

	public void playClick(){
		if(stateOnMouseDown == true){
			Time.timeScale = 1f;
			AudioSource.PlayClipAtPoint(soundClick, new Vector3(0,0,0), 0.5f);
			Time.timeScale = 0f;
		}
	}
}
