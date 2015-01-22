using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NavigationSlider : MonoBehaviour {
	
	public Slider slider;
	
	public float currentSetting;
	public float maxSetting = 1f;
	

	public virtual void setSlider (float value){
		currentSetting = value;
		slider.value = currentSetting;	
	}

	public virtual void changeSetting (bool up){
		if(up == true){
			currentSetting += 0.01f;
		}
		else{
			currentSetting -= 0.01f;	
		}
		currentSetting = Mathf.Clamp (currentSetting, 0, maxSetting);
		setSlider (currentSetting);
	}
}
