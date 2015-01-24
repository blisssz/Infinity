using UnityEngine;
using System.Collections;

/// <summary>
/// Player manager. stores interesting player data and important gameObject locations
/// (Attach this script to the main parent of the player)
/// </summary>

public class PlayerManager : MonoBehaviour {
	public static Vector3 playerPosition;
	
	public GameObject weaponAttachObject {get; set;}
	public GameObject projectileSpawnObject {get; set;}
	public GameObject playerHeadObject {get; set;}
	
	private GameObject currentWeapon {get; set;}
	
	// Weapons / tools
	public GameObject grapplingHook;
	public GameObject pogoStick;
	public GameObject blackHoleGun;
	public GameObject grapplingHookV2;
	public GameObject handGun;
	public GameObject SMG;
	public GameObject sniper;
	public GameObject jetpack;
	
	//UI
	public GameObject Crosshair;
	public AudioSource Sound;
//	private bool started=false;
	
	private bool switcher = false;
	
	public static int useWeaponID = 2;
	
	//	private int[] weaponID = {0, 1, 2, 3, 4};
	
	// example: private Weapon[] = {gun1, gun2...etc}
	
	private bool hasWeapon = false;
	
	// Use this for initialization
	void Start () {
		Crosshair=GameController.Crosshair;
		Sound=GetComponent<AudioSource>();
		if(Application.loadedLevelName.Equals("Doolhof")){
			Camera[] Cameras=GetComponentsInChildren<Camera>();
			for(int i=0; i<Cameras.Length;i++){
				if(Cameras[i].name.Equals ("Main Camera")){
					Cameras[i].farClipPlane=100;
					Cameras[i].clearFlags=CameraClearFlags.SolidColor;
				}
			}
		}

		if(useWeaponID==6){
			Camera[] Cameras=GetComponentsInChildren<Camera>();
			for(int i=0; i<Cameras.Length;i++){
				if(Cameras[i].name.Equals ("Main Camera")){
					//Cameras[i].clearFlags=CameraClearFlags.Skybox;
				}
			}
		}
		
		currentWeapon = null;
		
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		
		foreach (Transform child in allChildren){
			
			if (child.name.Equals ("Projectile Spawn Location")){
				projectileSpawnObject = child.gameObject;
			}
			
			if (child.name.Equals ("Weapon Attach Location")){
				weaponAttachObject = child.gameObject;
			}
			
			if (child.name.Equals ("Player_Head")){
				playerHeadObject = child.gameObject;
			}

			if (child.name.Equals("Main Camera")&&useWeaponID!=6){
				child.gameObject.AddComponent<CameraShake>();
			}
		}
		

		Crosshair.SetActive (false);
		SwitchWeapon();


		
	}
	
	// Update is called once per frame

	void Update () {
//		if(!started){
//			SwitchWeapon();
//			started=true;
//		}
		playerPosition = transform.position;
//		 spawn a tool/weapon
		bool key1 = KeyManager.key1 == 1;
		if(key1){
			SwitchWeapon();
		}
	}

	public void DestroyWeapon(){
		if( useWeaponID ==3){
			GrapplingHookV2 gh = currentWeapon.GetComponent<GrapplingHookV2>();
			gh.DestroyAll();
			setCrosshair (false);
			}
	}
	
	public void setCrosshair(bool switcher){
		Crosshair.SetActive (switcher);
	}
	
	public GameObject getCurrentWeapon(){
		return currentWeapon;
	}


	public void SwitchWeapon(){
		if (hasWeapon == false){
			
			 // tap key once
			
			// Spawn handgun
			if (useWeaponID == 4||useWeaponID == 5||useWeaponID == 6){
				if (switcher == false){
					if(currentWeapon == null){
						if(useWeaponID==4){
						currentWeapon = Instantiate(handGun, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.6f + weaponAttachObject.transform.up * 0.1f , weaponAttachObject.transform.rotation) as GameObject;
						}	else if(useWeaponID==5){
							currentWeapon = Instantiate(SMG, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.543f + weaponAttachObject.transform.up * 0.375f + weaponAttachObject.transform.forward * 0.21f , weaponAttachObject.transform.rotation) as GameObject;
							//Activate.SetCrossHair(25f);
						}	else if(useWeaponID==6){
							currentWeapon = Instantiate(sniper, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.543f + weaponAttachObject.transform.up * 0.433f + weaponAttachObject.transform.forward * 0.25f , weaponAttachObject.transform.rotation) as GameObject;
						}	
						currentWeapon.transform.parent = weaponAttachObject.transform;
						Activate.setActiveCustom(true);
						(Instantiate (jetpack, this.transform.position, this.transform.rotation) as GameObject).transform.parent = weaponAttachObject.transform;
					}
					Activate.setActiveCustom(true);
					switcher = true;
					setCrosshair(true);
					if(useWeaponID==4){
						currentWeapon.GetComponent<HandGun>().equip(true);
					}	else if(useWeaponID==5){
						currentWeapon.GetComponent<MachineGun>().equip(true);
					}	else if(useWeaponID==6){
						currentWeapon.GetComponent<Sniper>().equip(true);
						setCrosshair(false);
					}

				}
				else {
					switcher = false;
					Activate.setActiveCustom(false);
					setCrosshair(false);
					if(useWeaponID==4){
						currentWeapon.GetComponent<HandGun>().equip(false);
					}	else if(useWeaponID==5){
						currentWeapon.GetComponent<MachineGun>().equip(false);
					}	else if(useWeaponID==6){
						currentWeapon.GetComponent<Sniper>().equip(false);
					}
				}
			}
			
			
			// spawn weapon / tool
			if (currentWeapon == null){
				
				// spawn grapplingHook
				if (useWeaponID == 0){
					currentWeapon = Instantiate(grapplingHook, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;
					
					GrapplingHook gh = currentWeapon.GetComponent<GrapplingHook>();
					
					// projectile spawn location gameobject
					gh.projectileSpawnObject = projectileSpawnObject;
					setCrosshair(true);
					hasWeapon = true;
				}
				
				if (useWeaponID == 1){
					// pogostick spawn
					currentWeapon = Instantiate(pogoStick, this.transform.position, this.transform.rotation) as GameObject;
					float offsetY = 0.0f;
					float offsetZ = 0.45f;
					Vector3 offset = this.transform.position + this.transform.up * offsetY + this.transform.forward * offsetZ;
					currentWeapon.transform.position = offset;
					
					currentWeapon.transform.parent = this.transform;
					hasWeapon = true;
				}
				
				if (useWeaponID == 2){
					currentWeapon = Instantiate(blackHoleGun, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;
					setCrosshair (true);
					hasWeapon = true;
					//GrapplingHook gh = currentWeapon.GetComponent<GrapplingHook>();
					
					// projectile spawn location gameobject
					//gh.projectileSpawnObject = projectileSpawnObject;
					
					/*
					currentWeapon = Instantiate(blackHoleGun, this.transform.position, this.transform.rotation) as GameObject;
					float offsetY = 0.0f;
					float offsetZ = 0.45f;
					Vector3 offset = this.transform.position + this.transform.up * offsetY + this.transform.forward * offsetZ;
					currentWeapon.transform.position = offset;
					
					currentWeapon.transform.parent = this.transform;
					*/
				}
				
				// spawn grapplingHook
				if (useWeaponID == 3){
					currentWeapon = Instantiate(grapplingHookV2, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;
					
					GrapplingHookV2 gh = currentWeapon.GetComponent<GrapplingHookV2>();
					
					// projectile spawn location gameobject
					gh.projectileSpawnObject = projectileSpawnObject;
					setCrosshair(true);
					hasWeapon = true;
				}
				
			}
			// remove weapon when one is already active
			else if (currentWeapon != null){
				
				
				if (useWeaponID == 1){
					// pogostick delete
					Destroy(currentWeapon.gameObject);
					
				}
				
				if (useWeaponID == 2){
					// Destroy BlackHoleGun and a black hole if there is one left in the game
					currentWeapon.GetComponent<BlackHoleGun>().destroyBlackHole();
					Destroy(currentWeapon.gameObject);
					setCrosshair (false);
				}
				
				// Delete GrapplingHookV2
				if (useWeaponID == 3||useWeaponID == 0){
					GrapplingHookV2 gh = currentWeapon.GetComponent<GrapplingHookV2>();
					gh.DestroyAll();
					setCrosshair (false);
				}
				
			}
		}
	}
}
