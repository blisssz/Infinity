using UnityEngine;
using System.Collections;

public class HandGun : Gun {

	public AudioClip audioReload;
	
	private Animator anim;
	private int AimHash = Animator.StringToHash("Aim");
	private int ShootHash = Animator.StringToHash("Shoot");
	private int ReloadHash = Animator.StringToHash("Reload");
	private int isFiring = Animator.StringToHash("isFiring");
	private int ShootStateHash = Animator.StringToHash("Base Layer.HandsGun1Shoot");
//	private const int 
	private const int startingMagazines = 5;
	
	// Use this for initialization
	void Awake () {
		bulletsInMagazine = 18;
		magazineBulletsLeft = startingMagazines * bulletsInMagazine;
		bulletRange = 1000f;
		bulletOffset = 0f;
		bulletDamage = 15f;
		coolDownTime = 0.2f;
		reloadTime = 1.5f;
		timeStamp = Time.time;
		bulletsLeftInMagazine = bulletsInMagazine;
		anim = GetComponent<Animator>();

	}

	void Start () {
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(gunEquipped == true){
			bool fire = KeyManager.leftMouse == 1;
			bool reload = KeyManager.rightMouse == 1;

			//If left mouse button is pressed, fire
			if (fire == true && timeStamp <= Time.time) {
				if(bulletsLeftInMagazine > 0){
					Shoot (bulletRange, bulletOffset, bulletDamage);
				}
				else if(magazineBulletsLeft <= 0){
					OutOfAmmo.instance.StartCoroutine("Fade");
				}
			}

			//If magazine is emtpy, automatically reload
			if (bulletsLeftInMagazine == 0){
				Reload (bulletsInMagazine);
			}

			//If right mouse button is pressed, reload
			if (reload == true && fire == false && magazineBulletsLeft > 0 && bulletsLeftInMagazine < bulletsInMagazine) {
				Reload (bulletsInMagazine);
			}
		}
	}
	 

	public override void Shoot(float bulletRange, float bulletSpeed, float bulletOffset){
		//Call the base shoot method
		//this.StartCoroutine ("shootOff");

		if (anim.GetCurrentAnimatorStateInfo (0).nameHash == ShootStateHash) {
			//Handles a rare scenario when a new shot is already fired before shooting animation is finished
			anim.SetTrigger(isFiring);
		}
		else{
			anim.SetTrigger (ShootHash);
		}

		//anim.SetTrigger (ShootHash);
		base.Shoot (bulletRange, bulletSpeed, bulletOffset);
	}

	public override void Reload(int bulletsInMagazine){
		if(magazineBulletsLeft > 0){
			base.Reload (bulletsInMagazine);
			this.StartCoroutine("reload");
		}
	}

	//Handles reload animation and synced reload-audio playback
	IEnumerator reload() {
		anim.SetTrigger (ReloadHash);
		yield return new WaitForSeconds (0.65f);
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		AudioSource.PlayClipAtPoint(audioReload, transform.position, 1f);
	}

/* currently not used
	//Handles a rare scenario when a new shot is already fired before shooting animation is finished
	IEnumerator shootOff() {
		if (anim.animation.IsPlaying("Shoot"))
		{
			anim.animation["Shoot"].time = 0;
			anim.SetBool(ShootHash, true);
		}

		anim.SetBool (ShootHash, true);
		yield return new WaitForSeconds (0.05f);
		anim.SetBool (ShootHash, false);
	}
*/

	public void equip(bool status){
		anim.SetBool (AimHash, status);
		gunEquipped = status;
	}
}
