using UnityEngine;
using System.Collections;

public class newRopetest : MonoBehaviour {

	public Vector3 p = new Vector3(40f, 16f, 20f);

	// Use this for initialization
	void Start () {
		DynamicRope DR = this.gameObject.AddComponent<DynamicRope>() as DynamicRope;
		DR.HitPoint = p;
		DR.noHit = false;
		//DR.L_desired = (transform.position - p).magnitude;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (KeyManager.leftShift == 2){
			//rigidbody.AddForce(new Vector3(0, 00f, 20000f)*Time.deltaTime, ForceMode.Impulse);
			DynamicRope DR = this.gameObject.GetComponent<DynamicRope>() as DynamicRope;
			DR.L_desired = Mathf.Max(DR.L_desired-0.1f, 1.1f);
		}

		if (KeyManager.key1 == 2){
			//rigidbody.AddForce(new Vector3(0, 00f, 20000f)*Time.deltaTime, ForceMode.Impulse);
			DynamicRope DR = this.gameObject.GetComponent<DynamicRope>() as DynamicRope;
			DR.L_desired += 0.1f;

		}
	
	}
}
