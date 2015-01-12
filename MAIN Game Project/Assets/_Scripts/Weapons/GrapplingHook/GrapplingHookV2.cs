using UnityEngine;
using System.Collections;

public class GrapplingHookV2 : MonoBehaviour {
	
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
	private Vector3 ropeForceAtEnd;
	
	private GameObject nRope;								// rope that is instantiated

	public RopeParams ropeParams;

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
	public basicEnemy enemy;
	
	// Sound stuff
	public AudioClip audioShoot;
	private AudioSource ropeShoot;
	private AudioSource ropeRetract;
	private bool ropeRetracted = false;

	// dynamic Rope
	DynamicRope5 DR5;
	
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
		Destroy(nRope.gameObject);

		if (HookVisual.GetComponent<DynamicRope5>()){
			HookVisual.GetComponent<DynamicRope5>().DeleteRope();
			Destroy (HookVisual.GetComponent<DynamicRope5>());
		}
		
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
				//hitObject.rigidbody.AddForceAtPosition(-ropeSwing.forceInRope *Time.deltaTime, ropePointEnd, ForceMode.Impulse);
				hitObject.rigidbody.AddForceAtPosition(ropeForceAtEnd, ropePointEnd, ForceMode.Force);
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
				firedHook.renderer.enabled = true;
				nRope.renderer.enabled = true;
				
				// RayCast for hitpoint
				// we ignore all object in player layer
				
				RaycastHit hit;
				
				int layerMask = playerLayerMask;
				layerMask = ~layerMask;
				
				if (Physics.Raycast(firedHook.transform.position, fireDirection, out hit,  projectileSpeed * Time.deltaTime *8.0f, layerMask) ){
					firedHook.transform.position = hit.point - firedHook.transform.forward * offset;
					hookStatus  = 2; // grapple locked/hooked
					
					ropePointEnd = firedHook.transform.position;					
					
					// make it parent to hit object and keep original scale;
					// must do it by matrices and not a transfor.parent because unity does scaling weird on the child
					
					Matrix4x4 hitMatrixW2L = hit.transform.worldToLocalMatrix;
//					Matrix4x4 hitMatrixL2W = hit.transform.localToWorldMatrix;
					
					localPos = hitMatrixW2L.MultiplyPoint(firedHook.transform.position);
					localDir = Vector3.Normalize(localPos - hitMatrixW2L.MultiplyPoint(firedHook.transform.position+firedHook.transform.forward) );

					hitObject = hit.transform.gameObject;
					
					ropeShoot.Stop();
					if(hit.collider.tag.Equals("Enemy")){
						hookStatus = 3;
						enemy = hit.transform.gameObject.GetComponent<basicEnemy>();
						enemy.hookHit();
						ropeShoot.Stop ();
					}
					
				}
				else{
					
					firedHook.transform.position = firedHook.transform.position + fireDirection * projectileSpeed * Time.deltaTime;
					
					if (ropeLength >= maxExtend){
						hookStatus  = 3; // retracting
						ropeShoot.Stop();
						highScore.grapplingHookMiss ();
					}
				}
				
			}
			
			
			// Let the player Swing;
			if (hookStatus == 2){
				DynamicRope5 dr;
				if (!HookVisual.GetComponent<DynamicRope5>()){
					dr = HookVisual.AddComponent<DynamicRope5>();
					dr.noHit = false;
					dr.HitPointObject = firedHook;
					dr.setRopeParams(ropeParams);
					//thisOwner.rigidbody.useGravity = false;

				}
				else{

					dr = HookVisual.GetComponent<DynamicRope5>();
					dr.SetRopeMaterial(nRope.GetComponent<MeshRenderer>().material);
					ropeForceAtEnd = dr.getForceAtRopeEnd();
				}

				// make rope longer/shorter with Q/E keys
				if (KeyManager.keyQ == 2){
					dr.L_desired = Mathf.Min (dr.L_desired + lenExtend*Time.deltaTime, maxExtend);
				}
				else if (KeyManager.keyE == 2){
					dr.L_desired = Mathf.Max (dr.L_desired - lenShorten * Time.deltaTime, 1.0f);
				}
			
				nRope.renderer.enabled = false;

				
				// parent reference, without unity's own parent function; Parent it as long hitObject exists
				if (hitObject != null){
//					Matrix4x4 hitMatrixW2L = hitObject.transform.worldToLocalMatrix;
					Matrix4x4 hitMatrixL2W = hitObject.transform.localToWorldMatrix;
					
					ropePointEnd = hitMatrixL2W.MultiplyPoint(localPos);
					
					firedHook.transform.position = ropePointEnd;
					firedHook.transform.rotation = Quaternion.LookRotation(hitMatrixL2W.MultiplyVector(-localDir));
				}
				else{
					hookStatus = 3;
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

			if (hookStatus != 2){
				if (HookVisual.GetComponent<DynamicRope5>()){
					HookVisual.GetComponent<DynamicRope5>().DeleteRope();
					Destroy (HookVisual.GetComponent<DynamicRope5>());
				}
				thisOwner.rigidbody.useGravity = true;
			}
		}
		
		// Fire Mechanism
		if (fire == true && hookStatus == 0){
			firedHook = Instantiate(HookProjectile, projectileSpawnObject.transform.position, projectileSpawnObject.transform.rotation) as GameObject;
			
			ropePointEnd = firedHook.transform.position;
			
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
			if(hookStatus == 1){
				highScore.grapplingHookMiss();
			}
			ropeShoot.Stop();
			hookStatus = 3;
			
		}
		
	}
}
