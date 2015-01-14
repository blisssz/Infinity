using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Sniper : Gun {

	public AudioClip audioReload;

	private Animator anim;
	private int useHash = Animator.StringToHash("Use");
	private int fireHash = Animator.StringToHash("Fire");
	private int reloadHash = Animator.StringToHash("Reload");
	private int zoomHash = Animator.StringToHash("Zoom");
	private int shootState = Animator.StringToHash("Base Layer.SNIPERSHOT");
	private int aimState = Animator.StringToHash("Base Layer.SNIPERAIMIDLE");

	private List<Camera> cameras = new List<Camera>();
	private Camera sniperCamera;
	private bool camerazoomed = false;
	private bool reloading = false;
	private bool aimhold;
	private const float lerptime = 0.1f;
	private float startingFOV;
	private const float zoomFOV = 30f;
	private const float scrollSpeed = 25f;

	private bool shot = false;
	private float easeOut = 3f; //Handle a ease out of the zooming animation

	private const int bulletsInMagazine = 3;
	private const int startingMagazines = 5;

	// Use this for initialization
	void Awake () {
		magazineBulletsLeft = startingMagazines * bulletsInMagazine;
		bulletRange = 10000f;
		bulletOffset = 0f;
		bulletDamage = 50f;
		coolDownTime = 0.5f;
		reloadTime = 1.317f;
		timeStamp = Time.time;
		bulletsLeftInMagazine = bulletsInMagazine;
		anim = GetComponent<Animator>();
	}

	void Start () {
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		cameras = Camera.allCameras.ToList<Camera>();
		sniperCamera = GameObject.Find("Sniper Camera").GetComponent<Camera>();
		for(int i = 0; i < cameras.Count; i++){
			if(cameras[i] == sniperCamera){
				cameras.Remove (sniperCamera);
			}
		}
		startingFOV = Camera.main.fieldOfView;
		playerHead = GameObject.Find ("Player_Head");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(gunEquipped == true){
			bool fire = KeyManager.leftMouse == 1;
			bool reload = KeyManager.rightMouse == 1;
			bool aim = KeyManager.leftShift == 1;
			aimhold = KeyManager.leftShift == 2;
			bool aimRelease = KeyManager.leftShift == 3;
			
			//If left mouse button is pressed and the player is zoomed in, fire
			if (fire == true && timeStamp <= Time.time && aimhold == true) {
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
			if (reload == true && fire == false && bulletsLeftInMagazine < bulletsInMagazine) {
				Reload (bulletsInMagazine);
			}

			//If left shift is held down, aim
			if (aim == true) {
				anim.SetBool(zoomHash, true);
			}
			else if(aimhold == true){
				if(reloading == true){
					foreach(Camera camera in cameras){
						camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, startingFOV, lerptime);
					}
					camerazoomed = true;
					Zoom (false);
				}
				else{
					foreach(Camera camera in cameras){
						camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomFOV, lerptime);
					}
					camerazoomed = true;
					if(Camera.main.fieldOfView < 42f && anim.GetCurrentAnimatorStateInfo (0).nameHash == aimState){
						Zoom (true);
					}
					sniperCamera.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
					sniperCamera.fieldOfView = Mathf.Clamp(sniperCamera.fieldOfView, 1f, 9.45f);
				}
			}
			else if(aimRelease == true){
				anim.SetBool(zoomHash, false);
			}
			else if(camerazoomed == true){
				foreach(Camera camera in cameras){
					camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, startingFOV, lerptime);
				}
				Zoom (false);
				if(Camera.main.fieldOfView > 89.5f){
					foreach(Camera camera in cameras){
						camera.fieldOfView = 90;
					}
				}
				if(Camera.main.fieldOfView == 90){
					camerazoomed = false;
				}
			}

			if(shot == true && easeOut > 0){
				if(playerHead != null){
					playerHead.transform.parent.GetComponent<MouseLook>().gunRotationY = -easeOut;
				}
				easeOut -= 15f * easeOut * Time.deltaTime;
			}
		}
	}
	
	
	public override void Shoot(float bulletRange, float bulletSpeed, float bulletOffset){
		//Call the base zoomed shoot method
		base.Shoot (bulletRange, bulletSpeed, bulletOffset, true);
		//Do muzzleflash particle effect / animation here !
		anim.SetBool (fireHash, true);
		shot = true;
		easeOut = 3f;
	}

	public override void Reload(int bulletsInMagazine){
		if(magazineBulletsLeft > 0){
			base.Reload (bulletsInMagazine);
			this.StartCoroutine("reload");
		}
	}

	public void equip(bool status){
		anim.SetBool (useHash, status);
		gunEquipped = status;
	}
	
	private void Zoom(bool status){
		sniperCamera.enabled = status; //Enable or disable the zoomable sniper camera
		//Camera.main.GetComponent<Blur>().enabled = status; //Enable or disable the blur effect on the main camera
	}

	//Handles reload animation and synced reload-audio playback
	IEnumerator reload() {
		float extraTime = 0f;
		if (camerazoomed == true) {
			extraTime = 0.3f;	 //If camera is zoomed in, delay reloading sound 0.3 seconds
		}
		reloading = true;
		anim.SetBool(reloadHash, true);

		yield return new WaitForSeconds (0.7f + extraTime);

		AudioSource.PlayClipAtPoint(audioReload, transform.position, 1f);
		anim.SetBool(reloadHash, false);

		yield return new WaitForSeconds (reloadTime - (0.7f + extraTime));
		yield return new WaitForSeconds (0.525f);
		reloading = false;
	}
}