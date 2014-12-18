using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {
	public float bulletSpeed;
	public GameObject Player;
	private Vector3 playerPosition;
	private Vector3 toPlayer;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag ("Player");
		playerPosition = Player.transform.position;
		toPlayer = playerPosition - transform.position;
		rigidbody.velocity = toPlayer.normalized * bulletSpeed;
		Destroy (this, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if(col.tag.Equals("Enemy")){}
		else{
			if(col.tag.Equals("Player")){
				Player.GetComponent<HPmanager>().doDamage (10);
				print (healthBar.playerHealth);
			}
			Destroy(this);
		}
	}
}
