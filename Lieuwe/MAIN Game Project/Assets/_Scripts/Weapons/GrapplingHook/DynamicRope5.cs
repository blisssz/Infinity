using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicRope5 : MonoBehaviour {

	public Vector3 mainBodyOffset = new Vector3(0, 6f, 9f);		// offset from center in up direction

	public RopeParams ropeParams {get; set;}

	// default rope parameters
	public int iterations = 60; // homwany iterations for physics solver	

	public float ropeSegmentLength = 2.0f;
	public float ropeSegmentMass = 6f;
	public float ropeSphereRadius = 1.0f;
	
	public bool useCriticalDamping = true;
	public bool noSpringyRopes = true;
	public float clampTreshhold = 0.10f;
	
	public bool useBoxCollider = true;
	public float boxStretchFac = 1.5f;
	
	public float k_spring = 20000.0f;
	public float c_damp = 1000.0f;

	private int ropeVertices = 6;
	private float ropeRadius = 0.1f;
	

	// Data for swinging
	public bool noHit {get; set;}
	public GameObject HitPointObject {get; set;}
	public Vector3 HitPoint {get; set;}			// always the point where the rope is attached.
	public Vector3 forceInRope {get; set;}
	
	public float L_desired {get; set;}
	
	// tracking Lists
	private List<GameObject> ropeList;
	private GameObject[] ropeArray;


	private GameObject[] obArray;

	private Vector3[] forceArray;
	
	private List<Vector3[]> stateList;
	private Vector3[][] stateArray;

	private float[] massArray;
	private float[] c_dampArray;

	private int ropeObjectCount = 0;
	private int allRopeObjectsCount = 0;

	private Vector3 mainBodyInputForce;

	RopeMesh ropeMesh;

	// Use this for initialization
	void Start() {
		ropeMesh = new RopeMesh();
		ropeList = new List<GameObject>();

		stateList = new List<Vector3[]>();

		if (ropeSphereRadius <= 0){
			ropeSphereRadius = ropeSegmentLength/2f;
		}
		//L_desired = (transform.position + transform.up * mainBodyOffset - HitPoint).magnitude;
		L_desired = (transform.position - HitPointObject.transform.position).magnitude;
		
		iterations = (iterations < 1) ? 1 : iterations;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (noHit == true){
			// reset list\
			
		}
		else{ // we have a hit
			//this.transform.root.rigidbody.AddForce(new Vector3(2000, 0, 0));

			int nBodies = (int) Mathf.Floor(L_desired/ropeSegmentLength);
			int nSegments = (int) Mathf.Floor(L_desired/ropeSegmentLength) + 1;
			int nRopeObjects = nSegments - 1;
			int allRopeBodies = nRopeObjects + 2;

			mainBodyOffset = this.transform.position - this.transform.root.transform.position;

			//Vector3 ropeDirection; 
			//Vector3 offsetPosition = transform.position + mainBodyOffset;//transform.up * mainBodyOffset;
			
			Vector3 centripetalF;
			Vector3 velTang;

			bool ropeChanged = false;
			
			while (ropeList.Count < allRopeBodies){
				ropeChanged = true;
				Vector3[] state = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 0, 0)};

				if (allRopeObjectsCount == 0){
					ropeList.Add (HitPointObject);
					ropeList.Add(this.transform.root.gameObject);
					stateList.Add (state);
					stateList.Add (state);
				}
				else{
					GameObject newObj = createRopeRigidBodySegement();
					ropeList.Insert(allRopeObjectsCount, newObj);
					stateList.Add (state);
				}

				ropeArray = ropeList.ToArray(); // keep ropeArray up to date

				allRopeObjectsCount++;
			}


			int mainArrayLength = ropeList.Count;

			while (mainArrayLength > allRopeBodies && mainArrayLength > 2){
				ropeChanged = true;
				// always remove the second last index, and shifts the missing index
				Destroy (ropeArray[mainArrayLength-2]);
				ropeList.RemoveAt(mainArrayLength-2);
				stateList.RemoveAt(mainArrayLength-2);
				mainArrayLength--;
				allRopeObjectsCount--;
			}

			// update arrays to match new length
			if (ropeChanged == true){
				ropeArray = ropeList.ToArray();
				stateArray = stateList.ToArray();
				for (int i = 0; i < mainArrayLength; i++){
					stateArray[i] = new Vector3[2];
				}

				massArray = new float[mainArrayLength];
				c_dampArray = new float[mainArrayLength];
			}

			// fill arrays
			stateArray[0][0] = new Vector3(0, 0, 0);
			stateArray[0][1] = ropeArray[0].transform.position;

			massArray[0] = 1;
			c_dampArray[0] = 1;

			for (int i = 1; i < mainArrayLength; i++){
				stateArray[i][0] = ropeArray[i].rigidbody.velocity;

				if (i == mainArrayLength -1){
					stateArray[i][1] = ropeArray[i].transform.position + mainBodyOffset;
				}
				else{
					stateArray[i][1] = ropeArray[i].transform.position;
				}

				massArray[i] = ropeArray[i].rigidbody.mass;
				c_dampArray[i] = (useCriticalDamping) ? 2f*Mathf.Sqrt ((ropeSegmentMass*((float)(nBodies-i))+ this.transform.root.rigidbody.mass) * k_spring) : c_damp;
			}

			// physics calculations
			float ds;											// length of segment - ref_length
			float s_segment;									// length of current segment
			Vector3 dir_segment= new Vector3(0,0 ,0);			// direction of the segment
			Vector3 ropeForce = new Vector3(0,0 ,0);			// rope force

			Vector3 v_dir = new Vector3(0,0 ,0);;				// velocity direction in dir, direction

			Vector3 currentForce = new Vector3(0, 0,0);
			Vector3 inputForce = new Vector3(0,0,0);
			forceArray = new Vector3[mainArrayLength];

			// time step
			float dt = Time.deltaTime / (float)iterations;

/*			// debug input force
			// force input test
			if (KeyManager.middleMouse == 2){
				mainBodyInputForce = new Vector3(000f, 0f, 0f);
				this.transform.root.rigidbody.AddForce(new Vector3(2000f, 0f, 0f));
			}
			else if(KeyManager.rightMouse == 2){
				mainBodyInputForce = new Vector3(-000f, 0f, 0f);
				this.transform.root.rigidbody.AddForce(new Vector3(-2000f, 0f, 0f));
			}
*/
			mainBodyInputForce = new Vector3(0f, 0f, 0f);//this.transform.root.GetComponent<Movement1>().getInputForce();//new Vector3(0f, 0f, 0f);


			for (int iter = 1; iter <= iterations; iter++){


				//first rope entry
			/*	dir_segment = stateArray[0][1] - stateArray[1][1];
				s_segment = dir_segment.magnitude;
				dir_segment /= s_segment; // normalized
				v_dir = dir_segment * Vector3.Dot(stateArray[1][0] - stateArray[0][0], dir_segment);
				ds = s_segment - ropeSegmentLength;
				ropeForce = dir_segment * ds * k_spring - c_dampArray[1] * v_dir;	// linear damping and linear spring

				currentForce = ropeForce;*/

				for(int i = 1; i < mainArrayLength; i++){
					dir_segment = stateArray[i-1][1] - stateArray[i][1];
					s_segment = dir_segment.magnitude;
					dir_segment /= s_segment; // normalized
					v_dir = dir_segment * Vector3.Dot(stateArray[i][0]-stateArray[i-1][0], dir_segment);

					// final body must take into account the non fixed rope length
					if (i == mainArrayLength - 1){
						ds = s_segment - (ropeSegmentLength *(L_desired % ropeSegmentLength)/ropeSegmentLength);//L_desired % (ropeSegmentLength);
					//	print (s_segment);
					//	print (ds);
						inputForce = mainBodyInputForce;
					}
					else{
						ds = s_segment - ropeSegmentLength;
						inputForce = new Vector3(0,0,0);
					}

					ropeForce = dir_segment * ds * k_spring - c_dampArray[i] * v_dir;	// linear damping and linear spring

					currentForce += -ropeForce;
/* not working properly				
					// calc centripetal forces;
					velTang = createVelocityTangentRelative(stateArray[i-1][0], stateArray[i][0], dir_segment);
					centripetalF = massArray[i] * getAccelerationNormal(velTang, dir_segment, s_segment);
					
					// apply this force on the previous body, in the negative direction 
					currentForce += -centripetalF;
*/
					// assign force to the force array
					forceArray[i-1] = currentForce;
					// ...and reassign value 
					currentForce = ropeForce; // Only the last iterated currentForce will have a force input of != 0.

					if (iter == iterations  && i != (mainArrayLength-1)){
						// cant set object rotation by a rotation Matrix3x3 so then we use:
						if (i == 1){
							// have it refrence to itself
							ropeArray[i].transform.LookAt(ropeArray[i-1].transform.position, ropeArray[i].transform.up);
						}
						else{
							//ropeArray[i].transform.forward = dir_segment;
							// have it refrenced to previous object up, so we get a nice alligned rope
							ropeArray[i].transform.LookAt(ropeArray[i-1].transform.position, ropeArray[i-1].transform.up);
						}

					}

				}

				forceArray[mainArrayLength-1] = ropeForce + inputForce;

				// all forces calculated
				stateArray = solveMotion(forceArray, stateArray, massArray, dt, 1);
			
			}
			this.transform.root.rigidbody.useGravity = true;
			this.transform.root.rigidbody.AddForce(ropeForce + mainBodyInputForce, ForceMode.Force);

			///#########
			// hack to make the ropes less springy, downside is that the ropes stops swinging faster
			// first entry
			float lfac;
			

			if (noSpringyRopes == true){
				
				for (int j = 1; j < mainArrayLength-1; j++){
					dir_segment = stateArray[j-1][1] - stateArray[j][1];
					s_segment = dir_segment.magnitude;
					
					lfac = s_segment / ropeSegmentLength;
					lfac = Mathf.Clamp(lfac, 1f - clampTreshhold, 1f + clampTreshhold);
					
					dir_segment /= s_segment; // normalized

					stateArray[j][1] = stateArray[j-1][1] - dir_segment * ropeSegmentLength * lfac;					
					
					ropeArray[j].rigidbody.velocity = stateArray[j][0];
					ropeArray[j].transform.position = stateArray[j][1];
				}
				
				dir_segment = stateArray[mainArrayLength-2][1] - stateArray[mainArrayLength-1][1];
				s_segment = dir_segment.magnitude;
				dir_segment /= s_segment; // normalized
				
				stateArray[mainArrayLength-1][1] = stateArray[mainArrayLength-2][1] - dir_segment *(ropeSegmentLength *(L_desired % ropeSegmentLength)/ropeSegmentLength);
				//this.transform.root.rigidbody.velocity = stateArray[mainArrayLength-1][0];
				//this.transform.root.transform.position = stateArray[mainArrayLength-1][1] - mainBodyOffset;
			}
			else{				
				//assign position and velocity
				for (int j = 1; j <  mainArrayLength; j++){
					ropeArray[j].rigidbody.velocity = stateArray[j][0];

					if (j == mainArrayLength - 1){
						//ropeArray[j].transform.position = stateArray[j][1] - mainBodyOffset;
					}
					else{
						ropeArray[j].transform.position = stateArray[j][1];
					}
				}
			}


			//ropeMesh.UpdateRopeMesh(ropeArray, mainBodyOffset);

		}
	} // fixed update end




	void LateUpdate(){

		// for some reason ropeArray is null in lateUpdate() sometimes. Simple fix:
		if (ropeArray != null){
			ropeMesh.UpdateRopeMesh(ropeArray, mainBodyOffset, ropeVertices, ropeRadius);
		}
		
	}





	private GameObject createRopeRigidBodySegement(){
		GameObject newObj = new GameObject();
		newObj.name = "RopeColliderSegment";
		newObj.AddComponent<Rigidbody>();
		newObj.rigidbody.mass = ropeSegmentMass;
		newObj.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		newObj.rigidbody.drag = 0.0f;
		newObj.rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
		newObj.rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		newObj.rigidbody.useGravity = false;	// gravity off, because it is included in the force calculations
		
		if (useBoxCollider == true){
			BoxCollider bCol = newObj.AddComponent<BoxCollider>() as BoxCollider;
			bCol.center = new Vector3(0, 0, ropeSegmentLength/2f);
			if (ropeList.Count == 2){
				bCol.center = new Vector3(0, 0, ropeSegmentLength/4f);
				bCol.size = new Vector3(ropeSphereRadius, ropeSphereRadius, ropeSegmentLength*0.5f);
			}
			else{
				bCol.size = new Vector3(ropeSphereRadius, ropeSphereRadius, ropeSegmentLength*boxStretchFac);
			}
		}
		else{
			SphereCollider sCol = newObj.AddComponent<SphereCollider>() as SphereCollider;
			sCol.center = new Vector3(0, 0, 0);
			sCol.radius = ropeSphereRadius;
		}
		// Debug
		/*
		MeshFilter mesh = newObj.AddComponent<MeshFilter>() as MeshFilter;
		mesh.sharedMesh = this.transform.root.GetComponent<MeshFilter>().sharedMesh;
		newObj.AddComponent<MeshRenderer>();
		newObj.GetComponent<MeshRenderer>().material = this.transform.root.GetComponent<MeshRenderer>().material;
*/
		// add positions
		Vector3 prevBodyPos = ropeArray[ropeArray.Length -2].transform.position;
		Vector3 ropeDirection = Vector3.Normalize(this.transform.root.transform.position + mainBodyOffset - prevBodyPos);
		newObj.transform.position = prevBodyPos + ropeDirection * ropeSegmentLength;
		
		newObj.rigidbody.velocity = this.transform.root.rigidbody.velocity * (ropeSegmentLength * (float) ropeObjectCount)/L_desired; // factored velocity

		//Ignore rope self collisions
		Physics.IgnoreCollision(newObj.transform.collider, this.transform.root.collider);
		
		foreach (GameObject gOb in ropeList){

			if (gOb.GetInstanceID() != HitPointObject.GetInstanceID()  && !(gOb.name.Equals(this.transform.root.name)) ){
				Physics.IgnoreCollision(gOb.transform.collider, newObj.transform.collider);

			
				if (useBoxCollider == true){
					Physics.IgnoreCollision(gOb.GetComponent<BoxCollider>(), newObj.GetComponent<BoxCollider>());
				}
				else{
					Physics.IgnoreCollision(gOb.GetComponent<SphereCollider>(), newObj.GetComponent<SphereCollider>());
				}

			}
		}
		//ropeList.Add(newObj);
		return newObj;
	}


	private Vector3[] odeSolveMotionFromForce(Vector3 force, Vector3[] U, float m, bool useGravity){
		// dudt state dudt[0] = acceleration, dudt[1] = velocity
		Vector3[] dudt = new Vector3[2];
		// acceleration, state U[0] = vel, U[1] = pos
		dudt[0] = (useGravity) ? force/m + Physics.gravity : force/m;
		dudt[1] = U[0];
		
		return dudt;
	}


	/// <summary>
	/// Solves the motion. (Arrays)
	/// </summary>
	/// <returns>The motion.</returns>
	/// <param name="forceArray">Force array.</param>
	/// <param name="stateArray">State array.</param>
	/// <param name="mass">Mass.</param>
	/// <param name="dt">Dt.</param>
	/// 
	private Vector3[][] solveMotion(Vector3[] forceArray, Vector3[][] stateArray, float[] mass, float dt, int startIndex){
		// All forces per body calculated, now calculate the next velocity and position by integration
		Vector3[] dudt1 = new Vector3[2];
		Vector3[] dudt2 = new Vector3[2];
		Vector3[] p = new Vector3[2];		// sub State
		
		for (int i = startIndex; i < stateArray.Length; i ++){
			dudt1 = odeSolveMotionFromForce(forceArray[i], stateArray[i], mass[i], i != (stateArray.Length-1));
			p[0] = stateArray[i][0] + dt * dudt1[0];
			p[1] = stateArray[i][1] + dt * dudt1[1];
			
			dudt2 = odeSolveMotionFromForce(forceArray[i], p, mass[i], i != (stateArray.Length-1));
			stateArray[i][0] = stateArray[i][0] + dt/2f * (dudt1[0] + dudt2[0]);
			stateArray[i][1] = stateArray[i][1] + dt/2f * (dudt1[1] + dudt2[1]);
			
			
		}
		
		return stateArray;
	}


	/// <summary>
	/// Create relative velocity tangent. ropeDir points from Body2 to Body1 the pivot
	/// </summary>
	/// <returns>The velocity tangent relative.</returns>
	/// <param name="vel1">Vel1.</param>
	/// <param name="vel2">Vel2.</param>
	/// <param name="ropeDir">Rope dir. must be pre normqliaed</param>
	public Vector3 createVelocityTangentRelative(Vector3 vel1, Vector3 vel2, Vector3 ropeDir){
		Vector3 dVel = vel2 - vel1;
		return (dVel - ropeDir * Vector3.Dot(ropeDir, dVel));
	}


	
	/// <summary>
	/// Create velocity tangent. ropeDir points from Body2 to Body1 the pivot
	/// </summary>
	/// <returns>The velocity tangent.</returns>
	/// <param name="dVel">D vel.</param>
	/// <param name="ropeDir">Rope dir. must be normalized already</param>
	public Vector3 createVelocityTangent(Vector3 dVel, Vector3 ropeDir){
		return (dVel - ropeDir * Vector3.Dot(ropeDir, dVel));
	}
	
	
	
	public Vector3 getAccelerationNormal(Vector3 velTangent, Vector3 ropeDir, float segmentLength){
		return ropeDir*(Vector3.Dot(velTangent, velTangent) / segmentLength ) ;
	}

	public GameObject[] getRopeArray(){
		return ropeArray;
	}
	/// <summary>
	/// Deletes all game objects used by the rope.
	/// </summary>
	public void DeleteRope(){
		for (int i = 1; i < ropeArray.Length-1; i++){
			Destroy (ropeArray[i]);
		}
		Destroy(ropeMesh.getMeshGameObject());
	}

	public void SetRopeMaterial(Material mat){
		//ropeMesh.setMeshMaterial(mat);
		ropeMesh.getMeshGameObject().GetComponent<MeshRenderer>().material = mat;
	}

	public void setMainBodyForce(Vector3 F){
		mainBodyInputForce = F;
	}

	public void setRopeParams(RopeParams rp){
		// default rope parameters
		iterations = rp.iterations;

		ropeSegmentLength = rp.ropeSegmentLength;
		ropeSegmentMass = rp.ropeSegmentMass;
		ropeSphereRadius = rp.ropeSphereRadius;
		
		useCriticalDamping = rp.useCriticalDamping;
		noSpringyRopes = rp.noSpringyRopes;
		clampTreshhold = rp.clampTreshhold;
		
		useBoxCollider = rp.useBoxCollider;
		boxStretchFac = rp.boxStretchFac;
		
		k_spring = rp.k_spring;
		c_damp = rp.c_damp;

		ropeVertices = rp.ropeVertices;
		ropeRadius = rp.ropeRadius;
	}

	public Vector3 getForceAtRopeEnd(){
		return (forceArray != null)? forceArray[0] : new Vector3(0, 0, 0);
	}
}
