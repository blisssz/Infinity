using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

	public LayerMask playerLayerMask;
	public GameObject gunShotParticle;
	public GameObject muzzleFlash;
	public GameObject muzzleFlashLocation;
	public GameObject redDot;

	protected int bulletsLeftInMagazine;
	protected int magazinesLeft;
	protected float bulletRange;
	protected float bulletOffset;
	protected float bulletDamage;
	protected float coolDownTime;
	protected float reloadTime;
	protected float timeStamp;
	protected GameObject playerHead;

	private List<GameObject> muzzleFlashInstances = new List<GameObject>();

	private GameObject muzzleFlashInstance; 
	private int knockback = 50;
	


	void Update(){
		if(muzzleFlashInstances != null){
			foreach(GameObject muzzleflash in muzzleFlashInstances){
				muzzleflash.transform.position = muzzleFlashLocation.transform.position;
				muzzleflash.transform.rotation = muzzleFlashLocation.transform.rotation;
			}
		}
	}

	public virtual void Shoot(float bulletRange, float bulletOffset, float bulletDamage){
		Shoot (bulletRange, bulletOffset, bulletDamage, false);
	}

	public virtual void Shoot (float bulletRange, float bulletOffset, float bulletDamage, bool zoomed) {
		//Initialize a ray
		Ray	ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		if(zoomed == true && redDot != null){
			Debug.Log("reddot hit");
			ray = new Ray(redDot.transform.position, (redDot.transform.position - Camera.main.transform.position).normalized);
		}

		RaycastHit hit;
		//Add an (optional) offset/spread to the ray to make the bullet shooting more realistic
		ray.direction += new Vector3(Random.Range(-bulletOffset, bulletOffset),Random.Range(-bulletOffset, bulletOffset),Random.Range(-bulletOffset, bulletOffset));

		//Set a layermask to the player layer to make sure the player can't be hit by the raycast
		int layerMask = playerLayerMask;
		layerMask = ~layerMask;

		if (Physics.Raycast(ray, out hit, bulletRange, layerMask)){ 
			Debug.Log("raycast hit: " + hit.collider.name);
			//Play particle emitter at hit location
			Instantiate(gunShotParticle, hit.point, Quaternion.LookRotation(hit.normal));
			if(hit.rigidbody != null){
				hit.rigidbody.AddForce(ray.direction * knockback);
			}
//			muzzleFlashInstance = Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, muzzleFlashLocation.transform.rotation) as GameObject;
			//muzzleFlashInstances.Add(Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, muzzleFlashLocation.transform.rotation) as GameObject);
			SpawnMuzzleFlash();

			//If it hit an enemy, do damage
			if (hit.collider.tag.Equals("Enemy")){
				hit.transform.SendMessageUpwards("applyDamage", bulletDamage); //Tell the hit gameObject that it should execute a function called ApplyDamage with the parameter bulletDamage (i.e. ApplyDamage(bulletDamage);)
			}

		}
		else{ 
			//Raycast didn't hit anything
			Debug.Log("didn't hit anything");
//			muzzleFlashInstance = Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, muzzleFlashLocation.transform.rotation) as GameObject;
			//muzzleFlashInstances.Add(Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, muzzleFlashLocation.transform.rotation) as GameObject);
			SpawnMuzzleFlash();
		}

		timeStamp = Time.time + coolDownTime;
		bulletsLeftInMagazine--;
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();

	}

	public virtual void Reload(int bulletsInMagazine){
		bulletsLeftInMagazine = bulletsInMagazine;
		magazinesLeft--;
		MagazinesLeft.textValue.text = magazinesLeft.ToString();
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		BulletInfo.instance.StopAllCoroutines();
		BulletInfo.textValue.CrossFadeAlpha (0f, 0f, false);
		timeStamp = Time.time + reloadTime;
	}

	public void DeleteGameObject(GameObject muzzleflash){
		muzzleFlashInstances.Remove(muzzleflash);
	}

	private void SpawnMuzzleFlash(){
		muzzleFlashInstance = Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, muzzleFlashLocation.transform.rotation) as GameObject;
		muzzleFlashInstance.GetComponent<DestroyMuzzleFlashParticles>().setGun (this);
		muzzleFlashInstances.Add(muzzleFlashInstance);
	}
}

/*
- fire rate (how fast do the bullets come out)
- automatic (is it an automatic or not?)
- bullet range
- weapon damage (you can also make something like wpnDmgMin and wpnDmgMax, and every hit get a random between)
- bullet offset (how unprecise is the weapon)
- magazine capacity (how much can fit in curent mag)
- bullets in magazine (how much are currently in mag)
- spare magazine (how much spare ammo you have)
*/