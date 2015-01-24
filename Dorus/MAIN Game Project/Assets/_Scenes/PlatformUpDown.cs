using UnityEngine;
using System.Collections;

public class PlatformUpDown : MonoBehaviour {
	private float startPos = Random.value * Mathf.PI * 2;
	private float x = 2;

	void Update() {
		transform.Translate (Vector3.up * Mathf.Sin ((Time.time / x) + startPos)/(5*x));
	}

}