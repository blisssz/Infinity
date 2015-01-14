using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextHighlight : MonoBehaviour {

	protected Text text;
	public Color colorNormal;
	public Color colorHighlighted;

	// Use this for initialization
	protected virtual void Start () {
		text = this.GetComponent<Text> ();
		text.color = colorNormal;
	}

	protected virtual void OnDisable () {
		text.color = colorNormal;
	}

	public void color(bool highlighted){
		if(highlighted == true){
			text.color = colorHighlighted;
		}else{
			text.color = colorNormal;
		}
	}
}
