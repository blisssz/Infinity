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

	/// <summary>get world Velocity at center of object</summary>
	public Vector3 getWorldVelocity(){
		return this.worldVelocity;
	}

	public Vector3 getAngularVelocity(){
		return this.angularVelocity;
	}


	/// <summary>Get world velocity at a Point, in world coordinates.
	/// Right now only rotations over world Y are supported</summary>
	///
	public Vector3 getVelocityAtPoint(Vector3 hitPoint){
		Vector3 V_byAngV = Vector3.Cross (this.angularVelocity,(hitPoint - this.transform.position) );
		return V_byAngV + this.worldVelocity;
	}

}
