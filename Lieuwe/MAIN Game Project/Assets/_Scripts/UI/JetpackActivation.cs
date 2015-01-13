using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class JetpackActivation : MonoBehaviour {

	public GameObject fuelStatus;
	private Text textValue;
	public static JetpackActivation instance;
	private static bool switcher = true;
	
	// Use this for initialization
	void Start () {
		textValue = this.GetComponent<Text> ();
		instance = this;
		textValue.CrossFadeAlpha (0f, 0f, false);
		fuelStatus.SetActive(false);
	}

	public static void setText(bool status){
		instance.StartCoroutine ("Fade", status);
//		if (status == true) {
//			textValue.text = "Jetpack enabled";
//		}
//		else {
//			textValue.text = "Jetpack disabled";
//		}
//		instance.StartCoroutine (Fade ());
	}

	IEnumerator Fade(bool status) {
		if (status == true) {
			textValue.text = "Jetpack enabled";
			fuelStatus.SetActive(true);
		}
		else {
			textValue.text = "Jetpack disabled";
			fuelStatus.SetActive(false);
		}
		textValue.CrossFadeAlpha (0f, 0f, false);
		textValue.CrossFadeAlpha (1f, 0.3f, false);
		yield return new WaitForSeconds(1f);
		textValue.CrossFadeAlpha (0f, 1f, false);
	}
}
