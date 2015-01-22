using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class JetpackActivation : MonoBehaviour {
	
	public GameObject fuelStatus;
	public Image backgroundNoFuel;
	public Image backgroundWithFuel;
	private Text textValue;
	public static JetpackActivation instance;
	//	private static bool switcher = true;
	
	// Use this for initialization
	void Start () {
		textValue = this.GetComponent<Text> ();
		instance = this;
		textValue.CrossFadeAlpha (0f, 0f, false);

		if(PlayerManager.useWeaponID==4||PlayerManager.useWeaponID==5||PlayerManager.useWeaponID==6){
			fuelStatus.SetActive(true);
			backgroundWithFuel.enabled = true;
			backgroundNoFuel.enabled = false;
		} else {
			fuelStatus.SetActive(false);
			backgroundWithFuel.enabled = false;
			backgroundNoFuel.enabled = true;
		
		}
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

	public void SetActive(bool active){
		fuelStatus.SetActive (active);
	}

	public void Reset(){
		fuelStatus.SetActive(true);
		backgroundNoFuel.enabled = false;
		backgroundWithFuel.enabled = true;
	}
	
	IEnumerator Fade(bool status) {
		if (status == true) {
			textValue.text = "Jetpack enabled";
			fuelStatus.SetActive(true);
			backgroundNoFuel.enabled = false;
			backgroundWithFuel.enabled = true;
		}
		else {
			textValue.text = "Jetpack disabled";
			fuelStatus.SetActive(false);
			backgroundNoFuel.enabled = true;
			backgroundWithFuel.enabled = false;
		}
		textValue.CrossFadeAlpha (0f, 0f, false);
		textValue.CrossFadeAlpha (1f, 0.3f, false);
		yield return new WaitForSeconds(1f);
		textValue.CrossFadeAlpha (0f, 1f, false);
	}


}
