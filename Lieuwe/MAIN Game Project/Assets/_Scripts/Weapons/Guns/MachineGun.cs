using UnityEngine;
using System.Collections;

public class MachineGun : Gun {

	public AudioClip reloadRemove;
	public AudioClip reloadAttach;
	
	private bool doOnce = true;
	private Animator anim;
	private int useHash = Animator.StringToHash("Use");
	private int fireHash = Animator.StringToHash("Fire");
	private int reloadHash = Animator.StringToHash("Reload");
	private int zoomHash = Animator.StringToHash("Zoom");

	private Camera[] cameras;
	private bool camerazoomed = false;
	private bool reloading = false;
	private bool aimhold;
	private const float lerptime = 0.1f;
	private float startingFOV;
	private const float zoomFOV = 60f;

	private const float bulletOffsetHip = 0.06f;
	private const float bulletOffsetZoomed = 0.001f;

//	private const int 
	private const int startingMagazines = 5;

//	private Vector3 totalRotation = new Vector3(0,0,0);
//	private Vector3 totalRotationTemp = new Vector3(0,0,0);

	private MouseLook mouselook;
	private PlayerManager playerManager;

	// Use this for initialization
	void Awake () {
		bulletsInMagazine = 100;
		magazineBulletsLeft = startingMagazines * bulletsInMagazine;
		bulletRange = 1000f;
		bulletOffset = bulletOffsetHip;
		bulletDamage = 10f;
		coolDownTime = 0.05f; //20 bullets per second, 1200 rounds per minute.
		reloadTime = 3.5f;
		timeStamp = Time.time;
		bulletsLeftInMagazine = bulletsInMagazine;
		anim = GetComponent<Animator>();

	}

	void Start () {
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		cameras = Camera.allCameras;
		startingFOV = Camera.main.fieldOfView;
		playerHead = GameObject.Find ("Player_Head");
		if(playerHead != null){
			mouselook = playerHead.transform.parent.GetComponent<MouseLook>();
			playerManager = playerHead.transform.root.GetComponent<PlayerManager>();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(gunEquipped == true){
			bool firehold = KeyManager.leftMouse == 2;
			bool firereleased = KeyManager.leftMouse == 3;
			bool reload = KeyManager.rightMouse == 1;
			bool aim = KeyManager.leftShift == 1;
			aimhold = KeyManager.leftShift == 2;
			bool aimRelease = KeyManager.leftShift == 3;

			//If left mouse button is held down, fire
			if (firehold == true && timeStamp <= Time.time) {
				if(bulletsLeftInMagazine > 0){
					Shoot (bulletRange, bulletOffset, bulletDamage);
				}
				else if (doOnce == true) {
					doOnce = false;
					anim.SetBool (fireHash, false);
					if(magazineBulletsLeft <= 0){
						OutOfAmmo.instance.StartCoroutine("FadeOnHold");
					}
					Reload (bulletsInMagazine);
				}
			}

			//Left mouse button is released, stop shooting animation
			if (firereleased == true) {
				anim.SetBool (fireHash, false);
			}

			//If magazine is emtpy, automatically reload
			if (bulletsLeftInMagazine <= 0){
				Reload (bulletsInMagazine);
			}

			//If right mouse button is pressed, reload
			if (reload == true && firehold == false && bulletsLeftInMagazine < bulletsInMagazine) {
				Reload (bulletsInMagazine);
			}

	 		
			if (aim == true) {	//If left shift is pressed, start zoom animation and change the bullet offset to be more precise
				anim.SetBool(zoomHash, true);
				bulletOffset = bulletOffsetZoomed;
				playerManager.setCrosshair(false);
			}
			else if(aimhold == true){	//If left shift is being held down zoom the camera
				if(reloading == true){
					//If the player is reloading while being zoomed in, reset the zoom of the camera back to normal
					foreach(Camera camera in cameras){
						camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, startingFOV, lerptime);
					}
					camerazoomed = true;
				}
				else{
					foreach(Camera camera in cameras){
						camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomFOV, lerptime);
					}
					camerazoomed = true;
				}
			}
			else if(aimRelease == true){	//If the player just released left shift, start zoom out animation and change the bullet offset to be less precise
				anim.SetBool(zoomHash, false);
				bulletOffset = bulletOffsetHip;
			}
			else if(camerazoomed == true){	//No interaction with shift key in this loop of void lateUpdate, zoom out the camera to normal field of view
				foreach(Camera camera in cameras){
					camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, startingFOV, lerptime);
				}

				if(Camera.main.fieldOfView > 80f && reloading == false){
					playerManager.setCrosshair(true);
				}

				//If camera is almost completely zoomed back out, set it to it's final zoomed out value.
				if(Camera.main.fieldOfView > 89.5f){
					foreach(Camera camera in cameras){
						camera.fieldOfView = 90;
						camerazoomed = false;
					}
				}
			}

	/*
			if(totalRotation.magnitude > 0 && (firehold == false || doOnce == false) && playerHead != null){
				Vector3 rotation = -totalRotationTemp * (10f * Time.deltaTime);
				if(rotation.magnitude > totalRotation.magnitude){
					rotation = -totalRotation;
				}
				playerHead.transform.Rotate(rotation);
				totalRotation += rotation;
			}
	*/

		/*
			//If right mouse button is pressed, reload
			if (reload == true && firehold == false && magazineBulletsLeft > 0) {
				bulletsLeftInMagazine = bulletsInMagazine;
				magazineBulletsLeft--;
				MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
				BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
				BulletInfo.instance.StopAllCoroutines();
				BulletInfo.textValue.CrossFadeAlpha (0f, 0f, false);
			}
		*/


		}
	}

	public override void Shoot(float bulletRange, float bulletSpeed, float bulletOffset){
		if(aimhold == false){
			//Player is not pressing shift, do a normal shot
			base.Shoot (bulletRange, bulletSpeed, bulletOffset);
		}
		else{
			//Player is pressing shift, do a zoomed in shot
			base.Shoot (bulletRange, bulletSpeed, bulletOffset, true);
		}

		anim.SetBool (fireHash, true);

		if(playerHead != null){
/*
			Vector3 rotation = new Vector3(Random.Range(-.3f, .1f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
			totalRotation += rotation;
			totalRotationTemp = totalRotation;
			playerHead.transform.Rotate (rotation);
			anim.SetBool (fireHash, true);
*/			
			//Add a random camera shake
			mouselook.gunRotationY = Random.Range(-.3f, .1f);
			mouselook.gunRotationX = Random.Range(-.1f, .1f);
		}
	}

	public override void Reload(int bulletsInMagazine){	
		if(magazineBulletsLeft > 0){
			this.StartCoroutine("reload");
			base.Reload (bulletsInMagazine);
		}
		doOnce = true;
	}

	public void equip(bool status){
		anim.SetBool (useHash, status);
		gunEquipped = status;
	}

	//Handles reload animation and synced reload-audio playback
	IEnumerator reload() {
		float extraTime = 0f;
		if (camerazoomed == true) {
			extraTime = 0.14f; //If camera is zoomed in, delay reloading sounds 0.14 seconds
		}
		reloading = true;
		playerManager.setCrosshair(false);
		anim.SetBool(reloadHash, true);

		yield return new WaitForSeconds (0.50f + extraTime);

		AudioSource.PlayClipAtPoint(reloadRemove, transform.position, 1f);
		anim.SetBool(reloadHash, false);

		yield return new WaitForSeconds (1.50f);

		AudioSource.PlayClipAtPoint(reloadAttach, transform.position, 1f);
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();

		yield return new WaitForSeconds (reloadTime - (2f + extraTime));

		reloading = false;
		if(aimhold == false) {
			playerManager.setCrosshair(true);
		}
	}
}