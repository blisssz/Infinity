using UnityEngine;
using System.Collections;

/// <summary>
/// Player manager. stores interesting player data and important gameObject locations
/// (Attach this script to the main parent of the player)
/// </summary>

public class PlayerManager : MonoBehaviour {

	private GameObject thisPlayer;

	public GameObject weaponAttachObject {get; set;}
	public GameObject projectileSpawnObject {get; set;}
	public GameObject playerHeadObject {get; set;}

	private GameObject currentWeapon {get; set;}

	// Weapons / tools
	public GameObject grapplingHook;
	public GameObject pogoStick;
	public GameObject blackHoleGun;
	public GameObject handGun;
	public GameObject SMG;

	//UI
	public GameObject UIfixed;
	private GameObject Crosshair;

	private bool switcher = false;
	
	public int useWeaponID = 0;

	private int[] weaponID = {0, 1, 2, 3, 4};

	// example: private Weapon[] = {gun1, gun2...etc}

	private bool hasWeapon = false;

	// Use this for initialization
	void Start () {
		thisPlayer = this.gameObject;

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
	void Update () {

		// spawn a tool/weapon
		if (hasWeapon == false){

			bool key1 = KeyManager.key1 == 1; // tap key once

			// Spawn handgun
			if (key1 == true && useWeaponID == 3){
				if (switcher == false){
					if(currentWeapon == null){
						currentWeapon = Instantiate(handGun, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.6f + weaponAttachObject.transform.up * 0.1f , weaponAttachObject.transform.rotation) as GameObject;
						currentWeapon.transform.parent = weaponAttachObject.transform;
						Activate.setActiveCustom(true);
					}
					switcher = true;
					setCrosshair(true);
					currentWeapon.GetComponent<HandGun>().equip(true);
				}
				else {
					switcher = false;
					setCrosshair(false);
					currentWeapon.GetComponent<HandGun>().equip(true);
				}
			}

			//Spawn SMG
			if (key1 == true && useWeaponID == 4){
				if (switcher == false){
					if(currentWeapon == null){
						currentWeapon = Instantiate(SMG, weaponAttachObject.transform.position - weaponAttachObject.transform.right * 0.543f + weaponAttachObject.transform.up * 0.375f + weaponAttachObject.transform.forward * 0.21f , weaponAttachObject.transform.rotation) as GameObject;
						currentWeapon.transform.parent = weaponAttachObject.transform;
						Activate.setActiveCustom(true);
					}
					switcher = true;
					currentWeapon.GetComponent<MachineGun>().equip(true);;
				}
				else {
					switcher = false;
					currentWeapon.GetComponent<MachineGun>().equip(false);
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

				// spawn pogostick
				if (useWeaponID == 1){
					currentWeapon = Instantiate(pogoStick, this.transform.position, this.transform.rotation) as GameObject;
					float offsetY = 0.0f;
					float offsetZ = 0.45f;
					Vector3 offset = this.transform.position + this.transform.up * offsetY + this.transform.forward * offsetZ;
					currentWeapon.transform.position = offset;

					currentWeapon.transform.parent = this.transform;

				}

				// spawn blackHoleGun
				if (useWeaponID == 2){
					currentWeapon = Instantiate(blackHoleGun, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;
					setCrosshair (true);
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

				/*
				//Delete Handgun
				if (useWeaponID == 3){
					Destroy(currentWeapon.gameObject);
				}

				//Delete SMG
				if (useWeaponID == 4){
					Destroy(currentWeapon.gameObject);
				}
				*/
			}
		}
	}

	private void setCrosshair(bool switcher){
		Crosshair.SetActive (switcher);
	}
}
