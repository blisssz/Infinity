using UnityEngine;
using System.Collections;

public class MachineGun : Gun {

	private const int bulletsInMagazine = 100;
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
	private float lerptime = 0.1f;
	private float startingFOV;
	private float zoomFOV = 60f;

	private float bulletOffsetHip = 0.06f;
	private float bulletOffsetZoomed = 0.001f;

	private Vector3 totalRotation = new Vector3(0,0,0);
	private Vector3 totalRotationTemp = new Vector3(0,0,0);

	// Use this for initialization
	void Awake () {
		magazinesLeft = 5;
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
		MagazinesLeft.textValue.text = magazinesLeft.ToString();
		cameras = Camera.allCameras;
		startingFOV = Camera.main.fieldOfView;
		playerHead = GameObject.Find ("Player_Head");
	}
	
	// Update is called once per frame
	void LateUpdate () {
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
				anim.SetBool (fireHash, false);
				BulletInfo.instance.StopAllCoroutines ();
				BulletInfo.instance.StartCoroutine ("Fade");
				doOnce = false;
			}
		}
		//Left mouse button is released
		if (firereleased == true) {
			anim.SetBool (fireHash, false);
			if(bulletsLeftInMagazine <= 0){
				doOnce = true;
			}
		}

		//If right mouse button is pressed, reload
		if (reload == true && firehold == false && magazinesLeft > 0 && bulletsLeftInMagazine < bulletsInMagazine) {
			Reload (bulletsInMagazine);
			this.StartCoroutine("reload");
		}

		if(totalRotation.magnitude > 0 && (firehold == false || doOnce == false) && playerHead != null){
			Vector3 rotation = -totalRotationTemp * (10f * Time.deltaTime);
			if(rotation.magnitude > totalRotation.magnitude){
				rotation = -totalRotation;
			}
			playerHead.transform.Rotate(rotation);
			totalRotation += rotation;
		}

		/*
		//If right mouse button is pressed, reload
		if (reload == true && firehold == false && magazinesLeft > 0) {
			bulletsLeftInMagazine = bulletsInMagazine;
			magazinesLeft--;
			MagazinesLeft.textValue.text = magazinesLeft.ToString();
			BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
			BulletInfo.instance.StopAllCoroutines();
			BulletInfo.textValue.CrossFadeAlpha (0f, 0f, false);
		}
		*/

		//If left shift is held down, aim
		if (aim == true) {
			anim.SetBool(zoomHash, true);
			bulletOffset = bulletOffsetZoomed;
		}
		else if(aimhold == true){
			if(reloading == true){
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
		else if(aimRelease == true){
			anim.SetBool(zoomHash, false);
			bulletOffset = bulletOffsetHip;
		}
		else if(camerazoomed == true){
			foreach(Camera camera in cameras){
				camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, startingFOV, lerptime);
			}
			if(Camera.main.fieldOfView == 90){
				camerazoomed = false;
			}
		}

	}
	

	public override void Shoot(float bulletRange, float bulletSpeed, float bulletOffset){
		//Call the base shoot method
		if(aimhold == false){
			//Player is not pressing shift
			base.Shoot (bulletRange, bulletSpeed, bulletOffset);
		}
		else{
			//Player is pressing shift
			base.Shoot (bulletRange, bulletSpeed, bulletOffset, true);

		}
		if(playerHead != null){
			Vector3 rotation = new Vector3(Random.Range(-.4f, .2f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
			totalRotation += rotation;
			totalRotationTemp = totalRotation;
			playerHead.transform.Rotate (rotation);
			anim.SetBool (fireHash, true);
		}
	}

	IEnumerator reload() {
		reloading = true;
		anim.SetBool(reloadHash, true);
		yield return new WaitForSeconds (0.50f);
		anim.SetBool(reloadHash, false);
		yield return new WaitForSeconds (reloadTime - 0.50f);
		reloading = false;
	}

	public void equip(bool status){
		anim.SetBool (useHash, status);
	}
}
