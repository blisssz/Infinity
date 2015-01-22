using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHP : MonoBehaviour {

	public Text jetpackActivationText;
	public Text outOfAmmoText;
	public SlidingBar slider;
	public GameObject HPbar;

 	// Start mag alleen uitgevoerd worden als dit een boss level is
	void Start () {
//		if( THISLEVELISABOSSLEVEL? ){
			jetpackActivationText.alignment = TextAnchor.LowerCenter;
			outOfAmmoText.alignment = TextAnchor.LowerCenter;
			HPbar.SetActive(true);
//		}

	}

	//	slider.setValueBossHealth(BOSSCURRENTHEALTH, BOSSMAXHEALTH, true) 	//Roep deze functie wanneer de boss health geupdate moet worden 
}
