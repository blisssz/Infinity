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
	public GameObject UIfixed;
	public GameObject UI;
	private GameObject Crosshair;
	
	private bool switcher = false;
	
	public static int useWeaponID = 6;
	
	//	private int[] weaponID = {0, 1, 2, 3, 4};
	
	// example: private Weapon[] = {gun1, gun2...etc}
	
	private bool hasWeapon = false;
	
	// Use this for initialization
	void Start () {
		
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
		}
		
		Crosshair = Instantiate(UIfixed) as GameObject;
		Crosshair.SetActive (false);

		
	}
	
	// Update is called once per frame
	void Awake () {

		Instantiate(UI);
	}

	void Update () {
		playerPosition = transform.position;
		// spawn a tool/weapon
		if (hasWeapon == false){
			
			bool key1 = KeyManager.key1 == 1; // tap key once
			
			// Spawn handgun
			if (key1 == true && useWeaponID == 4){
				if (switcher == false){
					if(currentWeapon == null){
						currentWeapon = Instantiate(handGun, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.6f + weaponAttachObject.transform.up * 0.1f , weaponAttachObject.transform.rotation) as GameObject;
						currentWeapon.transform.parent = weaponAttachObject.transform;
						Activate.setActiveCustom(true);
						(Instantiate (jetpack, this.transform.position, this.transform.rotation) as GameObject).transform.parent = weaponAttachObject.transform;
					}
					switcher = true;
					setCrosshair(true);
					currentWeapon.GetComponent<HandGun>().equip(true);
				}
				else {
					switcher = false;
					setCrosshair(false);
					currentWeapon.GetComponent<HandGun>().equip(false);
				}
			}
			
			//Spawn SMG
			if (key1 == true && useWeaponID == 5){
				if (switcher == false){
					if(currentWeapon == null){
						currentWeapon = Instantiate(SMG, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.543f + weaponAttachObject.transform.up * 0.375f + weaponAttachObject.transform.forward * 0.21f , weaponAttachObject.transform.rotation) as GameObject;
						currentWeapon.transform.parent = weaponAttachObject.transform;
						Activate.setActiveCustom(true);
						(Instantiate (jetpack, this.transform.position, this.transform.rotation) as GameObject).transform.parent = weaponAttachObject.transform;
					}
					switcher = true;
					setCrosshair(true);
					currentWeapon.GetComponent<MachineGun>().equip(true);
				}
				else {
					switcher = false;
					setCrosshair(false);
					currentWeapon.GetComponent<MachineGun>().equip(false);
				}
			}

			//Spawn Sniper
			if (key1 == true && useWeaponID == 6){
				if (switcher == false){
					if(currentWeapon == null){
						currentWeapon = Instantiate(sniper, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.543f + weaponAttachObject.transform.up * 0.433f + weaponAttachObject.transform.forward * 0.25f , weaponAttachObject.transform.rotation) as GameObject;
						currentWeapon.transform.parent = weaponAttachObject.transform;
						Activate.setActiveCustom(true);
						(Instantiate (jetpack, this.transform.position, this.transform.rotation) as GameObject).transform.parent = weaponAttachObject.transform;
					}
					switcher = true;
					currentWeapon.GetComponent<Sniper>().equip(true);
				}
				else {
					switcher = false;
					currentWeapon.GetComponent<Sniper>().equip(false);
				}
			}
			
			
			// spawn weapon / tool
			if (key1 == true && currentWeapon == null){
				
				// spawn grapplingHook
				if (useWeaponID == 0){
					currentWeapon = Instantiate(grapplingHook, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;
					
					GrapplingHook gh = currentWeapon.GetComponent<GrapplingHook>();
					
					// projectile spawn location gameobject
					gh.projectileSpawnObject = projectileSpawnObject;
					setCrosshair(true);
					
				}
				
				if (useWeaponID == 1){
					// pogostick spawn
					currentWeapon = Instantiate(pogoStick, this.transform.position, this.transform.rotation) as GameObject;
					float offsetY = 0.0f;
					float offsetZ = 0.45f;
					Vector3 offset = this.transform.position + this.transform.up * offsetY + this.transform.forward * offsetZ;
					currentWeapon.transform.position = offset;
					
					currentWeapon.transform.parent = this.transform;
					
				}
				
				if (useWeaponID == 2){
					currentWeapon = Instantiate(blackHoleGun, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;
					setCrosshair (true);
					
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
					
				}
				
			}
			// remove weapon when one is already active
			else if (key1 == true && currentWeapon != null){
				
				// Delete GrapplingHook
				if (useWeaponID == 0){
					GrapplingHook gh = currentWeapon.GetComponent<GrapplingHook>();
					gh.DestroyAll();
					setCrosshair (false);
				}
				
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
				if (useWeaponID == 0){
					GrapplingHookV2 gh = currentWeapon.GetComponent<GrapplingHookV2>();
					gh.DestroyAll();
					setCrosshair (false);
				}
				
			}
		}
	}
	
	public void setCrosshair(bool switcher){
		Crosshair.SetActive (switcher);
	}
	
	public GameObject getCurrentWeapon(){
		return currentWeapon;
	}
}
