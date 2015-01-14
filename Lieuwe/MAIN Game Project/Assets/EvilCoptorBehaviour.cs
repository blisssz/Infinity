using UnityEngine;
using System.Collections;

public class EvilCoptorBehaviour : MonoBehaviour {

	public float hp = 10f;

	public HPmanager hpManager {get; set;}
	public GameObject ExplosionPrefab;
	public GameObject EnemyBullet;

	private float lastShot;
	public float reloadingTime;
	public float shootingRadius;

	void Start () {
		hpManager = this.gameObject.AddComponent<HPmanager>();	
		hpManager.hp = hp;

		lastShot = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		bool playerAttention = this.gameObject.GetComponent<AgentS>().targetsMainTarget;
		if (lastShot <= (Time.time - reloadingTime) && playerAttention == true) {
			attackPlayer();
		}
	}

	void FixedUpdate() {
		if (hpManager.hp <= 0) {
			//zak door de grond
			rigidbody.isKinematic = true;
			rigidbody.MovePosition (rigidbody.position + new Vector3 (0, -8, 0) * Time.deltaTime);
			transform.RotateAround(rigidbody.position, new Vector3 (0,-8,0),5);
			Invoke ("Death",0.5f);
		}
	}
	
	void Death() {
		Instantiate(ExplosionPrefab, transform.position, transform.rotation);
		Destroy (gameObject);
		highScore.enemyKill ();
		score.gameScore += 5;
	}
	


	void attackPlayer (){
		GameObject b = Instantiate(EnemyBullet, (transform.position - new Vector3(0,1.2f,0)), transform.rotation) as GameObject; 
		Destroy(b, 3f);
		lastShot = Time.time;
	}



}
