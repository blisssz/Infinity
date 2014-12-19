using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderValue : MonoBehaviour {
	Text textValue;

	// Use this for initialization
	void Start () {
		textValue = GetComponent<Text> ();
	}
	
	public void setIntPercentage(float value){
		textValue.text = Mathf.RoundToInt (value * 100) + "%";
	}

	public void setIntValue(float value){
		textValue.text = (Mathf.RoundToInt (value)).ToString();
	}

	public void setPercentage(float value){
		textValue.text = value * 100 + "%";
	}

	public void setValue(float value){
		textValue.text = value.ToString();
	}

}
