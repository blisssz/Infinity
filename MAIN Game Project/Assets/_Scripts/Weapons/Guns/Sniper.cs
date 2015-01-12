using UnityEngine;
using System.Collections;

public class Sniper : Gun {

	private const int bulletsInMagazine = 10;
	
	
	// Use this for initialization
	void Start () {
		magazinesLeft = 5;
		bulletRange = 10000f;
		bulletOffset = 0f;
		bulletDamage = 25f;
		coolDownTime = 0.5f;
		reloadTime = 1f;
		timeStamp = Time.time;
		bulletsLeftInMagazine = bulletsInMagazine;
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		MagazinesLeft.textValue.text = magazinesLeft.ToString();
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		bool fire = KeyManager.leftMouse == 1;
		bool reload = KeyManager.rightMouse == 1;
		
		//If left mouse button is pressed, fire
		if (fire == true && timeStamp <= Time.time) {
			if (bulletsLeftInMagazine > 0){
				Shoot (bulletRange, bulletOffset, bulletDamage);
			}
			else{
				BulletInfo.instance.StopAllCoroutines ();
				BulletInfo.instance.StartCoroutine ("Fade");
			}
		}
		
		//If right mouse button is pressed, reload
		if (reload == true && fire == false && magazinesLeft > 0) {
			Reload (bulletsInMagazine);
		}
		
	}
	
	
	public override void Shoot(float bulletRange, float bulletSpeed, float bulletOffset){
		//Call the base shoot method
		base.Shoot (bulletRange, bulletSpeed, bulletOffset);
		//Do muzzleflash particle effect / animation here !
	}

}
