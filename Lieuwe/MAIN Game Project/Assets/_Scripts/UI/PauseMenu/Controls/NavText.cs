using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NavText : TextHighlight {

	public bool selected;
	public Image underscore;


	protected override void Awake () {
		text = this.GetComponent<Text> ();
		checkState ();
	}

	protected override void OnDisable () {
		checkState ();
	}

	public override void color(bool highlighted){
		if(highlighted == true){
			text.color = colorHighlighted;
			if(selected == false){
				playAudio(soundHover);
			}
		}else{
			checkState ();
		}
	}

	public void checkState(){
		if(selected == true){
			text.color = colorHighlighted;
		}
		else{
			text.color = colorNormal;
		}
	}

	public void setSelected(bool state){
		selected = state;
		underscore.enabled = state;
		if (state == false) {
			text.color = colorNormal;
		}
	}

}
