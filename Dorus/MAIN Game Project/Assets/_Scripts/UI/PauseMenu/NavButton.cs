using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavButton : TextHighlight {

	public Color colorDisabled;
	public bool leftArrow; //true = left, false = right

	protected Navigation parent1;
	protected NavigationSlider parent2;

	// Use this for initialization
	protected override void Awake () {
		text = this.GetComponent<Text> ();
		parent1 = transform.parent.GetComponent<Navigation>();
		parent2 = transform.parent.GetComponent<NavigationSlider>();
	}

	void Start () {
		CheckState ();
	}

	protected float getCurrentSetting(){
		if(parent1 != null){return parent1.currentSetting;}
		if(parent2 != null){return parent2.currentSetting;}
		Debug.Log ("Something is wrong, the parent's of this navButton doesn't have a navigation or navigationslider component");
		return 1f;
	}

	protected float getMaxSetting(){
		if(parent1 != null){return parent1.maxSetting;}
		if(parent2 != null){return parent2.maxSetting;}
		Debug.Log ("Something is wrong, the parent's of this navButton doesn't have a navigation or navigationslider component");
		return 1f;
	}

	protected override void OnDisable () {
		CheckState ();
	}

	public virtual void MouseEnter () {
		if((getCurrentSetting() == 0 && leftArrow == true) || (getCurrentSetting() == getMaxSetting() && leftArrow == false)){
			text.color = colorDisabled;
		}
		else{
			text.color = colorHighlighted;
			playAudio(soundHover);
		}
	}

	public void MouseExit () {
		CheckState ();
	}

	public void UpdateOther () {
			text.color = colorNormal;
	}

	public virtual void CheckState () {
		if((getCurrentSetting() == 0 && leftArrow == true) || (getCurrentSetting() == getMaxSetting() && leftArrow == false)){
			text.color = colorDisabled;
		}
		else{
			text.color = colorNormal;
		}
	}


}
