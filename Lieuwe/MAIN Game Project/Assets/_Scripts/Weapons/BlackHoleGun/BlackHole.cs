using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackHole : MonoBehaviour {

	public float gravity;
	public float radius;

	public GameObject ParticleSystemGlow;

	public bool shot;


	private GameObject player;

	private bool fading = false;

	private ParticleSystem glow;

	private static List<GravityObject> rigidbodiesWithGravity = new List<GravityObject>();

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		glow = this.GetComponent<BlackHole>().ParticleSystemGlow.GetComponent<ParticleSystem>();
	}

	void Update () {
		if (fading == false){
			foreach (GravityObject target in rigidbodiesWithGravity)
			{	//attract all rigidbodies that are affected by the gravity of the blackhole towards the black hole (if within range)
				if(target != null){
					// Make it possible to shoot projectiles at boss/enemies 
					if(target.tag.Equals ("projectile")){
						float dist = (transform.position - target.transform.position).magnitude;
							if(dist < radius){
								Vector3 projectileForce = gravity * transform.forward * (radius - dist);
								target.rigidbody.AddForce(projectileForce);
						}
					}
					else{
						target.rigidbody.AddExplosionForce(-gravity, transform.position, radius, 0.0f, ForceMode.Acceleration);
					}
				}
			}
		}
	}

	void LateUpdate () {
		if (fading == true) {
			//Make the orb shrink
			float scale = 1f * Time.deltaTime;
			this.transform.localScale -= new Vector3 (scale, scale, scale);

			/* Doesn't work, no idea why :( Not very important, might try to fix it, might not.
			//Set the current particle effect's particles lifetime to 0.10 (they started at 0.50) to make the particle effect shrinking immediatly in sync with the black hole shrinking
			if (doThisOnce == true){
				ParticleSystem.Particle[] particles = new ParticleSystem.Particle[glow.particleCount];;
				glow.GetParticles(particles);
				for(int i = 0; i < particles.Length; i++){
					if(particles[i].lifetime > 0.10f){
						particles[i].lifetime = 0.10f;
					}
				}
				doThisOnce = false;
			}
			*/

			glow.startLifetime = 0.10f; //Set the lifetime of the particle effect smaller to make it in sync with the very fast shrinking black hole
			glow.startSize = this.transform.localScale.x * 0.25f; //Change the size of the particle to make it match with the current size of the shrinking black hole
			glow.maxParticles -= Mathf.RoundToInt(400f * Time.deltaTime); //Decrease the number of particles over time as the black hole is shrinking

			if(this.transform.localScale.x <= 0){
				//The orb has imploded completely

				//Play explosion animation NOT IMPLEMENTED

				//Make sure the player is definitely affected by the explosion but it's only once in the list
				if(player != null){
					BlackHole.DeleteRigidbody(player.GetComponent<GravityObject>()); //Remove the player from the list of objects affected by the orb
					BlackHole.AddRigidbody(player.GetComponent<GravityObject>()); //Make the player affected by the gravitational effect of the blackhole
					foreach (GravityObject target in rigidbodiesWithGravity)
					{ //Add explosion force
						if(target != null){
							//if(target.tag.Equals("projectile")){
							//	
							//	}
							//}
							//else{
							target.rigidbody.AddExplosionForce(this.gravity * 500, transform.position, this.radius);

						}	
					}
				}
				//Destroy this object;
				BlackHole.DeleteRigidbody (player.GetComponent<GravityObject>()); //Remove the player from the list of objects affected by the orb after explosion
				fading = false;
				Destroy (this.gameObject);
			}
		}
	}

	//This function is called once the blackhole projectile hits a collider
	void OnCollisionEnter(Collision collision){
		if (shot == true){
			if(collision.gameObject.tag.Equals("Player") == false){
				BlackHole.AddRigidbody (player.GetComponent<GravityObject>()); //Make the player affected by the gravitational effect of the blackhole
				transform.parent = collision.collider.transform;
				Destroy (this.rigidbody);
				shot = false;
			}
		}
	}
	
	void OnApplicationQuit (){
		rigidbodiesWithGravity = new List<GravityObject>();
	}

	public static void AddRigidbody(GravityObject rigidbody){
		rigidbodiesWithGravity.Add(rigidbody);
	}

	public static void DeleteRigidbody(GravityObject rigidbody){
		rigidbodiesWithGravity.Remove(rigidbody);
	}

	public void setFadeOut(){
		fading = true;
	}
}