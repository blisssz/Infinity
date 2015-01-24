using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropDownHighlight : TextHighlight {

	public Color backgroundColorNormal;
	public Color backgroundColorHighlight;
	public Image background;

	protected override void Awake ()
	{
		base.Awake ();
		background.color = backgroundColorNormal;
	}

	public override void color(bool highlighted){
		base.color (highlighted);
		if(highlighted == true){
			background.color = backgroundColorHighlight;
			
		}else{
			background.color = backgroundColorNormal;
		}
	}

	protected override void OnDisable () {
		base.OnDisable ();
		background.color = backgroundColorNormal;

	}
}
