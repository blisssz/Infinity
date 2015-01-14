using UnityEngine;
using System.Collections;

public class BlackHoleGun : MonoBehaviour {

	public LayerMask playerLayerMask;

	public GameObject blackHole;
	public GameObject gunPoint;
	public GameObject Gun;


	private GameObject player;
	private GameObject projectile;
	
	private ParticleSystem glow;

	private static bool firstShotFired;

	private static float lerpTime = 1f;
	private static float currentLerpTime;
	private const float scaleRate = 0.5f;
	private const float maxSize = 1f;



	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		BlackHole.DeleteRigidbody (player.GetComponent<GravityObject>()); //Remove the player from the list of objects affected by the orb
	}

	void LateUpdate () {	
		bool fire = KeyManager.leftMouse == 1;
		bool firehold = KeyManager.leftMouse == 2;
		bool firereleased = KeyManager.leftMouse == 3;
		bool retract = KeyManager.rightMouse == 1;

		//Spawn black hole on begin of left mouseclick
		if (fire == true){
			//If the gun has been used previously there might be a blackhole left in the game. If so, destroy it. 
			if(firstShotFired == true && projectile != null){
				projectile.GetComponent<BlackHole>().setFadeOut();
			}
			projectile = Instantiate(blackHole, gunPoint.transform.position, Gun.transform.rotation) as GameObject; //Instantiate the black hole orb
			projectile.GetComponent<BlackHole>().enabled = false; //Turn the gravitational effects of the blackhole off
			firstShotFired = true;
			currentLerpTime = 0f; //Reset the lerptimer if a new black hole orb has been fired
			glow = projectile.GetComponent<BlackHole>().ParticleSystemGlow.GetComponent<ParticleSystem>(); //Get the particle system of the child of the black hole 
		}

		//Charge black hole while player is holding the left mousebutton
		if (firehold == true){
			
			//increment the lerp timer once per frame
			currentLerpTime += (Time.deltaTime * scaleRate)*1.33f;
			if (currentLerpTime > lerpTime) {
				currentLerpTime = lerpTime;
			}
			
			//lerp the projectile from the instantiation point to it's endpoint using a coserp function
			float perc = currentLerpTime / lerpTime;
			perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f); //coserp function
			projectile.transform.position = Vector3.Lerp(gunPoint.transform.position, gunPoint.transform.position + (-gunPoint.transform.up * 0.5f), perc); //lerp the black hole orb

			//Increase size of the black hole and the particles around it
			float scale = scaleRate * Time.deltaTime;
			if (projectile.transform.localScale.x <= maxSize){
				projectile.transform.localScale += new Vector3(scale,scale,scale);
				glow.startSize = projectile.transform.localScale.x * 0.25f; //Change the size of the particles of the glow particle system
			}
		}

		//Shoot it when the player releases the left mouse button.
		if(firereleased == true){

			//Set the strength of the Black hole according to the current size
			projectile.GetComponent<BlackHole>().gravity *= projectile.transform.localScale.x;
			projectile.GetComponent<BlackHole>().radius *= projectile.transform.localScale.x;

			//Shoot the projectile
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			RaycastHit hit;

			//Set a layermask to the player layer to make sure the player can't be hit by the raycast
			int layerMask = playerLayerMask;
			layerMask = ~layerMask;

			if (Physics.Raycast(ray, out hit,10000f, layerMask)){ 
				//Raycast hit something
				projectile.transform.LookAt (hit.point);
				projectile.rigidbody.AddForce(projectile.transform.forward * 5000);
			}
			else{ 
				//Raycast didn't hit anything
				projectile.transform.LookAt (hit.point);
				projectile.transform.LookAt(ray.origin + (ray.direction * 10000f)); //Face the projectile towards the point at the end of the raycast
				projectile.rigidbody.AddForce(projectile.transform.forward *5000);
			}

			Physics.IgnoreCollision(projectile.collider, player.collider); //Make sure the black hole orb and the player don't collide
			projectile.GetComponent<BlackHole>().enabled = true; //only let the blackhole check for a collision once it has been shot
			projectile.GetComponent<BlackHole>().shot = true; //activate the gravitational effect of the blackhole
		}

		//Remove the black hole orb when the player clicks the right mouse button
		if (retract == true){
			destroyBlackHole();
		}

	}

	public void destroyBlackHole(){
		if (projectile != null){
			//Only try to remove a black hole orb if it actually exists
			projectile.GetComponent<BlackHole>().enabled = true;
			projectile.GetComponent<BlackHole>().setFadeOut();
			firstShotFired = false;
		}
	}

	void OnDestroy(){if(GameController.dead){Destroy(projectile); destroyBlackHole ();}}

}