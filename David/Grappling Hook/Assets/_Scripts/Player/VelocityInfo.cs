using UnityEngine;
using System.Collections;

public class VelocityInfo : MonoBehaviour {


	private Vector3 worldVelocity;		//	[m/s]
	private Vector3 angularVelocity;	// 	[w/s]


	private Vector3 prevPos;
	private Vector3 prevAngPos;

	// Use this for initialization
	void Start () {
		prevPos = transform.position;
		prevAngPos = transform.forward;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		worldVelocity = (transform.position - prevPos)/Time.deltaTime; // in [m/s]
		prevPos = transform.position;

		// only valid for the rotation over Y direction when transform.forward is in the XZ plane
		Vector3 r = (transform.forward + prevAngPos)*0.5f;
		angularVelocity = Vector3.Cross ( r, (transform.forward - prevAngPos)/Time.deltaTime)/ r.sqrMagnitude;

		prevAngPos = transform.forward;


	}

	// get world Velocity at center of object
	public Vector3 getWorldVelocity(){
		return this.worldVelocity;
	}

	public Vector3 getAngularVelocity(){
		return this.angularVelocity;
	}

}
