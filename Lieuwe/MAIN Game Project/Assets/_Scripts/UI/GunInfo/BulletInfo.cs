using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BulletInfo : MonoBehaviour {
	
	private Text textValue;
	public static BulletInfo instance;
	
//	private static Color tempColor = Color.black;
//	private static float time = totalTime;
//	private static bool fading = false;
//	public static Animation animation;
//	private const float totalTime = 1f;

	// Use this for initialization
	void Start () {
		textValue = this.GetComponent<Text> ();
		instance = this;
		textValue.CrossFadeAlpha (0f, 0f, false);
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
/*
	public static void setActive(){
		textValue.enabled = true;
		instance.StopAllCoroutines ();
		instance.StartCoroutine ("Fade");
	}
*/

	IEnumerator Fade() {
		textValue.CrossFadeAlpha (0f, 0f, false);
		textValue.CrossFadeAlpha (1f, 0.3f, false);
		yield return new WaitForSeconds(1f);
		textValue.CrossFadeAlpha (0f, 0.3f, false);
	}
}
