using UnityEngine;
using System.Collections;

public class SoundWindScript : MonoBehaviour {

	public GameObject Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Player == null) {
			Player = GameObject.FindWithTag ("Player");
		}
		gameObject.audio.volume = -0.1f+0.01f*Player.rigidbody.velocity.magnitude;
		gameObject.audio.pitch = 0.4f+0.02f*Player.rigidbody.velocity.magnitude;
	}

}
