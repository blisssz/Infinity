using UnityEngine;
using System.Collections;

public class PogoBossLaser : MonoBehaviour {


	public float DMG = 0f;
	private bool hasHit = false;
	public float lifetime = 1f;


	public float xyScale = 1f;
	private float t = 0f;

	void Update () {

		RaycastHit hit;

		// simple quadratic function for scale from 0 to 1 to 0;
		float xyS = xyScale * (-1f/(0.25f*lifetime*lifetime) *Mathf.Pow(t - 0.5f*lifetime, 2f) + 1f);
		t += Time.deltaTime;

		if (Physics.Raycast(transform.position, transform.forward, out hit, 10000f) ){
			transform.localScale = new Vector3(xyS, xyS, hit.distance);
			if (!hasHit && hit.transform.tag == "Player"){
				hasHit = true;
				Debug.Log ("Player got Hit");
			}
		}
		else{
			transform.localScale = new Vector3(xyS, xyS, 10000f);
		}

	
	}
}
