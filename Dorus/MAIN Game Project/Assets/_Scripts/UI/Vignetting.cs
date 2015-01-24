using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Vignetting : MonoBehaviour {

	private static Vignetting thisInstance;
	private static Image vignetting;
	private static float currentAlpha = 0f;
	private static bool pulsating = false;

	// Use this for initialization
	void Start () {
		vignetting = this.GetComponent<Image> ();
		vignetting.CrossFadeAlpha (currentAlpha, 0f, false);
		thisInstance = this;
	}


	public static void FadeTheAlpha(float percentage){
		if(percentage < 0.2f){
		currentAlpha = MapValues (percentage, 0f, 0.20f, 1f, 0f);
		} else {
			currentAlpha=0f;

		}
		if(pulsating == false){
			vignetting.CrossFadeAlpha(currentAlpha, 0.3f, false);
		}
	}

	public static void PlayerHit(){
		thisInstance.StartCoroutine (PulseOnce ());
	}
	
	private static IEnumerator PulseOnce(){
		pulsating = true;
		vignetting.CrossFadeAlpha(1f, 0.1f, false);
		yield return new WaitForSeconds (0.1f);
		vignetting.CrossFadeAlpha(currentAlpha, 0.1f, false);
		pulsating = false;
	}

	private static float MapValues(float x, float inMin, float inMax, float outMin, float outMax){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
