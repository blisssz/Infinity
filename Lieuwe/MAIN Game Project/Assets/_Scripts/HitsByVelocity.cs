using UnityEngine;
using System.Collections;

/// <summary>
/// Compontent that enables a object to hit by velocity based dmg; Where vel == dmg;
/// </summary>

public class HitsByVelocity : MonoBehaviour {


	// Use this for initialization
	void Start () {
		if (!this.gameObject.GetComponent<Rigidbody>()){
			Destroy (this);
		}
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 vel = this.rigidbody.velocity;
		RaycastHit rayHit;
		if (Physics.Raycast(this.transform.position, -this.transform.up, out rayHit, vel.magnitude*Time.deltaTime + 1f)){
			if (rayHit.transform.GetComponent<HPmanager>()){
				rayHit.transform.GetComponent<HPmanager>().doDamage(0f, vel, false);
			}
		}
	
	}
}
