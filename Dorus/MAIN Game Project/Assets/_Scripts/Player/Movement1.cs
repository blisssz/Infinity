using UnityEngine;
using System.Collections;

///<summary> Player movement script, physics based with a PID controller. 
/// Attach Script to a GameObject. This GameObject must have a Rigidbody Component.
/// 
/// Mandatory Scripts this class uses:
/// - KeyManager.cs
/// This class refers to scripts
/// - VelocityInfo.cs; <summary>
///

public class Movement1 : MonoBehaviour {
	
	// mask to ignore raycast
	public LayerMask playerLayerMask;
	
	// control values (some good base parameters for mass of 99kg)
	public float v_max = 10.0f;
	public float KpBaseVal = 75.0f;
	public float Kp {get; set;}
	public float Ki = 10.0f;
	public float KiClamp = 35.0f;
	public float Lcomp = 0.3f;
	
	public float walkForceMax = 2500.0f;	// Newton, (a 90kg Human Force is about 750N)
	private Vector3 inputForce;
	
	public float c_air = 0.5f;
	public float A_obj = 1.0f;
	
	public float slideControlFac = 0.1f;
	public Vector3 DeltaPosition;
	
	public float playerCenter2Ground_height = 0.6f;
	
	private Vector3 E_old = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 E_integral = new Vector3(0.0f, 0.0f, 0.0f);
	
	
	
	//max walk angle;
	public float maxWalkAngle = 45.0f;
	private float maxWalkSlope;
	
	// jump stuff
	public float jump_acceleration = 5.0f;	// [m/s^2], when g == 0.0
	public float jump_maxtime = 0.4f;
	private float jump_time = 0.0f;
	private float extra_jump_timer = 0.0f;
	public  float extra_jump_time = 0.3f;
	private bool allow_extra_jump = true;
	
	private Vector3 V_inherit = new Vector3(0.0f, 0.0f, 0.0f);
	
	private float vfac = 1.0f;
	public float airControl_fac = 1.0f;
	
	// States
	public int JumpState {get; set;}		// 0: on ground, 1: is jumping. 2: is landing
	public int MovementState {get; set;}	// 0: on ground, 1: in the air, 2: sliding
	
	
	// cameras
	private GameObject playerCam;
	private GameObject weaponCam;
	
	// head bob params
	public float headBobAmp = 0.05f;
	public float headBobFreq = 10.0f;
	private float headBobTime = 0.0f;
	private float iheadFac;
	
	
	private bool movementEnabled;
	private bool airControlEnabled;
	private bool groundControlEnabled;
	private bool jumpingEnabled;
	private bool springyfeetEnabled = true;
	
	// stat tracking
	private float distanceTravelled = 0f;
	private float timeInit;
	private Vector3 prevPos;
	
	
	// Use this for initialization
	void Start () {
		maxWalkSlope = Mathf.Cos(maxWalkAngle/180f * Mathf.PI);
		Kp = KpBaseVal * rigidbody.mass;
		
		foreach (Transform tchild in GetComponentsInChildren<Transform>()){
			if (tchild.name.Equals("Main Camera")){
				playerCam = tchild.gameObject;
			}
			if (tchild.name.Equals("Weapon Camera")){
				weaponCam = tchild.gameObject;
			}
		}
		
		movementEnabled = true;
		airControlEnabled = true;
		groundControlEnabled = true;
		jumpingEnabled = true;
		
		
		distanceTravelled = 0f;
		timeInit = Time.time;
		prevPos = this.transform.position;
	}
	
	
	void FixedUpdate () {
		
		if (movementEnabled){
			float mass = rigidbody.mass;
			
			//			bool shiftKey = KeyManager.leftShift == 2;
			
			bool jump = KeyManager.jump == 2;
			bool forward = KeyManager.forward == 2;
			bool backward = KeyManager.backward == 2;
			bool left = KeyManager.left == 2;
			bool right = KeyManager.right == 2;
			
			Vector3 springUpForce = Vector3.zero;
			
			// ray cast for checking if the player walks on ground;
			RaycastHit hit;
			
			if (Physics.Raycast(transform.position, -transform.up, out hit, 5.01f, ~playerLayerMask) ){
				
				Vector3 center2hitPos = (hit.point - transform.position);
				
				vfac = 0.2f;
				
				
				if (center2hitPos.magnitude < playerCenter2Ground_height && JumpState != 1){
					MovementState = 0;
					
					// make sure that space is not pressed so that jump resets and is not spammable.
					if (jump != true){
						JumpState = 0;
						jump_time = 0.0f;
						extra_jump_timer = Time.realtimeSinceStartup;
					}
					
					vfac = 1.0f;
					
					
					GroundDamageEffects.doGroundDamage(this.gameObject,hit.transform.gameObject);
					
					// calc spring Up force
					if (springyfeetEnabled){
						float L0 = playerCenter2Ground_height + Lcomp;
						float k = rigidbody.mass * Physics.gravity.magnitude / Lcomp;
						float cdamp = 2f * Mathf.Sqrt(k*rigidbody.mass);
						float L_compression = L0 - hit.distance;
						
						springUpForce = transform.up * (cdamp * Vector3.Dot (rigidbody.velocity, -transform.up)
						                                + L_compression * k);
						
						//springUpForce = Vector3.ClampMagnitude(springUpForce,L_compression * 2*k);
					}
					
					if (Vector3.Dot(hit.normal, transform.up) < maxWalkSlope){
						MovementState = 2;		// sliding
					}
					
					// Get velocity of the object it walks on
					if (hit.transform.GetComponent<Rigidbody>()){
						V_inherit = hit.transform.GetComponent<Rigidbody>().GetPointVelocity (hit.point);
					}
					else if (hit.transform.GetComponent<VelocityInfo>()){
						// totVel = angularV + transV;
						Vector3 V_byAngV = Vector3.Cross (hit.transform.GetComponent<VelocityInfo>().getAngularVelocity(),(hit.point - hit.transform.position) );
						V_inherit = V_byAngV +  hit.transform.GetComponent<VelocityInfo>().getWorldVelocity();
					}
					
					else{
						V_inherit = new Vector3(0.0f, 0.0f, 0.0f);
					}
				}
				else{
					MovementState = 1; // not touching ground
					if (Time.realtimeSinceStartup - extra_jump_timer < extra_jump_time){
						allow_extra_jump = true;
						JumpState = 0;
					}
				}
			}
			else{
				MovementState = 1; // not touching ground
				if (Time.realtimeSinceStartup - extra_jump_timer < extra_jump_time){
					allow_extra_jump = true;
					JumpState = 0;
				}
			}
			
			
			if (jumpingEnabled){
				////////// Jumping
				if (jump == true && JumpState != 2 ){
					
					
					E_integral = new Vector3(0, 0, 0);		// reset integral
					
					if (JumpState == 0 && (MovementState != 1 || allow_extra_jump)){
						//V_inherit = rigidbody.velocity;
						JumpState = 1;
						MovementState = 1;
						allow_extra_jump = false;
						
					}
					if (JumpState == 1 && jump_time < jump_maxtime){
						jump_time += Time.deltaTime;
						
						// keep springyfeet force at zero else it increases the jump force
						springUpForce = Vector3.zero;
						
						// jump force
						rigidbody.AddForce (transform.up*mass*(jump_acceleration+Physics.gravity.magnitude), ForceMode.Force);
					}
					else{
						JumpState = 2;
					}
				}
				else if (MovementState == 1){
					JumpState = 2;
					//jumpingEnabled = false;
				}
			}
			
			////////// Movement And Control
			/// 
			Vector3 V_ref = movementVelocityRef(forward, backward, left, right, v_max);
			
			// intial force input;
			Vector3 U = new Vector3(0.0f, 0.0f, 0.0f);
			
			
			// Ground Control
			if (groundControlEnabled &&(MovementState == 0 || MovementState == 2)){
				// Vref to world coords
				V_ref = transform.localToWorldMatrix.MultiplyVector (V_ref*vfac) + V_inherit;
				
				Kp = KpBaseVal * rigidbody.mass;
				// control Kd value
				float Kd = 2.0f * Mathf.Sqrt (mass*Kp);
				
				
				
				// control linear velocity
				Vector3 E = V_ref - rigidbody.velocity;
				
				E_integral += E;
				E_integral.y = 0.0f;
				E_integral = Vector3.ClampMagnitude (E_integral, KiClamp);
				
				
				U = E* Kp + (E - E_old) * Kd + E_integral * Ki;
				U.y = 0.0f; // ignore applying velocity in direction of gravity
				
				// clamp input force
				U = Vector3.ClampMagnitude(U, walkForceMax);
				
				E_old = E;
				
				
				if (MovementState == 2){
					U *= slideControlFac;
				}
				else if(PlayerManager.useWeaponID!=6){ // head bob
					headBob(headBobAmp, headBobFreq);
				}
			}
			
			// Air Control
			else if (airControlEnabled && MovementState == 1){
				
				// get good results by trial and error, or genetic algorithm ;)
				vfac = airControl_fac;
				
				U = transform.localToWorldMatrix.MultiplyVector (V_ref/v_max*vfac)*mass - rigidbody.velocity * rigidbody.velocity.magnitude * c_air * A_obj;
			}
			
			inputForce = U;
			
			Vector3 outputForce = U + springUpForce;
			
			// apply forces
			rigidbody.AddForce (outputForce, ForceMode.Force);
		}
	}
	
	void Update(){
		updateDistanceTraveled();
	}
	
	
	/// <summary>  Set reference velocity for player by forward backward left right keys;
	/// The reference velocity is in local coordinates </summary>
	public Vector3 movementVelocityRef(bool forward, bool backward, bool left, bool right, float v_max){
		// set V_ref to the corresponding input
		Vector3 V_ref;
		
		if ((forward && !backward) && !(left || right) ){
			//rigidbody.AddForce (w_Z * U.z);
			V_ref = new Vector3(0.0f, 0.0f, v_max);
		}
		else if ((!forward && backward) && !(left || right) ) {
			V_ref = new Vector3(0.0f, 0.0f, -v_max);
		} 
		else if (!(forward || backward) && (left && !right)) {
			V_ref = new Vector3(-v_max, 0.0f, 0.0f);
		}
		else if (!(forward || backward) && (!left && right)) {
			V_ref = new Vector3(v_max, 0.0f, 0.0f);
		}
		else if ((forward && left) && !(backward || right)){
			V_ref = new Vector3(-v_max*0.7071f, 0.0f, v_max*0.7071f);
		}
		else if ((forward && right) && !(backward || left)){
			V_ref = new Vector3(v_max*0.7071f, 0.0f, v_max*0.7071f);
		}
		else if ((backward && left) && !(forward || right)){
			V_ref = new Vector3(-v_max*0.7071f, 0.0f, -v_max*0.7071f);
		}
		else if ((backward && right) && !(forward || left)){
			V_ref = new Vector3(v_max*0.7071f, 0.0f, -v_max*0.7071f);
		}
		else {
			V_ref = new Vector3(0.0f, 0.0f, 0.0f);
		}
		
		return V_ref;
	}
	
	
	
	/// <summary>
	/// <c>head bob player</c> </summary>
	/// <param name="amp">Amp.</param>
	/// <param name="freq">Freq.</param>
	public void headBob(float amp, float freq){
		
		// v factor of how fast bob
		// head bob
		headBobTime += Time.deltaTime * (rigidbody.velocity.magnitude - V_inherit.magnitude)/v_max;
		DeltaPosition=playerCam.transform.up * Mathf.Sin(headBobTime*freq)* amp *iheadFac;
		playerCam.transform.position = playerCam.transform.parent.transform.position;
		weaponCam.transform.position = playerCam.transform.position;

		if ((rigidbody.velocity.magnitude - V_inherit.magnitude) > 0.9f){
			iheadFac = Mathf.Min(iheadFac+ 0.10f, 1.0f);
			iheadFac = 1.0f;
		}
		else{ //interpolate to original pos;
			iheadFac = Mathf.Max(iheadFac-0.05f, 0.0f);
			if (iheadFac == 0.0f){
				headBobTime = 0.0f;
			}						
		}
	}
	
	
	public void disableMovement(){
		movementEnabled = false;
	}
	public void enableMovement(){
		movementEnabled = true;
	}
	
	public void enableGroundAndAirControl(){
		movementEnabled = true;
		airControlEnabled = true;
		groundControlEnabled = true;
	}
	
	public void disableGroundAndAirControl(){
		movementEnabled = false;
		airControlEnabled = false;
		groundControlEnabled = false;
	}
	
	public void disableAirControl(){
		airControlEnabled = false;
	}
	
	public void enableAirControl(){
		airControlEnabled = true;
	}
	
	public void disableGroundControl(){
		groundControlEnabled = false;
	}
	
	public void enableGroundControl(){
		groundControlEnabled = true;
	}
	
	public void enableJumping(bool b){
		jumpingEnabled = b;
		JumpState = 0;
	}
	
	public void enableSpringyFeet(bool b){
		springyfeetEnabled = b;
	}
	/// <summary>
	/// Gets the local velocity of this rigidbody;
	/// </summary>
	/// <returns>The local velocity.</returns>
	public Vector3 getLocalVelocity(){
		return this.rigidbody.velocity - V_inherit;
	}
	
	public Vector3 getInputForce(){
		return inputForce;
	}
	
	//++++++++++++ Stat Tracking
	private void updateDistanceTraveled(){
		distanceTravelled += (transform.position - prevPos).magnitude;
		prevPos = transform.position;
	}
	
	public float getDistanceTraveled(){
		return distanceTravelled;
	}
	
	public float getTimeAlive(){
		return Time.time - timeInit;
	}
	
	
}
