using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

	public LayerMask playerLayerMask;
	public GameObject gunShotParticle;
	public GameObject gunShotHitParticle;
	public GameObject gunShotHeadshotParticle;
	public GameObject muzzleFlash;
	public GameObject muzzleFlashLocation;
	public GameObject redDot;
	public AudioClip audioShoot;

	protected int bulletsLeftInMagazine;
	protected int magazineBulletsLeft;
	protected float bulletRange;
	protected float bulletOffset;
	protected float bulletDamage;
	protected float coolDownTime;
	protected float reloadTime;
	protected float timeStamp;
	protected GameObject playerHead;
	protected bool gunEquipped = false;

	private List<GameObject> muzzleFlashInstances = new List<GameObject>();
	private GameObject muzzleFlashInstance; 
	private int knockback = 10;
	private int playerKnockback = 0; //200 would probably be a good value when used
	private int bulletsInMagazine;

	void Update(){
		//Make sure all muzzleflashes stay at the tip of the gun
		if(muzzleFlashInstances != null){
			foreach(GameObject muzzleflash in muzzleFlashInstances){
				muzzleflash.transform.position = muzzleFlashLocation.transform.position;
				muzzleflash.transform.rotation = muzzleFlashLocation.transform.rotation;
			}
		}
	}

	public virtual void Shoot(float bulletRange, float bulletOffset, float bulletDamage){
		//call other shoot method
		Shoot (bulletRange, bulletOffset, bulletDamage, false);
	}

	public virtual void Shoot (float bulletRange, float bulletOffset, float bulletDamage, bool zoomed) {
		//Play shot sound
		AudioSource.PlayClipAtPoint(audioShoot, transform.position, 1f);

		//Initialize a ray
		Ray	ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		if(zoomed == true && redDot != null){
			//Debug.Log("reddot hit");
			ray = new Ray(redDot.transform.position, (redDot.transform.position - Camera.main.transform.position).normalized);
		}

		RaycastHit hit;
		//Add an (optional) offset/spread to the ray to make the bullet shooting more realistic
		ray.direction += new Vector3(Random.Range(-bulletOffset, bulletOffset),Random.Range(-bulletOffset, bulletOffset),Random.Range(-bulletOffset, bulletOffset));

		//Set a layermask to the player layer to make sure the player can't be hit by the raycast
		int layerMask = playerLayerMask;
		layerMask = ~layerMask;

		if (Physics.Raycast(ray, out hit, bulletRange, layerMask)){ 
			//Raycast hit something
			//Debug.Log("raycast hit: " + hit.collider.name);

			//Add knockback if a rigidbody is hit
			if(hit.rigidbody != null){
				hit.rigidbody.AddForce(ray.direction * knockback * bulletDamage);
			}
			this.transform.root.rigidbody.AddForce (-ray.direction * playerKnockback * bulletDamage);

			//Spawn a muzzleflash at the tip of the gun
			SpawnMuzzleFlash();

			//If it hit an enemy, do damage
			if (hit.collider.tag.Equals ("EnemyHead")){
				//Headshot, damage x2
				GameObject Hitted=hit.transform.root.gameObject;
				Hitted.GetComponent<HPmanager>().doDamage(bulletDamage);
				BulletInfo.instance.StartCoroutine ("Fade");
				Instantiate(gunShotHeadshotParticle, hit.point, Quaternion.LookRotation(hit.normal));
			}
			else if (hit.collider.tag.Equals("Enemy")){
				GameObject Hitted=hit.transform.root.gameObject;
				Hitted.GetComponent<HPmanager>().doDamage(bulletDamage); //Tell the hit gameObject that it should execute a function called ApplyDamage with the parameter bulletDamage (i.e. ApplyDamage(bulletDamage);)
				Instantiate(gunShotHitParticle, hit.point, Quaternion.LookRotation(hit.normal));
			} 
			/*else if (hit.collider.tag.Equals("Agent")){
				GameObject Hitted=hit.transform.root.gameObject;
				Hitted.GetComponent<HPmanager>().doDamage(bulletDamage); //Tell the hit gameObject that it should execute a function called ApplyDamage with the parameter bulletDamage (i.e. ApplyDamage(bulletDamage);)
				Instantiate(gunShotHitParticle, hit.point, Quaternion.LookRotation(hit.normal));
			} */
			else{
				//Play particle emitter at hit location
				Instantiate(gunShotParticle, hit.point, Quaternion.LookRotation(hit.normal));
			}
		}
		else{ 
			//Raycast didn't hit anything
			//Debug.Log("didn't hit anything");
			SpawnMuzzleFlash();
		}

		timeStamp = Time.time + coolDownTime; //Update the timestamp for the cooldown
		bulletsLeftInMagazine--; //Remove one bullet from magazine
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString(); //Update bulletsLeft UI text

	}

	public virtual void Reload(int bulletsInMagazine){
		int bulletsToRefill = (bulletsInMagazine - bulletsLeftInMagazine);
		if(magazineBulletsLeft >= bulletsToRefill){
			bulletsLeftInMagazine = bulletsInMagazine;
			magazineBulletsLeft -= bulletsToRefill;

		} else{
			bulletsLeftInMagazine += magazineBulletsLeft;
			magazineBulletsLeft = 0;
		}

		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
		timeStamp = Time.time + reloadTime;
	}

	private void SpawnMuzzleFlash(){
		muzzleFlashInstance = Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, muzzleFlashLocation.transform.rotation) as GameObject;
		muzzleFlashInstance.GetComponent<DestroyMuzzleFlashParticles>().setGun (this);
		muzzleFlashInstances.Add(muzzleFlashInstance);
	}

	public void DeleteGameObject(GameObject muzzleflash){
		muzzleFlashInstances.Remove(muzzleflash);
	}

	public void addAmmunition(int amount){
		magazineBulletsLeft += amount;
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
	}

	public void addMagazines(int amount){
		magazineBulletsLeft += amount*bulletsInMagazine;
		MagazinesLeft.textValue.text = magazineBulletsLeft.ToString();
		BulletsLeft.textValue.text = bulletsLeftInMagazine.ToString();
	}
	
	public bool getEquipStatus(){
		return gunEquipped;
	}
}