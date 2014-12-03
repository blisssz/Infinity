using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicRopeR : MonoBehaviour {
	
	public GameObject rope;
	
	public float mainBodyOffset = 3.0f;		// offset from center in up direction
	
	public float hitPointsOffset = 0.6f;
	
	public LayerMask ignoreLayers;
	
	public Transform HitPointTrans;
	
	// Data for swinging
	public bool noHit {get; set;}
	public Vector3 HitPoint {get; set;}			// always the point where the rope is attached.
	public Vector3 forceInRope {get; set;}
	
	public float L_desired {get; set;}			// the total length of the rope;
	private int frame = 0;

	// offsets
	public float tangentOffset = 1.0f;
	public float normalOffset = 0.3f;
	
	//
	private Vector3 currentTangent;
	
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


			Vector3 velRope  = new Vector3(0, 0, 0);
			Vector3 velBody  = new Vector3(0, 0, 0);			
			Vector3 velBodyDir = new Vector3(0, 0, 0);

			Vector3 tangent2Normal;

			
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



					// got a hit, get data from Velocity info if it has that component 
					// And not the same index as previous, so it can keep using the velocity data
					if (lastIndex != i){
						if (hit.transform.root.GetComponent<VelocityInfo>()){
							velBody = hit.transform.root.GetComponent<VelocityInfo>().getVelocityAtPoint(hit.point);
							velBodyDir = velBody.normalized;
						}
						else{
							velBody = new Vector3(0, 0, 0);
							velBodyDir = new Vector3(0, 0, 0);
						}
						if (i+1 != transfArray.Length - 1){
							velRope = hit.distance/currentLength * transfArray[i+1].GetComponent<VelocityInfo>().getWorldVelocity() + (1 -  hit.distance/currentLength) * transfArray[i].GetComponent<VelocityInfo>().getWorldVelocity() ;
						}
						else{ 
							velRope = hit.distance/currentLength * this.transform.root.rigidbody.velocity + (1 -  hit.distance/currentLength) * transfArray[i].GetComponent<VelocityInfo>().getWorldVelocity() ;
						} 
					}

					// add new object
					GameObject newObj = new GameObject();
					newObj.name = "RopeSubHitPoint";
					newObj.AddComponent<VelocityInfo>();

					// calc signed projected magnitudes. if Vrope(proj) >= Vbody then search in negative of Vrope
					if (Vector3.Dot(velBodyDir, velRope) >= Vector3.Dot(velBodyDir, velBody) ){
						// search for points in the negative velocity of the rope
						tangent2Normal = tangentFromRefTangent(-velRope, hit.normal);
					}

					else{
						// create offset vector towards velBody vector
						tangent2Normal = tangentFromRefTangent(velBody, hit.normal);
					}

					newObj.transform.position = hit.point + tangent2Normal * tangentOffset + hit.normal * normalOffset;
					newObj.transform.rotation = Quaternion.identity;
					newObj.transform.parent = hit.transform;

					// add rope
					GameObject newRope = Instantiate(rope, newObj.transform.position, Quaternion.identity) as GameObject;
					newRope.transform.parent = newObj.transform;
					
					// must insert a new point in the hitPosList;
					hitPosList.Insert(i+1, newObj.transform);
					ropeList.Insert(i+1, newRope.transform);
					
					transfArray = hitPosList.ToArray();
					ropeArray = ropeList.ToArray();

					lastIndex = i;


					// check list if we can remove an attachpoint]
					// then i--
					
					// must look atleast have 1 behind and 1 in front
					if (i != 0){
						
						ropeDirection = transfArray[i+1].position - transfArray[i-1].position;
						currentLength = ropeDirection.magnitude;
						ropeDirection /= currentLength;		// normalized;
						
						// cast ray
						if (Physics.Raycast(transfArray[i-1].position, ropeDirection, currentLength, ~ignoreLayers) ){
							// hit, destroy gameObject and pop from array
							Destroy(transfArray[i].gameObject);		// remove instance from game
							Destroy (ropeArray[i].gameObject);
							
							hitPosList.RemoveAt(i);					// pop from list
							ropeList.RemoveAt(i);
							
							transfArray = hitPosList.ToArray();
							ropeArray = ropeList.ToArray();
							i--;
							
						}
					}

				}// raycast done
				



				
				
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
					if (!Physics.Raycast(transfArray[i-1].position, ropeDirection, currentLength, ~ignoreLayers) ){
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
