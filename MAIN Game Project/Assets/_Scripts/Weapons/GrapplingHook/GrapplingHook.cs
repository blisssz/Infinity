using UnityEngine;
using System.Collections;

public class GrapplingHook : MonoBehaviour {

	public LayerMask playerLayerMask;
	// the owner of this Weapon
	public GameObject thisOwner {get; set;}

	private GameObject HookVisual;		// stores the static hook object on this weapon

	private int hookStatus = 0;			// 0 ready to fire, 1 fired, 2 hook, 3 retracting;

	public static bool enemyHit;


	// projectile stuff
	public GameObject projectileSpawnObject {get; set;}		// The location where projectiles are added
	public GameObject HookProjectile;						// projectile Object visual only
	public float offset = 1.5f;								// offset of hook embedding in objects
	public float projectileSpeed;

	private GameObject firedHook;							// the instansiated projectile
	private Vector3 fireDirection;


	//------ Rope Stuff
	public GameObject rope;									// rope visual, that will be scaled
	public float ropeSize = 0.5f;
	public Vector3 ropePointStart {get; set;}
	public Vector3 ropePointEnd {get; set;}

	private GameObject nRope;								// rope that is instantiated

	// rope swing class and parameters
	private GrapplingRopeSwing ropeSwing;		// class that handles rope swinging
	public bool useCriticalDamping = true;
	public bool useRigidRope = false;
	
	public float k_spring = 5000.0f;			// rope linear spring constant [N/m]
	public float c_damp = 100.0f;				// rope linear damping constant [N/(dm/dt)]



	// Retract Rope parameters
	public float maxExtend = 100.0f;
	public float maxRetractTime = 3.0f;
	private float retractTime = 0.0f;

	private float retractingLength = 0.0f;		// stores the length just before retracting


	// Extend/Shorten Length;
	public float lenExtend = 2.0f;		// m of rope/sec
	public float lenShorten = 2.0f;		// m of rope/sec


	// Hit Object Stuff
	private GameObject hitObject;
	private Vector3 localPos;
	private Vector3 localDir;
	private basicEnemy enemy;

	// Sound stuff
	public AudioClip audioShoot;
	private AudioSource ropeShoot;
	private AudioSource ropeRetract;
	private bool ropeRetracted = false;

	// Use this for initialization
	void Start () {

		thisOwner = transform.root.gameObject;

		foreach (Transform child in GetComponentsInChildren<Transform>() ){
			if (child.name.Equals("ProjectilePoint")){
				HookVisual =  child.gameObject;
			}
		}

		
		// check if thisOwner has a rope swing script:
		if (!thisOwner.GetComponent<GrapplingRopeSwing>()){
			thisOwner.AddComponent<GrapplingRopeSwing>();
		}
		// set rope swing data
		ropeSwing = thisOwner.GetComponent<GrapplingRopeSwing>() as GrapplingRopeSwing;
		ropeSwing.k_spring = k_spring;
		ropeSwing.c_damp = c_damp;
		ropeSwing.useCriticalDamping = useCriticalDamping;
		ropeSwing.useRigidRope = useRigidRope;

		// create a rope
		nRope = Instantiate (rope) as GameObject;
		nRope.transform.localScale =  new Vector3(0, 0, 0);
		nRope.transform.position = HookVisual.transform.position; //projectileSpawnObject.transform.position; //HookVisual.transform.position; 
		nRope.transform.parent = HookVisual.transform; //projectileSpawnObject.transform; //HookVisual.transform;

		// Sound
		AudioSource[] audio = GetComponents<AudioSource>();
		ropeShoot = audio[0];
		ropeRetract = audio[1];
		ropeRetract.pitch = 1.5f;

	}

	// Destroys all reference objects and components with this weapon, like rope and fired projectile
	// Because by calling Destroy(this.gameObject) remaining parts will float around in the game
	public void DestroyAll(){
		//Destroy (ropeSwing);		// destroy <GrapplingRopeSwing> component
		ropeSwing.noHit = true;
		Destroy(nRope.gameObject);

		if (firedHook != null){
			Destroy(firedHook.gameObject);
		}
		// destroy this weapon + children
		Destroy (this.gameObject);
	}

	void FixedUpdate(){
		// physics for hit object
		if (hitObject != null){
			if (hitObject.GetComponent<Rigidbody>()){
				//hitObject.rigidbody.AddForce(-ropeSwing.forceInRope *Time.deltaTime, ForceMode.Impulse);
				if(hitObject.tag.Equals("Enemy")){
					hitObject.rigidbody.AddForceAtPosition(-ropeSwing.forceInRope *Time.deltaTime, ropePointEnd, ForceMode.Impulse);
				}
			}
		}
	}

	void LateUpdate () {

		bool fire = KeyManager.leftMouse == 2;

		ropePointStart = HookVisual.transform.position;	//projectileSpawnObject.transform.position;  //HookVisual.transform.position; 

		// have we fired a grapplingHook?
		if (firedHook != null){

			Vector3 Rope_E2S = (ropePointStart - firedHook.transform.position);
			Vector3 direction = Vector3.Normalize(Rope_E2S);
			float ropeLength = Rope_E2S.magnitude;

			ropePointEnd = firedHook.transform.position;


			if (hookStatus  == 1){
				ropeSwing.noHit = true;
				firedHook.renderer.enabled = true;
				nRope.renderer.enabled = true;

				// RayCast for hitpoint
				// we ignore all object in player layer
				
				RaycastHit hit;
				
				int layerMask = playerLayerMask;
				layerMask = ~layerMask;
				
				if (Physics.Raycast(firedHook.transform.position, fireDirection, out hit,  projectileSpeed * Time.deltaTime *2.0f, layerMask) ){
					firedHook.transform.position = hit.point - firedHook.transform.forward * offset;
					hookStatus = 2; // grapple locked/hooked
					ropePointEnd = firedHook.transform.position;					
					ropeSwing.L_desired = (ropePointEnd - ropePointStart).magnitude;

					// make it parent to hit object;
					Vector3 hitScale = hit.transform.lossyScale;
					Vector3 currentScale = firedHook.transform.localScale;
					//firedHook.transform.localScale = new Vector3(currentScale.x/hitScale.x, currentScale.y/hitScale.y, currentScale.z/hitScale.z);
					GameObject emptyObject = new GameObject();
					//emptyObject.transform.parent = hit.transform;

					//

					Matrix4x4 hitMatrixW2L = hit.transform.worldToLocalMatrix;
					Matrix4x4 hitMatrixL2W = hit.transform.localToWorldMatrix;

					localPos = hitMatrixW2L.MultiplyPoint(firedHook.transform.position);
					localDir = Vector3.Normalize(localPos - hitMatrixW2L.MultiplyPoint(firedHook.transform.position+firedHook.transform.forward) );

					hitObject = hit.transform.gameObject;

					ropeShoot.Stop();
					if(hit.collider.tag.Equals("Enemy")){
						hookStatus = 3;
						enemy = hit.transform.gameObject.GetComponent<basicEnemy>();
						enemy.hookHit();
						ropeShoot.Stop ();
						ropeSwing.noHit = true;
					}
				}
				else{

					firedHook.transform.position = firedHook.transform.position + fireDirection * projectileSpeed * Time.deltaTime;

					if (ropeLength >= maxExtend){
						hookStatus  = 3; // retracting
						ropeShoot.Stop();
						highScore.missedGrapplingHook += 1;
					}
				}



			}


			// Let the player Swing;
			if (hookStatus == 2){
				ropeSwing.noHit = false;

				// make rope longer/shorter with Q/E keys
				if (KeyManager.keyQ == 2){
					ropeSwing.L_desired = Mathf.Min (ropeSwing.L_desired + lenExtend*Time.deltaTime, maxExtend);
				}
				else if (KeyManager.keyE == 2){
					ropeSwing.L_desired = Mathf.Max (ropeSwing.L_desired - lenShorten * Time.deltaTime, 0.0f);
				}

				// parent reference, without unity's own parent function;
				if(hitObject == null){
					hookStatus = 3;
				}

				else{
					Matrix4x4 hitMatrixW2L = hitObject.transform.worldToLocalMatrix;
					Matrix4x4 hitMatrixL2W = hitObject.transform.localToWorldMatrix;
				
				
					ropePointEnd = hitMatrixL2W.MultiplyPoint(localPos);
				
					firedHook.transform.position = ropePointEnd;
					firedHook.transform.rotation = Quaternion.LookRotation(hitMatrixL2W.MultiplyVector(-localDir));
				
					ropeSwing.HitPoint = ropePointEnd;
				}

			

			}

			// Retract
			if (hookStatus == 3){
				hitObject = null;
				// Rope end to start vector
				retractingLength = Mathf.Max(ropeLength, retractingLength);
				float interpolatefac = Mathf.Max((1.0f - retractTime/maxRetractTime * maxExtend/retractingLength ), 0.0f);

				firedHook.transform.position = ropePointStart + -direction * retractingLength * interpolatefac;
				firedHook.transform.rotation = Quaternion.LookRotation(-direction);
				retractTime += Time.deltaTime;
				//firedHook.transform.position = firedHook.transform.position + direction * projectileSpeed * Time.deltaTime;

				if (ropeRetracted == false){
					ropeRetract.Play ();
					ropeRetracted = true;
				}

				if (interpolatefac  == 0.0f){
					hookStatus = 0;
					retractTime = 0.0f;
					retractingLength = 0.0f;
					Destroy (firedHook.gameObject);

					nRope.transform.localScale =  new Vector3(0, 0, 0);
					HookVisual.renderer.enabled = true;

					ropeRetract.Stop();
					ropeRetracted = false;
				}

			}

			// show Rope (at the end so it matches all positions)
			if (hookStatus != 0){
				
				HookVisual.renderer.enabled = false;				
				// rope Code
				Vector3 VirtualScale = HookVisual.transform.parent.transform.localScale;
				nRope.transform.localScale =  new Vector3(ropeSize, ropeSize, ropeLength) / VirtualScale.z;			
				nRope.transform.LookAt (firedHook.transform.position);
				
			}
		}

		// Fire Mechanism
		if (fire == true && hookStatus == 0){
			firedHook = Instantiate(HookProjectile, projectileSpawnObject.transform.position, projectileSpawnObject.transform.rotation) as GameObject;
			ropePointEnd = firedHook.transform.position;
			//Debug.Log (HookVisual.transform.rotation);

			firedHook.transform.rotation = transform.rotation;

			fireDirection = transform.forward;	// fires in direction of the attach rotation

			firedHook.transform.LookAt (firedHook.transform.position + HookVisual.transform.forward);

			firedHook.renderer.enabled = false;
			nRope.renderer.enabled = false;

			hookStatus  = 1;
			ropeShoot.Play ();


			AudioSource.PlayClipAtPoint(audioShoot, transform.position, 1.5f);
		}

		if  (hookStatus != 0 && hookStatus != 3 && KeyManager.rightMouse == 2){
			ropeShoot.Stop();
			hookStatus = 3;
			ropeSwing.noHit = true;

		}

	}
}
