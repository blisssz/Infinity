using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlidingBar : MonoBehaviour {

	public GameObject percentageText;
	private RectTransform rectTransform;
	private Image image;
	private float percentage;

	// Use this for initialization
	void Start () {
		rectTransform = this.GetComponent<RectTransform>();
		image = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Smooth not implemented
	public void setValueFade(float newValue, float maxValue, bool smooth){
		percentage = newValue / maxValue;
		rectTransform.anchorMax = new Vector2(percentage, rectTransform.anchorMax.y);
		percentageText.GetComponent<SliderValue>().setIntPercentage(percentage);
	}


}
