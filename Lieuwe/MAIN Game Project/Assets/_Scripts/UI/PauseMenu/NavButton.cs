using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavButton : TextHighlight {

	public Color colorDisabled;
	public bool leftArrow; //true = left, false = right
	private Navigation parent;

	// Use this for initialization
	protected override void Start () {
		text = this.GetComponent<Text> ();
		parent = transform.parent.GetComponent<Navigation>();

		if((parent.currentSetting == 0 && leftArrow == true) || (parent.currentSetting == parent.maxSetting && leftArrow == false)){
			text.color = colorDisabled;
		}
		else{
			text.color = colorNormal;
		}
	}

	protected override void OnDisable () {
		text.color = colorNormal;
	}

	// Update is called once per frame
	void Update () {

	}

	public void MouseEnter () {
		if((parent.currentSetting == 0 && leftArrow == true) || (parent.currentSetting == parent.maxSetting && leftArrow == false)){
			text.color = colorDisabled;
		}
		else{
			text.color = colorHighlighted;
		}
	}

	public void MouseExit () {
		if((parent.currentSetting == 0 && leftArrow == true) || (parent.currentSetting == parent.maxSetting && leftArrow == false)){
			text.color = colorDisabled;
		}
		else{
			text.color = colorNormal;
		}
	}

	public void UpdateOther () {
			text.color = colorNormal;
	}

}
