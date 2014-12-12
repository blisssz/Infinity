using UnityEngine;
using System.Collections;

/// <summary>
/// Pogo stick, Attach this to a PogoStick GameObject
/// </summary>
public class PogoStick : MonoBehaviour {

	private Transform thisOwner;

	/* legacy params
	// animation clips (legacy)
	public AnimationClip ani_PogoMount;
	public AnimationClip ani_PogoDismount;

	public AnimationClip ani_PogoUse;
	public AnimationClip ani_PogoIdleOff;
	public AnimationClip ani_PogoRunOff;
	private Animation animation;

	public float pogoMountTime = 1.0f;
	public float pogoDismountTime = 1.0f;
	public float pogoUseTime = 1.0f;
	public float pogoIdleTime = 1.0f;
	public float pogoRunTime = 1.0f;
	*/
	private float animationTimer = 0.0f;

//	private GameObject pogo;
	private GameObject pogoMain;	// body that has as child the visual pogo

/*
	private Transform handleRight;
	private Transform handleLeft;
*/	
	private float offsetY = -0.0f;
	private float offsetZ = 0.45f;
	
	public float pogoHeight = 3.5f;
	public float pogoSpring = 15000f;
	public float maxCompression = 0.5f;
	public float pogoJumpForce = 14000f;
	
	
	private float currentRotX = 0.0f;
	private float currentRotZ = 0.0f;
	
	private float rotX = 0.0f;
	private float rotZ = 0.0f;
	
	private float acummulatedRotX = 0.0f;
	private float acummulatedRotZ = 0.0f;
	
	private float stepRotate = 2.0f;
	
	private float maxRotateX = 10f;
	private float maxRotateZ = 10f;
	
	public LayerMask ignoreLayer;
	
	private bool sign_check;
	private bool firstContact = false;
	public float upVelTreshhold = -10f;

	private float velSigned = 0.0f;
	private float ds_old = 0f;
	
	private int sign;
	
	private bool springCompressing = true;

	private enum PogoState {inHand, Mounting, isUsing, Dismounting};
	private PogoState pogoState = PogoState.inHand;


	private float clickCD;
	private float clickTimer;

	private bool lockAnimation = false; // can animations switch mid animation?



	// animator mecanim
	private Animator anim;

	public float defaultSpeed = 1f;
	public float mountSpeed = 3f;
	public float dismountSpeed = 3f;

	private float animSpeedParam = 0.0f;	// param that controlls the "Speed" tag in the animator states


	private int mountHash;
	private int dismountHash;


	// Use this for initialization
	void Start () {
		thisOwner = this.transform.root;

		Vector3 offset = thisOwner.position + thisOwner.up * offsetY + thisOwner.forward * offsetZ;
		this.transform.parent = thisOwner;
		// find Animation Component
		foreach (Transform t in transform.GetComponentsInChildren<Transform>()){
			
			if (t.name.Equals("PogoRotatorZ")){
				pogoMain = t.gameObject;
			}

		/*
			if (t.name.Equals("HandLeft")){
				handleLeft = t;
			}
			
			if (t.name.Equals("HandRight")){
				handleRight = t;
			}
		*/
			// animation component of hands and pogo
			if (t.name.Equals("FPhandsWithPogo") && t.GetComponent<Animation>() != null){

				// new anims
				anim = t.GetComponent<Animator>();
				mountHash = Animator.StringToHash("Base Layer.Mount");
				dismountHash = Animator.StringToHash("Base Layer.Dismount");

				/*## legacy
				animation = (t.GetComponent<Animation>());

				ani_PogoIdleOff.wrapMode = WrapMode.Loop;
				ani_PogoMount.wrapMode = WrapMode.Once;
				ani_PogoDismount.wrapMode = WrapMode.Once;
				ani_PogoUse.wrapMode = WrapMode.Loop;
				ani_PogoRunOff.wrapMode = WrapMode.Loop;

				animation[ani_PogoRunOff.name].layer = 0;
				animation[ani_PogoIdleOff.name].layer = 0;

				animation[ani_PogoMount.name].layer = 0;
				animation[ani_PogoDismount.name].layer = 0;
				animation[ani_PogoUse.name].layer = 0;

*/

			}


		}

	
	}
	
	// Update is called once per frame
	void Update () {

		bool leftMouseTap = KeyManager.leftMouse == 1;
	//	bool leftMouse = KeyManager.leftMouse == 2;
	//	bool rightMouse = KeyManager.rightMouse == 2;

		if (leftMouseTap && lockAnimation == false){
			if (pogoState == PogoState.inHand){
				pogoState = PogoState.Mounting;
				lockAnimation = true;
				anim.SetTrigger("Mounting");

				//anim.speed =2f;
				//anim.GetCurrentAnimationClipState(0)[0].clip.frameRate = 1000f;
			}
			else if (pogoState == PogoState.Mounting){
				pogoState = PogoState.isUsing;
				anim.SetTrigger("Mounting");
			}
			else if (pogoState == PogoState.isUsing){
				pogoState = PogoState.Dismounting;
				lockAnimation = true;
				anim.SetTrigger("Mounting");
			}
		}

		// handle state changes of PogoState
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

		if (stateInfo.nameHash == mountHash && stateInfo.normalizedTime > 0.9f){
			pogoState = PogoState.isUsing;
			thisOwner.GetComponent<Movement1>().disableGroundControl();
			lockAnimation = false;
		}
		else if (stateInfo.nameHash == dismountHash && stateInfo.normalizedTime > 0.9f){
			pogoState = PogoState.inHand;
			thisOwner.GetComponent<Movement1>().enableGroundControl();
			lockAnimation = false;
		}

		if( anim.GetCurrentAnimatorStateInfo(0).IsName("Mount")){
			anim.speed = mountSpeed;
		}
		else if( anim.GetCurrentAnimatorStateInfo(0).IsName("Dismount")){
			anim.speed = dismountSpeed;
		}
		else{
			anim.speed = defaultSpeed;
		}
		Vector3 localV = thisOwner.GetComponent<Movement1>().getLocalVelocity();
		Vector3 thisOwnerUp = thisOwner.GetComponent<Movement1>().transform.up;

		Vector3 velInUpDir = thisOwnerUp * Vector3.Dot (localV, thisOwnerUp);

		// xz speed
		//print (thisOwner.GetComponent<Movement1>().MovementState == 1);
		if (thisOwner.GetComponent<Movement1>().MovementState == 1){
			animSpeedParam *= 0.9f;
			anim.SetFloat("Speed", animSpeedParam);
		}
		else{
			animSpeedParam = (localV-velInUpDir).magnitude;
			anim.SetFloat("Speed", animSpeedParam);
		}




/*- Old anims Legacy
		if (pogoState == PogoState.Mounting){
			playPogoAnimation(ani_PogoMount, pogoMountTime, 0.1f, true, PogoState.isUsing);

		}
		else if (pogoState == PogoState.isUsing){
			playPogoAnimation(ani_PogoUse, pogoUseTime, 0.1f, false);
			// force player in air movement state
			thisOwner.GetComponent<Movement1>().disableGroundControl();
		}
		else if (pogoState == PogoState.Dismounting){
			playPogoAnimation(ani_PogoDismount, pogoDismountTime, 0.1f, true, PogoState.inHand);

		}
		else if (pogoState == PogoState.inHand){

			thisOwner.GetComponent<Movement1>().enableGroundControl();

			Vector3 localV = thisOwner.GetComponent<Movement1>().getLocalVelocity();
			if (localV.magnitude > 1.0f){
				playPogoAnimation(ani_PogoRunOff, pogoRunTime, 0.25f, false);
			}
			else{
				playPogoAnimation(ani_PogoIdleOff, pogoIdleTime, 0.25f, false);
			}
		}
Old anims -*/


	
	}
	// handle pogo physics
	void FixedUpdate(){
		if (pogoState == PogoState.isUsing){
			RaycastHit hit;
			
			float c_damp = 0.0f;
			Vector3 force = new Vector3();
			

			
			
			
			// pogo controls
			bool forward = KeyManager.forward == 2;
			bool backward = KeyManager.backward == 2;
			bool left = KeyManager.left == 2;
			bool right = KeyManager.right == 2;
			
			if ((forward && !backward) && !(left || right) ){
				rotX = 1;
				rotZ = 0;
			}
			else if ((!forward && backward) && !(left || right) ) {
				rotX = -1;
				rotZ = 0;
			} 
			else if (!(forward || backward) && (left && !right)) {
				rotX = 0;
				rotZ = 1;
			}
			else if (!(forward || backward) && (!left && right)) {
				rotX = 0;
				rotZ = -1;
			}
			else if ((forward && left) && !(backward || right)){
				rotX = 1;
				rotZ = 1;
			}
			else if ((forward && right) && !(backward || left)){
				rotX = 1;
				rotZ = -1;
			}
			else if ((backward && left) && !(forward || right)){
				rotX = -1;
				rotZ = 1;
			}
			else if ((backward && right) && !(forward || left)){
				rotX = -1;
				rotZ = -1;
			}
			else {
				rotX = 0;
				rotZ = 0;
			}
			

			// Control Rotation pivots
			currentRotX = stepRotate * rotX;
			currentRotZ = stepRotate * rotZ;
			
			if (rotX == 0){
				currentRotX = -acummulatedRotX/10f;
			}
			if (rotZ == 0){
				currentRotZ = -acummulatedRotZ/10f;
			}
			
			if ((acummulatedRotX + currentRotX) > maxRotateX){
				currentRotX = maxRotateX - acummulatedRotX;
			}
			else if ((acummulatedRotX + currentRotX) < -maxRotateX){
				currentRotX = -maxRotateX - acummulatedRotX;
			}
			acummulatedRotX += currentRotX;
			
			if ((acummulatedRotZ + currentRotZ) > maxRotateZ){
				currentRotZ = maxRotateX - acummulatedRotZ;
			}
			else if ((acummulatedRotZ + currentRotZ) < -maxRotateZ){
				currentRotZ = -maxRotateZ - acummulatedRotZ;
			}
			
			acummulatedRotZ += currentRotZ;
			
			
			this.transform.Rotate(new Vector3(currentRotX, 0, 0));
			pogoMain.transform.Rotate (new Vector3(0, 0, currentRotZ) );

			
			//handleLeft.Rotate(new Vector3(-currentRotX*0.5f, 0, 0));
			//handleRight.Rotate(new Vector3(-currentRotX*0.5f, 0, 0));
			
			Vector3 offset = thisOwner.transform.position + thisOwner.transform.up * offsetY + thisOwner.transform.forward * offsetZ;

			// cast ray from player's main body center.
			if (Physics.Raycast(thisOwner.transform.position, -this.transform.up, out hit, pogoHeight, ~ignoreLayer)){
				// bounce
				
				Vector3 velocity = this.transform.root.rigidbody.velocity;
				Vector3 velocityDir = velocity.normalized;
				
				if (firstContact == false){
					// let the hitted object know that he was hit
					noticeHitObjectGotHit(hit.transform.gameObject);
					// can only compress with a high enough velocity in up dir;
					velSigned = Vector3.Dot (velocity, this.transform.up);
				}
				float ds = Mathf.Min(pogoHeight - hit.distance, maxCompression);
				
				if (velSigned > upVelTreshhold){
					// clamp ds some more, helpfull for no sudden change in ds when velocity downward is low
					ds = Mathf.Min(ds, maxCompression* velSigned/upVelTreshhold + maxCompression*0.21f);

				}
				
				if ((ds - ds_old) < 0f){
					springCompressing = false;
				}
				else{
					springCompressing = true;
					// play compressing sound 
				}
				
				// offset pogo by factor ds for jump effect so you know when to apply force to increase junp or decrease.
				this.transform.position = offset + this.transform.up * ds *0.2f;

				ds_old = ds;
				
				if (KeyManager.jump == 2){
					c_damp *= 0.2f;
					force = this.transform.up * pogoJumpForce * ds/maxCompression;
				}
				
				force += pogoMain.transform.up * (ds * pogoSpring);// - c_damp * Vector3.Dot(thisOwner.rigidbody.velocity, this.transform.up));
				
				thisOwner.rigidbody.AddForce(force * Time.deltaTime, ForceMode.Impulse);
				
				firstContact = true;
			}
			else{
				this.transform.position = offset;
				firstContact = false;
			}
		}
	}

	private void noticeHitObjectGotHit(GameObject hitObj){
		// do something
	}


	private void pogoAnimationTimer(float maxTime, PogoState toState){		
		animationTimer += Time.deltaTime;
		if (animationTimer >= maxTime){
			pogoState = toState;
			animationTimer = 0f;
			lockAnimation = false;
		}
	}

	private void playPogoAnimation(AnimationClip currentClip, float playTime,
	                                float fadeTime, bool lockAnim, PogoState toState){

		lockAnimation = lockAnim;
		animation[currentClip.name].speed = currentClip.length/playTime;
		animation.CrossFade(currentClip.name, fadeTime);
		pogoAnimationTimer(playTime, toState);

	}

	/// <summary>
	/// Loops pogo animations.
	/// </summary>
	/// <param name="currentClip">Current clip.</param>
	/// <param name="playTime">Play time.</param>
	/// <param name="fadeTime">Fade time.</param>
	private void playPogoAnimation(AnimationClip currentClip, float playTime,
	                               float fadeTime, bool lockAnim){

		lockAnimation = lockAnim;
		animation[currentClip.name].speed = currentClip.length/playTime;
		animation.CrossFade(currentClip.name, fadeTime);
		animationTimer = 0f; // force timer to 0;
		
	}

}
