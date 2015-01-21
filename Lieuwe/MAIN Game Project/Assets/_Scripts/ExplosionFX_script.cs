using UnityEngine;
using System.Collections;

public class ExplosionFX_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("Death",2.0f);
	}

	void Death() {
		Destroy (gameObject);
	}
}
