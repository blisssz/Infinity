using UnityEngine;
using System.Collections;

public class GrapplingRopeSwing : MonoBehaviour {

	// default rope parameters
	public bool useCriticalDamping = true;
	public bool useRigidRope = false;
	
	public float k_spring = 5000.0f;
	public float c_damp = 100.0f;


	// Data for swinging
	public bool noHit {get; set;}
	public Vector3 HitPoint {get; set;}			// always the point where the rope is attached.
	public Vector3 forceInRope {get; set;}

	public float L_desired {get; set;}
	
	// Use this for initialization
	void Start () {

		noHit = true;
		L_desired = 0.0f;
		HitPoint = transform.position;
	}

	void FixedUpdate () {
		
	
		if  (noHit == false){
			
			// vector from player to hit position
			Vector3 dir = HitPoint - transform.position;
			
			Vector3 current_Velocity =  rigidbody.velocity;
			
			Vector3 vel_tan = current_Velocity - dir * (Vector3.Dot(dir, current_Velocity) / dir.sqrMagnitude);
			
			
			Vector3 accel_normal = (dir / dir.magnitude) * (vel_tan.sqrMagnitude / dir.magnitude);
			
			
			//float L_desired = 25.0f;			
			float du = Mathf.Max (dir.magnitude - L_desired, 0.0f);
			
			dir.Normalize();

			float forceFac = 1.0f;
			// Rope check, if velocity in is the direction of hinge of the rope then no force added;
			if (Vector3.Dot (dir, Vector3.Normalize (-accel_normal+Physics.gravity)) <  0.00f){
				forceFac = 1.0f;
			}
			
			
			// use Critical damping
			if (useCriticalDamping == true){
				c_damp = 2f * Mathf.Sqrt (rigidbody.mass * k_spring);
			}
			
			if (useRigidRope == false){
				if (du == 0.0f){
					forceFac = 0.0f;
					
				}
				else{
					// Rope check, if total acceleration is in the direction of hinge of the rope then no damping added if du > 0.0f;
					if (Vector3.Dot (dir, Vector3.Normalize (-accel_normal+Physics.gravity)) >  0.00f){
						forceFac = 1.0f;
						c_damp = 0.0f;
					}
				}
			}
			
			// velocity direction in dir, direction
			Vector3 v_dir = dir * Vector3.Dot(rigidbody.velocity, dir);
			
			Vector3 F = dir * du * k_spring - c_damp * v_dir;
			F *= forceFac;
			
			
			rigidbody.AddForce (F *Time.deltaTime, ForceMode.Impulse);

			forceInRope = F;
		}

		else{
			noHit = true;
		}

		
	}
}
