using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	public float rotationSpeed;
	public Vector3 pos;
	public ParticleSystem coinPickup;

	// Use this for initialization
	void Start () {
		transform.Rotate (90, 0, 0);
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Player") || col.tag.Equals ("pogoStick")) {
			Destroy (gameObject);
			Instantiate (coinPickup, pos, Quaternion.identity);
			score.scoreUp (10);
			highScore.pickUpCoin ();
		}
	}
}
