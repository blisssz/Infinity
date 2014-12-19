using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public bool switcher = false;	//A switch to regulate when the crosshairs being turned on or off at every keypress

	public GameObject LeftCrosshair;
	public GameObject RightCrosshair;
	public GameObject UpperCrosshair;
	public GameObject LowerCrosshair;

	void Awake(){
		drawCrosshair (false);
	}

	void Update() {
		bool key1 = KeyManager.key1 == 1; // tap key once
		if (key1 == true) {
			if (switcher == false) {
				drawCrosshair (true);
				switcher = true;
			} 
			else {
				drawCrosshair (false);
				switcher = false;
			}
		}
	}

	/**
	 * A function for turning the crosshairs on or off. true = on, false = off.
	 */
	public void drawCrosshair(bool switcher){
		LeftCrosshair.SetActive (switcher);
		RightCrosshair.SetActive (switcher);
		UpperCrosshair.SetActive (switcher);
		LowerCrosshair.SetActive (switcher);
	}

	/**
	 * A function for setting the length of the crosshairs in pixels
	 */
	public void setLength(float length){
		RectTransform leftCrosshair = LeftCrosshair.transform.GetComponent<RectTransform>();
		RectTransform rightCrosshair = RightCrosshair.transform.GetComponent<RectTransform>();
		RectTransform upperCrosshair = UpperCrosshair.transform.GetComponent<RectTransform>();
		RectTransform lowerCrosshair = LowerCrosshair.transform.GetComponent<RectTransform>();

		leftCrosshair.sizeDelta = new Vector2(length, leftCrosshair.sizeDelta.y);
		rightCrosshair.sizeDelta = new Vector2(length, rightCrosshair.sizeDelta.y);
		upperCrosshair.sizeDelta = new Vector2(upperCrosshair.sizeDelta.x, length);
		lowerCrosshair.sizeDelta = new Vector2(lowerCrosshair.sizeDelta.x, length);
	}

	/**
	 * A function for setting the thickness of the crosshairs in pixels
	 */
	public void setThickness(float thickness){
		RectTransform leftCrosshair = LeftCrosshair.transform.GetComponent<RectTransform>();
		RectTransform rightCrosshair = RightCrosshair.transform.GetComponent<RectTransform>();
		RectTransform upperCrosshair = UpperCrosshair.transform.GetComponent<RectTransform>();
		RectTransform lowerCrosshair = LowerCrosshair.transform.GetComponent<RectTransform>();
		
		leftCrosshair.sizeDelta = new Vector2(leftCrosshair.sizeDelta.x, thickness);
		rightCrosshair.sizeDelta = new Vector2(rightCrosshair.sizeDelta.x, thickness);
		upperCrosshair.sizeDelta = new Vector2(thickness, upperCrosshair.sizeDelta.y);
		lowerCrosshair.sizeDelta = new Vector2(thickness, lowerCrosshair.sizeDelta.y);
	}

	/**
	 * A function for setting the spread of the crosshairs in pixels from the center
	 */
	public void setSpread(float spread){
		RectTransform leftCrosshair = LeftCrosshair.transform.GetComponent<RectTransform>();
		RectTransform rightCrosshair = RightCrosshair.transform.GetComponent<RectTransform>();
		RectTransform upperCrosshair = UpperCrosshair.transform.GetComponent<RectTransform>();
		RectTransform lowerCrosshair = LowerCrosshair.transform.GetComponent<RectTransform>();

		leftCrosshair.localPosition = new Vector3 (-spread, 0, 0);
		rightCrosshair.localPosition = new Vector3 (spread, 0, 0);
		upperCrosshair.localPosition = new Vector3 (0, spread, 0);
		lowerCrosshair.localPosition = new Vector3 (0, -spread, 0);
	}

	/**
	 * A function for changing the color of the crosshairs
	 */
	public void setColor(Color color){
		LeftCrosshair.transform.GetComponent<Image>().color = color;
		RightCrosshair.transform.GetComponent<Image>().color = color;
		UpperCrosshair.transform.GetComponent<Image>().color = color;
		LowerCrosshair.transform.GetComponent<Image>().color = color;
	}

	/**
	 * A function for changing only the alpha value of the current color of the crosshairs
	 */
	public void setTransparency(float transparency){
		Color color = LeftCrosshair.transform.GetComponent<Image>().color;
		color.a = transparency;
		setColor(color);
	}

	/**
	 * A function for changing only the red value of the current color of the crosshairs
	 */
	public void setRed(float red){
		Color color = LeftCrosshair.transform.GetComponent<Image>().color;
		color.r = red;
		setColor(color);
	}

	/**
	 * A function for changing only the green value of the current color of the crosshairs
	 */
	public void setGreen(float green){
		Color color = LeftCrosshair.transform.GetComponent<Image>().color;
		color.g = green;
		setColor(color);
	}

	/**
	 * A function for changing only the blue value of the current color of the crosshairs
	 */
	public void setBlue(float blue){
		Color color = LeftCrosshair.transform.GetComponent<Image>().color;
		color.b = blue;
		setColor(color);
	}

}
