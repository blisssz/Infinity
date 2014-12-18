using UnityEngine;
using System.Collections;

public class BOSS1script : MonoBehaviour {
	
	public float speed;
	public GameObject Player;
	public GameObject Boss1DeathSound;
	private Vector3 playerPosition;
	private Vector3 toPlayer;
	private bool deathAnimation = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//get player location
		if (Player == null) {
			Player = GameObject.FindWithTag ("Player");
		}
		playerPosition = Player.transform.position;

		toPlayer = new Vector3(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y +18, playerPosition.z - transform.position.z);

		//move to player location
		float step = (DistanceToObject (Player)-14) * Time.deltaTime;
		if (DistanceToObject (Player) > 14 && deathAnimation == false) {
						transform.Translate (toPlayer * step * Time.deltaTime, Space.World);
				}
		if(DistanceToObject (Player) < 23)
				{
						transform.Rotate (Vector3.up, Time.deltaTime * 50f);
				}
		//check if dead
		if (returnNearestObjectDistance ("Boss1Balls") > 100) {
			Destroy (this.gameObject,10);
			if (deathAnimation == false) {
				GameObject soundbody;
				soundbody = Instantiate(Boss1DeathSound, (transform.position - new Vector3(0,1.2f,0)), transform.rotation) as GameObject;
			}
			deathAnimation = true;

			GameObject[] names = GameObject.FindGameObjectsWithTag("Boss1Balls");
			
			foreach(GameObject item in names)
			{
				Destroy(item,10);
			}
		}
		if (deathAnimation == true) {
			Vector3 d = new Vector3(0.0f,25,0.0f);
			transform.Translate (d * Time.deltaTime,Space.World);
			transform.Rotate(Vector3.up, Time.deltaTime*80f);
		}
	}

	float DistanceToObject(GameObject other) {
		return Vector3.Distance (this.transform.position, other.transform.position);
	}

	float returnNearestObjectDistance(string gtag) {
		float nearestDistanceSqr= Mathf.Infinity;
		GameObject[] taggedGameObjects= GameObject.FindGameObjectsWithTag(gtag); 
	GameObject:Transform nearestObj = null;
		
		// loop through each tagged object, remembering nearest one found
		foreach(GameObject obj in taggedGameObjects) {
			
			Vector3 objectPos= obj.transform.position;
			float distanceSqr= (objectPos - transform.position).sqrMagnitude;
			
			if (distanceSqr < nearestDistanceSqr) {
				nearestObj = obj.transform;
				nearestDistanceSqr = distanceSqr;
			}
		}
		return nearestDistanceSqr;
	}	

}
