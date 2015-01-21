using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {
	public float bulletSpeed;
	public float dmg = 10f;
	public GameObject Player;
	private Vector3 playerPosition;
	private Vector3 toPlayer;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag ("Player");
		playerPosition = Player.transform.position;
		toPlayer = playerPosition - transform.position;
		rigidbody.velocity = toPlayer.normalized * bulletSpeed;

		audio.PlayOneShot(audio.clip);

		Destroy (this.gameObject, 3f);
	}

	void OnTriggerEnter(Collider col){
		if(col.tag.Equals("Enemy")){}
		else if (col.tag.Equals("Agent")){}
		else if (col.tag.Equals("HitBox")){}
		else{
			if(col.transform.tag.Equals("Player")){
				Player.GetComponent<HPmanager>().doDamage (dmg);
				CameraShake.shakeMainCamera(0.5f);
			}
			Destroy(this.gameObject);
		}
	}
}
