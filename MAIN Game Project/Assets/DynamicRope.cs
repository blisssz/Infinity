using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Multi rigidbody Rope
 * todo: centripetal forces between bodies;
 * Original Author: Erik Veldhuis
 */
public class DynamicRope : MonoBehaviour {

	//public Mesh aMesh;
	public float mainBodyOffset = 2.0f;		// offset from center in up direction

	// default rope parameters
	public float ropeSegmentLength = 2.0f;
	public float ropeSpringLength = 1.0f;
	public float ropeSegmentMass = 6f;
	public float ropeSphereRadius = 2.0f;	// must be at least 2x lower then the segment length

	public bool useCriticalDamping = true;
	public bool useRigidRope = true;

	public bool useBoxCollider = true;
	public float boxStretchFac = 2.4f;
	
	public float k_spring = 30000.0f;
	public float c_damp = 1000.0f;

	
	// Data for swinging
	public bool noHit {get; set;}
	public Vector3 HitPoint {get; set;}			// always the point where the rope is attached.
	public Vector3 forceInRope {get; set;}
	
	public float L_desired {get; set;}			// the total length of the rope;
	private int frame = 0;

	// tracking Lists
	private List<GameObject> rbList;
	private GameObject[] obArray;

	private List<Vector3[]> stateList;
	private Vector3[][] stateArray;

	private Vector3[] thisState;

	public float iterations = 50.0f; // homwany iterations for physics solver

	private int bodies = 0;

	// Use this for initialization
	void Start () {
		rbList = new List<GameObject>();
		stateList = new List<Vector3[]>();

		thisState = new Vector3[2];

		if (ropeSphereRadius <= 0){
			ropeSphereRadius = ropeSegmentLength/2f;
		}
		//L_desired = (transform.position + transform.up * mainBodyOffset - HitPoint).magnitude;
		L_desired = (transform.position - HitPoint).magnitude;

		iterations = (iterations < 1.0f) ? 1f : Mathf.Floor(iterations);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		frame += 1;
		if (noHit == true){
			// reset list
			rbList = new List<GameObject>();

		}
		else{ // we have a hit
			int nBodies = (int) Mathf.Floor(L_desired/ropeSegmentLength);

			Vector3 ropeDirection; 
			Vector3 offsetPosition = transform.position + transform.up * mainBodyOffset;

			Vector3 centripetalF;
			Vector3 velTang;

			while (rbList.Count < nBodies){

				bodies += 1;

				// create missing objects
				GameObject newObj = new GameObject();
				newObj.name = "RopeColliderSegment";
				newObj.AddComponent<Rigidbody>();
				newObj.rigidbody.mass = ropeSegmentMass;
				newObj.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				newObj.rigidbody.drag = 0.2f;
				newObj.rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
				newObj.rigidbody.useGravity = false;

				if (useBoxCollider == true){
					BoxCollider bCol = newObj.AddComponent<BoxCollider>() as BoxCollider;
					bCol.center = new Vector3(0, 0, ropeSegmentLength/2f);
					bCol.size = new Vector3(ropeSphereRadius, ropeSphereRadius, ropeSegmentLength*boxStretchFac);
				}
				else{
					SphereCollider sCol = newObj.AddComponent<SphereCollider>() as SphereCollider;
					sCol.center = new Vector3(0, 0, 0);
					sCol.radius = ropeSphereRadius;
				}
				// Debug
				MeshFilter mesh = newObj.AddComponent<MeshFilter>() as MeshFilter;
				mesh.sharedMesh = GetComponent<MeshFilter>().sharedMesh;
				newObj.AddComponent<MeshRenderer>();
				newObj.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;



				// set Position in reference to previous obj;
				if (rbList.Count == 0){
					ropeDirection = Vector3.Normalize(transform.position - HitPoint);
					newObj.transform.position = HitPoint + ropeDirection * ropeSegmentLength;

				}
				else{
					// get from last index;
					Vector3 prevBodyPos = obArray[obArray.Length -1].transform.position;
					ropeDirection = Vector3.Normalize(transform.position - prevBodyPos);
					newObj.transform.position = prevBodyPos + ropeDirection * ropeSegmentLength;
				}

				newObj.rigidbody.velocity = this.rigidbody.velocity * (ropeSegmentLength * (float) nBodies)/L_desired; // factored velocity

				// this gives unstable results
				// always ignore this object in collision
				Physics.IgnoreCollision(newObj.transform.collider, this.collider);

				foreach (GameObject gOb in rbList){
					Physics.IgnoreCollision(gOb.transform.collider, newObj.transform.collider);

					if (useBoxCollider == true){
						Physics.IgnoreCollision(gOb.GetComponent<BoxCollider>(), newObj.GetComponent<BoxCollider>());
					}
					else{
						Physics.IgnoreCollision(gOb.GetComponent<SphereCollider>(), newObj.GetComponent<SphereCollider>());
					}
				}
				Vector3[] state = new Vector3[2];
				state[0] = newObj.rigidbody.velocity;
				state[1] = newObj.transform.position;


				// append to list
				rbList.Add(newObj);
				obArray = rbList.ToArray();

				stateList.Add (state);
				stateArray = stateList.ToArray();
			}

			// remove entries if longer
			while (rbList.Count > nBodies){
				Destroy(obArray[bodies-1]);

				rbList.RemoveAt (bodies-1);
				stateList.RemoveAt (bodies-1);
				bodies--;

			}

			obArray = rbList.ToArray();
			stateArray = stateList.ToArray();

			// get new states per update
			for (int i = 0; i < obArray.Length; i++){
				stateArray[i][0] = obArray[i].rigidbody.velocity;
				stateArray[i][1] = obArray[i].transform.position;
			}

			thisState[0] = this.rigidbody.velocity;
			thisState[1] = this.transform.position;


			// physics calculations
			float ds;				// length of segment - ref_length
			float s_segment;		// length of current segment
			Vector3 dir_segment= new Vector3(0,0 ,0);;	// direction of the segment
			float c_damp =0;			// damping constant
			Vector3 ropeForce = new Vector3(0,0 ,0);;		// rope force

			Vector3 v_dir = new Vector3(0,0 ,0);; 			// velocity direction in dir, direction

			Vector3 currentForce = new Vector3(0, 0,0);	

			if (obArray.Length == 0){

			}
			else{

				float dt = Time.deltaTime / iterations;
				Vector3[] forceArray = new Vector3[stateArray.Length];

				for (float t = 0; t < Time.deltaTime; t += dt){
					// first entry
					dir_segment = HitPoint - obArray[0].transform.position;
					s_segment = dir_segment.magnitude;
					dir_segment /= s_segment; // normalized


					v_dir = dir_segment * Vector3.Dot(stateArray[0][0], dir_segment);
					c_damp = 2f * Mathf.Sqrt ((obArray[0].rigidbody.mass*((float)(nBodies-1)) + this.rigidbody.mass) * k_spring);

					ds = s_segment - ropeSegmentLength;

					ropeForce = dir_segment * ds * k_spring - c_damp * v_dir;// + Physics.gravity * ropeSegmentMass;

					currentForce = ropeForce;

					if (useBoxCollider == true){
						obArray[0].transform.rotation = Quaternion.LookRotation(dir_segment);
					}


					////obArray[0].rigidbody.AddForce (ropeForce * Time.deltaTime, ForceMode.Impulse);

	/*
					// for 0 look 1 body farther to get those reaction forces
					dir_segment =  obArray[1].transform.position - obArray[0].transform.position;
					s_segment = dir_segment.magnitude;
					dir_segment /= s_segment; // normalized				
					
					v_dir = dir_segment * Vector3.Dot(stateArray[1][0], dir_segment);
					c_damp = 2f * Mathf.Sqrt (obArray[1].rigidbody.mass * k_spring);
					
					ds = s_segment - ropeSegmentLength;
					
					ropeForce = dir_segment * ds * k_spring - c_damp * v_dir + Physics.gravity * ropeSegmentMass;

					//currentForce += ropeForce;
	*/


					for (int i = 1; i < obArray.Length; i++){
						dir_segment = obArray[i-1].transform.position - obArray[i].transform.position;
						s_segment = dir_segment.magnitude;
						dir_segment /= s_segment; // normalized
						//obArray[i].rigidbody.velocity
						v_dir = dir_segment * Vector3.Dot(stateArray[i][0]-stateArray[i-1][0], dir_segment);

						c_damp =2f * Mathf.Sqrt ((obArray[i].rigidbody.mass*((float)(nBodies-1)) + this.rigidbody.mass) * k_spring);
						
						ds = s_segment - ropeSegmentLength;


						ropeForce = dir_segment * ds * k_spring - c_damp * v_dir;// + Physics.gravity * ropeSegmentMass;

						currentForce += -ropeForce;

						// calc centripetal forces;
						velTang = createVelocityTangentRelative(stateArray[i-1][0], stateArray[i][0], dir_segment);
						centripetalF = ropeSegmentMass * getAccelerationNormal(velTang, dir_segment, s_segment);
						
						// apply this force on the previous body, in the negative direction 
						currentForce += -centripetalF;

						////obArray[i-1].rigidbody.AddForce (-ropeForce * Time.deltaTime, ForceMode.Impulse);
						////obArray[i].rigidbody.AddForce (ropeForce * Time.deltaTime, ForceMode.Impulse);


						// assign force to the force array 
						forceArray[i-1] = currentForce;

						// ...and reassign value
						currentForce = ropeForce;

						if (useBoxCollider == true){
							obArray[i].transform.rotation = Quaternion.LookRotation(dir_segment);
						}

					}


					// add force/loc to final body, itself;
					dir_segment = obArray[bodies-1].transform.position - (transform.position );
					s_segment = dir_segment.magnitude;
					dir_segment /= s_segment; // normalized

					v_dir = dir_segment * Vector3.Dot(thisState[0] - stateArray[bodies-1][0], dir_segment);

					// ref seg is rest value of total length
					ds = s_segment - L_desired % obArray.Length;

					c_damp =2f * Mathf.Sqrt ((this.rigidbody.mass + obArray[bodies-1].rigidbody.mass) * k_spring);
					//Debug.Log (ds);

					ropeForce = dir_segment * ds * k_spring - c_damp * v_dir;// + Physics.gravity * ropeSegmentMass;
					currentForce += -ropeForce;

					// calc centripetal forces;
					velTang = createVelocityTangentRelative(stateArray[bodies-1][0], thisState[0], dir_segment);
					centripetalF = this.rigidbody.mass * getAccelerationNormal(velTang, dir_segment, s_segment);

					// apply this force on the previous body, in the negative direction 
					currentForce += -centripetalF;


					forceArray[bodies-1] = currentForce;

					// ropeForce is the Force acting on the main body

					//obArray[obArray.Length-1].rigidbody.AddForce (currentForce * Time.deltaTime, ForceMode.Impulse);

					////obArray[obArray.Length-1].rigidbody.AddForce (-ropeForce * Time.deltaTime, ForceMode.Impulse);
					////rigidbody.AddForce (ropeForce * Time.deltaTime, ForceMode.Impulse);

					// All forces per body calculated, now calculate the next velocity and position by integration
					Vector3[] dudt1 = new Vector3[2];
					Vector3[] dudt2 = new Vector3[2];
					Vector3[] p = new Vector3[2];		// sub State

					for (int i = 0; i < stateArray.Length; i ++){
						dudt1 = odeSolveMotionFromForce(forceArray[i], stateArray[i], ropeSegmentMass);
						p[0] = stateArray[i][0] + dt * dudt1[0];
						p[1] = stateArray[i][1] + dt * dudt1[1];

						dudt2 = odeSolveMotionFromForce(forceArray[i], p, ropeSegmentMass);
						stateArray[i][0] = stateArray[i][0] + dt/2f * (dudt1[0] + dudt2[0]);
						stateArray[i][1] = stateArray[i][1] + dt/2f * (dudt1[1] + dudt2[1]);

						// assign position and velocity (could be done outside loop if using states only)
						obArray[i].rigidbody.velocity = stateArray[i][0];
						obArray[i].transform.position = stateArray[i][1];

					}

					// force input test
					if (KeyManager.middleMouse == 2){
						ropeForce = ropeForce + new Vector3(1000f, 0f, 0f);
					}
					// solve last body
					dudt1 = odeSolveMotionFromForce(ropeForce, thisState, this.rigidbody.mass);
					p[0] = thisState[0] + dt * dudt1[0];
					p[1] = thisState[1] + dt * dudt1[1];
					
					dudt2 = odeSolveMotionFromForce(ropeForce, p, this.rigidbody.mass);
					thisState[0] = thisState[0] + dt/2f * (dudt1[0] + dudt2[0]);
					thisState[1] = thisState[1] + dt/2f * (dudt1[1] + dudt2[1]);
					
					// assign position and velocity (could be done outside loop if using states only)
					this.rigidbody.velocity = thisState[0];
					this.transform.position = thisState[1];

				} // for loop end

				// apply soft force to all
			//	for (int i = 0; i < obArray.Length; i++){
			//		obArray[i].rigidbody.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime, ForceMode.Impulse);
			//	}

			//	this.rigidbody.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime, ForceMode.Impulse);
				//Debug.Log (forceArray[bodies-1]);
				//Debug.Log (ropeForce);
				//Debug.Log (stateArray.Length);
				//Debug.Log(obArray.Length);
			} // else no empty array end



		}

	
	}

	/*
	 */
	public Vector3[] odeSolveMotionFromForce(Vector3 force, Vector3[] U, float m){
		// dudt state dudt[0] = acceleration, dudt[1] = velocity
		Vector3[] dudt = new Vector3[2];
		// acceleration, state U[0] = vel, U[1] = pos
		dudt[0] = force/m + Physics.gravity;
		dudt[1] = U[0];
		
		return dudt;
	}

	/*
	 */
	public Vector3[] odeMassSpringDamperState(Vector3 force, Vector3[] U, float m, float c, float k){
		// dudt state dudt[0] = acceleration, dudt[1] = velocity
		Vector3[] dudt = new Vector3[2];
		// acceleration, state U[0] = vel, U[1] = pos
		dudt[0] = (force - c*U[0] + k * U[1])/m;
		dudt[1] = U[0];

		return dudt;
	}
	
	/* 
	 * ropeDir must be normalized already
	 * ropeDir points from Body2 to Body1 the pivot
	 * vel1 is the global velocity of Body1
	 * vel2 is the global velocity of Body2
	 */
	public Vector3 createVelocityTangentRelative(Vector3 vel1, Vector3 vel2, Vector3 ropeDir){
		Vector3 dVel = vel2 - vel2;
		return (dVel - ropeDir * Vector3.Dot(ropeDir, dVel));
	}

	/* 
	 * ropeDir must be normalized already
	 * ropeDir points from Body2 to Body1 the pivot
	 */
	public Vector3 createVelocityTangent(Vector3 dVel, Vector3 ropeDir){
		return (dVel - ropeDir * Vector3.Dot(ropeDir, dVel));
	}


	public Vector3 getAccelerationNormal(Vector3 velTangent, Vector3 ropeDir, float segmentLength){
		return ropeDir*(Vector3.Dot(velTangent, velTangent) / segmentLength ) ;
	}



/*	void Update(){
		// make sure every segment has a fixed distance

		float ds;				// length of segment - ref_length
		float s_segment;		// length of current segment
		Vector3 dir_segment;	// direction of the segment

		if (false){
			if (obArray.Length != 0){
				// first entry
				dir_segment = HitPoint - obArray[0].transform.position;
				s_segment = dir_segment.magnitude;
				dir_segment /= s_segment; // normalized
				
				obArray[0].transform.position = HitPoint - dir_segment * ropeSegmentLength;
				
				for (int i = 1; i < obArray.Length; i++){
					dir_segment = obArray[i-1].transform.position - obArray[i].transform.position;
					s_segment = dir_segment.magnitude;
					dir_segment /= s_segment; // normalized
					
					obArray[i].transform.position = obArray[i-1].transform.position - dir_segment * ropeSegmentLength;
				}
				// last entry
				dir_segment = obArray[obArray.Length-1].transform.position - transform.position;
				s_segment = dir_segment.magnitude;
				dir_segment /= s_segment; // normalized
				
				transform.position = obArray[obArray.Length-1].transform.position - dir_segment * ropeSegmentLength;
				
				
			}
		}

	}*/

}
