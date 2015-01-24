using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {
	public float timeleft;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, timeleft);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
