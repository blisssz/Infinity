using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicRope2 : MonoBehaviour {

	public GameObject rope;

	public float mainBodyOffset = 3.0f;		// offset from center in up direction

	public float hitPointsOffset = 0.6f;

	public LayerMask ignoreLayers;

	public bool useCriticalDamping = true;
	public bool useRigidRope = true;
	
	public float k_spring = 1000.0f;
	public float c_damp = 100.0f;

	public Transform HitPointTrans;
	
	// Data for swinging
	public bool noHit {get; set;}
	public Vector3 HitPoint {get; set;}			// always the point where the rope is attached.
	public Vector3 forceInRope {get; set;}
	
	public float L_desired {get; set;}			// the total length of the rope;
	private int frame = 0;


	
	// tracking Lists
	private List<GameObject> rbList;
	private GameObject[] obArray;
	private float[] E_deriv = new float[200];
	private float[] E_integral = new float[200];

	private List<Transform> hitPosList;
	private List<Transform> tangentRope;
	private List<Transform> ropeList;

	private Vector3 lastAttachpointOffset;
	private int lastIndex;
	
	//
	
	private int bodies = 0;
	
	// Use this for initialization
	void Start () {
		hitPosList = new List<Transform>();
		hitPosList.Add (HitPointTrans);
		hitPosList.Add (this.transform);

		ropeList = new List<Transform>();
		GameObject newRope = Instantiate(rope, HitPointTrans.position, Quaternion.identity) as GameObject;
		newRope.transform.parent = HitPointTrans;
		ropeList.Add(newRope.transform);

		tangentRope = new List<Transform>();
		tangentRope.Add (HitPointTrans);
		tangentRope.Add (this.transform);

		L_desired = (transform.position - HitPoint).magnitude;
		lastIndex = -1;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		frame += 1;
		if (noHit == true){
			// reset list
			rbList = new List<GameObject>();
			
		}
		else{ // we have a hit
			Vector3 ropeDirection; 
			float currentLength;
			RaycastHit hit; // struct for raycasts

			Vector3 offsetPosition = transform.position + transform.up * mainBodyOffset;

			Transform[] transfArray = hitPosList.ToArray();
			Transform[] ropeArray = ropeList.ToArray();
			int i = 0;
			int j = 0;
			while( i < transfArray.Length - 1 && j < 100){
				j++;
			//for (int i = 0; i < transfArray.Length - 1; i++){
				ropeDirection = transfArray[i+1].position - transfArray[i].position;
				currentLength = ropeDirection.magnitude;
				ropeDirection /= currentLength;		// normalized;


				// cast ray to every point to check for no obstructions

				if (Physics.Raycast(transfArray[i].position, ropeDirection, out hit, currentLength, ~ignoreLayers) ){

					if (hit.transform.root.gameObject.GetInstanceID() != this.transform.root.gameObject.GetInstanceID()){
						// hit: add a new object for hits
						GameObject newObj = new GameObject();
						newObj.name = "RopeSubHitPoint";


						// we have to add the position in a offset so that we can still raycast from the new position
						if (lastIndex != i){
							Vector3 tangent2Normal = ropeDirection - hit.normal * Vector3.Dot (hit.normal, ropeDirection);
							tangent2Normal.Normalize();

							Vector3 tangent2Rope = hit.normal -  -ropeDirection * Vector3.Dot (hit.normal, -ropeDirection);
							tangent2Rope.Normalize();

							// set position on point with offset in normal and tangent direction
							lastAttachpointOffset =(hit.normal + tangent2Normal) * hitPointsOffset;
							newObj.transform.position = hit.point + lastAttachpointOffset;

							newObj.transform.rotation = Quaternion.LookRotation(tangent2Rope);
							newObj.transform.parent = hit.transform;
						}
						//else use previous offset, so you cant become stuck on a flat surface
						else{
							newObj.transform.position = hit.point + lastAttachpointOffset;							
							newObj.transform.rotation = Quaternion.identity;
							newObj.transform.parent = hit.transform;
						}

						GameObject newRope = Instantiate(rope, newObj.transform.position, Quaternion.identity) as GameObject;
						newRope.transform.parent = newObj.transform;

						// must insert a new point in the hitPosList;
						hitPosList.Insert(i+1, newObj.transform);
						tangentRope.Insert(i+1, newObj.transform);
						ropeList.Insert(i+1, newRope.transform);

						transfArray = hitPosList.ToArray();
						ropeArray = ropeList.ToArray();

						
						lastIndex = i;

						//#### Initial check after ray cast
						// check list if we can remove an attachpoint]
						// then i--

						// must look atleast have 1 behind and 1 in front
						if (i != 0){

							ropeDirection = transfArray[i+1].position - transfArray[i-1].position;
							currentLength = ropeDirection.magnitude;
							ropeDirection /= currentLength;		// normalized;

							// cast ray
							if (Physics.Raycast(transfArray[i-1].position, ropeDirection, out hit, currentLength, ~ignoreLayers) ){
								// hit, destroy gameObject and pop from array
								Destroy(transfArray[i].gameObject);		// remove instance from game
								Destroy (ropeArray[i].gameObject);

								hitPosList.RemoveAt(i);					// pop from list
								ropeList.RemoveAt(i);

								transfArray = hitPosList.ToArray();
								ropeArray = ropeList.ToArray();
								i--;

							}

							/* OLD
							Vector3 prevDir =  transfArray[i-1].transform.position - transfArray[i].transform.position;
							Vector3 newDir =   transfArray[i+1].transform.position - transfArray[i].transform.position;

							//Vector3 tangent = createTangent(Vector3.Normalize(prevDir));
							Vector3 tangent = tangentFromRefTangent(transfArray[i].forward, Vector3.Normalize(prevDir) );

							// if dot product >= 0: remove previous point
							if (Vector3.Dot (newDir, tangent) >= 0){

								Destroy(transfArray[i].gameObject);		// remove instance from game
								hitPosList.RemoveAt(i);					// pop from list
								transfArray = hitPosList.ToArray();
								i--;
							}
							*/ //^^OLD

						}
					}
				}

				// align ropes
				ropeDirection = transfArray[i+1].position - transfArray[i].position;
				currentLength = ropeDirection.magnitude;
				ropeDirection /= currentLength;		// normalized;

				ropeArray[i].rotation = Quaternion.LookRotation(ropeDirection);
				ropeArray[i].localScale = new Vector3(0.1f, 0.1f, currentLength);
				
				//### Continious check for ropes (better build tangent from a ref tangent)
				if (i != 0 && i < transfArray.Length-1){
					ropeDirection = transfArray[i+1].position - transfArray[i-1].position;
					currentLength = ropeDirection.magnitude;
					ropeDirection /= currentLength;		// normalized;

					//Debug.Log (transfArray[i-1].name);
					//Debug.Log (transfArray[i+1].name);
					// cast ray
					if (!Physics.Raycast(transfArray[i-1].position, ropeDirection, out hit, currentLength, ~ignoreLayers) ){
						//no hit, destroy gameObject and pop from array
						Destroy(transfArray[i].gameObject);		// remove instance from game
						Destroy (ropeArray[i].gameObject);

						hitPosList.RemoveAt(i);					// pop from list
						ropeList.RemoveAt(i);
						
						transfArray = hitPosList.ToArray();
						ropeArray = ropeList.ToArray();
						i--;						
					}
					else{
						//Debug.Log(hit.transform.name);
					}


					/* OLD
					Vector3 prevDir =  transfArray[i-1].transform.position - transfArray[i].transform.position;
					Vector3 newDir =   transfArray[i+1].transform.position - transfArray[i].transform.position;
					
					//Vector3 tangent = createTangent(Vector3.Normalize(prevDir));
					Vector3 tangent = tangentFromRefTangent(transfArray[i].forward, Vector3.Normalize(prevDir) );
					//Debug.Log(tangent);
					
					// if dot product >= 0: remove previous point
					if (Vector3.Dot (newDir, tangent) >= 0){
						
						Destroy(transfArray[i].gameObject);		// remove instance from game
						hitPosList.RemoveAt(i);					// pop from list
						transfArray = hitPosList.ToArray();
						i--;
					} */ //OLD
					
				}
				

				//### Continious check end
				i++;



			}
		
			
			// physics calculations
			float ds;				// length of segment - ref_length
			float s_segment;		// length of current segment
			Vector3 dir_segment= new Vector3(0,0 ,0);;	// direction of the segment
			float c_damp =0;			// damping constant
			Vector3 ropeForce = new Vector3(0,0 ,0);;		// rope force
			
			Vector3 v_dir = new Vector3(0,0 ,0);; 			// velocity direction in dir, direction
			
			Vector3 currentForce = new Vector3(0,0 ,0);	
			Vector3 prevForce;
			
			float dE;

			
		}
		
		
	}

	/** Creates a normalized tangent vector from a Vector3 by cross products
	 * normal must be normalized
	 * (Only gives a problem when normal == +-Up)
	 */
	public Vector3 createTangent(Vector3 normal){
		// normal points in y dir
		Vector3 Up = new Vector3(0, 1, 0);
		Vector3 biNormal = Vector3.Normalize(Vector3.Cross (normal, Up)); // x dir

		return Vector3.Cross (biNormal, normal); // z dir

	}

	/** Makes a reftan a orthonormal tangent to normal
	 * normal and reftan must be normalized already
	 */
	public Vector3 tangentFromRefTangent(Vector3 reftan, Vector3 normal){
		return Vector3.Normalize (reftan - normal * Vector3.Dot (reftan, normal));
	}


}
