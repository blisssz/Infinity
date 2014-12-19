using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BulletInfo : MonoBehaviour {
	
	public static Text textValue;
	public static Animation animation;
	public static BulletInfo instance;
	private static Color tempColor = Color.black;
	private const float totalTime = 1f;
	private static float time = totalTime;
	private static bool fading = false;


	// Use this for initialization
	void Start () {
		textValue = GetComponent<Text> ();
		textValue.CrossFadeAlpha (0f, 0f, false);
		instance = this;
	}

	void LateUpdate () {


		/*
		if (tempColor.a < 1f) {
			if (fading == false){
				//Fade In
				tempColor.a += 1.25f * Time.deltaTime;
				textValue.color = tempColor;
				time = totalTime;
			}
			else{
				//Fade Out
				tempColor.a -= 1.25f * Time.deltaTime;
				textValue.color = tempColor;
			}
		}
		else if(time <= 0) {
			//Fade Out
			fading = true;

		}
		else{
			//Don't do anything
			time -= Time.deltaTime;
		}
		*/
	}

	public static void setActive(){
		textValue.enabled = true;
		instance.StopAllCoroutines ();
		instance.StartCoroutine ("Fade");
	}

	IEnumerator Fade() {
		textValue.CrossFadeAlpha (0f, 0f, false);
		textValue.CrossFadeAlpha (1f, 0.3f, false);
		yield return new WaitForSeconds(1f);
		textValue.CrossFadeAlpha (0f, 1f, false);
	}
}
