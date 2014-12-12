using UnityEngine;
using System.Collections;

/// <summary>
/// Player manager. stores interesting player data and important gameObject locations
/// (Attach this script to the main parent of the player)
/// </summary>

public class PlayerManager : MonoBehaviour {

	public GameObject weaponAttachObject {get; set;}
	public GameObject projectileSpawnObject {get; set;}
	public GameObject playerHeadObject {get; set;}

	private GameObject currentWeapon {get; set;}

	// Weapons / tools
	public GameObject grapplingHook;
	public GameObject pogoStick;
	public GameObject blackHoleGun;
	public GameObject grapplingHookV2;
	
	public int useWeaponID = 0;

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
	
	
	}
	
	// Update is called once per frame
	void Update () {

		// spawn a tool/weapon
		if (hasWeapon == false){

			bool key1 = KeyManager.key1 == 1; // tap key once

			// spawn weapon / tool
			if (key1 == true && currentWeapon == null){

				// spawn grapplingHook
				if (useWeaponID == 0){
					currentWeapon = Instantiate(grapplingHook, weaponAttachObject.transform.position, weaponAttachObject.transform.rotation) as GameObject;
					currentWeapon.transform.parent = weaponAttachObject.transform;

					GrapplingHook gh = currentWeapon.GetComponent<GrapplingHook>();

					// projectile spawn location gameobject
					gh.projectileSpawnObject = projectileSpawnObject;

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
					
				}

			}
			// remove weapon when one is already active
			else if (key1 == true && currentWeapon != null){

				// Delete GrapplingHook
				if (useWeaponID == 0){
					GrapplingHook gh = currentWeapon.GetComponent<GrapplingHook>();
					gh.DestroyAll();
				}

				if (useWeaponID == 1){
					// pogostick delete
					Destroy(currentWeapon.gameObject);

				}

				if (useWeaponID == 2){
					// Destroy BlackHoleGun and a black hole if there is one left in the game
					currentWeapon.GetComponent<BlackHoleGun>().destroyBlackHole();
					Destroy(currentWeapon.gameObject);
				}

				// Delete GrapplingHookV2
				if (useWeaponID == 0){
					GrapplingHookV2 gh = currentWeapon.GetComponent<GrapplingHookV2>();
					gh.DestroyAll();
				}

			}
		}
	}
}
