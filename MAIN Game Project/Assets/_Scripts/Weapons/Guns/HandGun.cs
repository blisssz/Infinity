using UnityEngine;
using System.Collections;

public class HandGun : Gun {

	private const int bulletsInMagazine = 18;
	private Animator anim;
	private int AimHash = Animator.StringToHash("Aim");
	private int ShootHash = Animator.StringToHash("Shoot");
	private int ReloadHash = Animator.StringToHash("Reload");
	private int isFiring = Animator.StringToHash("isFiring");
	private int ShootStateHash = Animator.StringToHash("Base Layer.HandsGun1Shoot");

	// Use this for initialization
	void Awake () {
		magazinesLeft = 5;
		bulletRange = 1000f;
		bulletOffset = 0f;
		bulletDamage = 10f;
		coolDownTime = 0.2f;
		reloadTime = 1f;
		timeStamp = Time.time;
		bulletsLeftInMagazine = bulletsInMagazine;
		anim = GetComponent<Animator>();
	}

	void Start () {
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
		if (reload == true && fire == false && magazinesLeft > 0 && bulletsLeftInMagazine < bulletsInMagazine) {
			Reload (bulletsInMagazine);
			anim.SetTrigger (ReloadHash);
		}

	}
	 

	public override void Shoot(float bulletRange, float bulletSpeed, float bulletOffset){
		//Call the base shoot method
		//this.StartCoroutine ("shootOff");

		if (anim.GetCurrentAnimatorStateInfo (0).nameHash == ShootStateHash) {
			Debug.Log ("got here");
			anim.SetTrigger(isFiring);
		}
		else{
			anim.SetTrigger (ShootHash);
		}

		//anim.SetTrigger (ShootHash);
		base.Shoot (bulletRange, bulletSpeed, bulletOffset);
		//Do muzzleflash particle effect / animation here !
	}

	IEnumerator shootOff() {


		if (anim.animation.IsPlaying("Shoot"))
		{
			Debug.Log ("got here");
			anim.animation["Shoot"].time = 0;
			anim.SetBool(ShootHash, true);
		}

		anim.SetBool (ShootHash, true);
		yield return new WaitForSeconds (0.05f);
		anim.SetBool (ShootHash, false);

	}

	public void equip(bool status){
		anim.SetBool (AimHash, status);
	}
}
