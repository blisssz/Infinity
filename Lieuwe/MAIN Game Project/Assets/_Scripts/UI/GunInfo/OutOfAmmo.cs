using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class OutOfAmmo : MonoBehaviour {
	
	private Text textValue;
	public static OutOfAmmo instance;
	private static bool switcher = true;
	
	// Use this for initialization
	void Start () {
		textValue = this.GetComponent<Text> ();
		instance = this;
		textValue.CrossFadeAlpha (0f, 0f, false);
	}

	IEnumerator FadeOnHold() {
		if(switcher == true){
			switcher = false;
			textValue.CrossFadeAlpha (0f, 0f, false);
			textValue.CrossFadeAlpha (1f, 0.3f, false);
			yield return new WaitForSeconds(1f);
			if(KeyManager.leftMouse != 2){
				textValue.CrossFadeAlpha (0f, 1f, false);
				switcher = true;
			}
		}
		else {
			yield return new WaitForSeconds(1f);
			if(KeyManager.leftMouse != 2){
				textValue.CrossFadeAlpha (0f, 0.5f, false);
				switcher = true;
			}
		}

	}

	IEnumerator Fade() {
		textValue.CrossFadeAlpha (0f, 0f, false);
		textValue.CrossFadeAlpha (1f, 0.3f, false);
		yield return new WaitForSeconds(1f);
		textValue.CrossFadeAlpha (0f, 1f, false);
	}
}
