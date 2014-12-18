using UnityEngine;
using System.Collections;

/* Player movement script, physics based.
 * 
 * 
 * Attach Script to a GameObject. This GameObject must have a Rigidbody Component.
 * 
 * Mandatory Scripts this class uses:
 * - KeyManager.cs
 * 
 * This class refers to scripts
 * - VelocityInfo.cs;
 */

public class Movement1 : MonoBehaviour {

	// control values
	public float v_max = 10.0f;
	public float KpBaseVal = 10.0f;
	public float Kp {get; set;}
	public float c_air = 0.5f;
	public float A_obj = 1.0f;

	public float slideControlFac = 0.001f;

	public float playerCenter2Ground_height = 0.6f;

	private Vector3 E_old = new Vector3(0.0f, 0.0f, 0.0f);

	//max walk angle;
	public float maxWalkAngle = 45.0f;
	private float maxWalkSlope;

	// jump stuff
	public float jump_acceleration = 5.0f;	// [m/s^2], when g == 0.0
	public float jump_maxtime = 0.4f;
	private float jump_time = 0.0f;

	private Vector3 V_inherit = new Vector3(0.0f, 0.0f, 0.0f);

	private float vfac = 1.0f;
	public float airControl_fac = 1.0f;

	// States
	private int JumpState = 0;			// 0: on ground, 1: is jumping. 2: is landing
	private int MovementState = 0;		// 0: on ground, 1: in the air, 2: sliding

	int k1 = 0;

	// Use this for initialization
	void Start () {
		maxWalkSlope = Mathf.Cos(maxWalkAngle/180f * Mathf.PI);
		Kp = KpBaseVal * rigidbody.mass;
	}
	

	void FixedUpdate () {

		float mass = rigidbody.mass;
		
		bool shiftKey = KeyManager.leftShift == 2;

		bool jump = KeyManager.jump == 2;

		bool forward = KeyManager.forward == 2;
		bool backward = KeyManager.backward == 2;
		bool left = KeyManager.left == 2;
		bool right = KeyManager.right == 2;

		// ray cast for checking if the player walks on ground;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, 2.0f) ){

			Vector3 center2hitPos = (hit.point - transform.position);
			
			
			vfac = 0.2f;
		

			if (center2hitPos.magnitude < playerCenter2Ground_height && JumpState != 1){
				MovementState = 0;
				JumpState = 0;
				vfac = 1.0f;
				jump_time = 0.0f;

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
		}
		else{
			MovementState = 1;	// not touching ground
		}


		////////// Jumping
		if (jump == true){


			if (JumpState == 0 && MovementState != 1){
				//V_inherit = rigidbody.velocity;
				JumpState = 1;
				MovementState = 1;

			}
			if (JumpState == 1 && jump_time < jump_maxtime){
				jump_time += Time.deltaTime;
				// jump force
				rigidbody.AddForce (transform.up*mass*(jump_acceleration+Physics.gravity.magnitude) *Time.deltaTime, ForceMode.Impulse);
			}
		}
		else if (MovementState == 1){
			JumpState = 2;
		}


		////////// Movement And Control
		/// 
		Vector3 V_ref;
		
		// set V_ref to the corresponding input
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

		Vector3 U = new Vector3(0.0f, 0.0f, 0.0f);

		// Ground Control
		if (MovementState == 0 || MovementState == 2){
			// Vref to world coords
			V_ref = transform.localToWorldMatrix.MultiplyVector (V_ref*vfac) + V_inherit;



			// control Kd value
			float Kd = 2.0f * Mathf.Sqrt (mass*Kp);
			
			// control linear velocity
			Vector3 E = V_ref - rigidbody.velocity;
			U = E* Kp + (E - E_old) * Kd*1.0f;
			U.y = 0.0f;
			
			E_old = E;


			if (MovementState == 2){
				U *= slideControlFac;
			}
		}

		// Air Control
		else if (MovementState == 1){

			// get good results by trial and error, or genetic algorithm ;)
			vfac = airControl_fac;

			U = transform.localToWorldMatrix.MultiplyVector (V_ref/v_max*vfac)*mass - rigidbody.velocity * rigidbody.velocity.magnitude * c_air * A_obj;
		}

		// apply forces
		rigidbody.AddForce (U *Time.deltaTime, ForceMode.Impulse);

	}
}
