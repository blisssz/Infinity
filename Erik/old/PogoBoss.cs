using UnityEngine;
using System.Collections;


// TODO
// - Make eye explosions and destroyed eye mesh done for normal eyes

// - tweak pogo stick jump height

public class PogoBossEye{
	
	public float hp = 100f;
	public HPmanager eyeHP{get; set;}
	
	protected Transform eyeOB;	
	public bool eyeAlive{get; set;}

	public GameObject deadEyeOb;
	public GameObject deadEffect;

	public SoundObjectSpawner soundSpawner{get; set;}

	public PogoBossEye(){
		eyeAlive = true;
	}

	public void setEyeOB(Transform eye){
		eyeOB = eye;
	}

	public void eyeStatus(){
		if (eyeAlive && eyeHP.getHP() <= 0){
			deadEye ();
			eyeAlive = false;
		}
	}
	
	
	public void deadEye(){
		// replace mesh + make it inactive
		if (deadEyeOb != null){
			GameObject deadOB = GameObject.Instantiate(deadEyeOb, eyeOB.position, eyeOB.rotation) as GameObject;
			deadOB.transform.parent = eyeOB.parent;
			soundSpawner.InstantiateSound(eyeOB.position, "Hurt1");
		}
		if (deadEffect != null){
			GameObject deathFX = GameObject.Instantiate(deadEffect, eyeOB.position, eyeOB.rotation) as GameObject;
			deathFX.transform.parent = eyeOB.parent;
			GameObject.Destroy(deathFX, 10f);
		}

		GameObject.Destroy(eyeOB.gameObject);
		
	}
}


[System.Serializable]
public class EyeNormal : PogoBossEye{

	//public float hp = 100f;
	//public HPmanager eyeHP{get; set;}
	//private Transform eyeOB;	
	//public bool eyeAlive{get; set;}

	public float openT1 = 8f;
	public float openT2 = 12f;

	public float closeT1 = 3f;
	public float closeT2 = 5f;

	private float ocTimer = 0f;
	private float closeTime = 0f;
	private float openTime = 0f;

	public bool eyeOpen = false;

	public float lookTime = 0.5f;
	private float lookTimer = 0f;

	public float targetAngle = 25f;
	public float laserCutAccuracy = 5f;
		
	public GameObject laser;
	private GameObject laserOB;

	public float laserDMG = 0f;

	public float chargeTime = 2f;
	public float fireTime = 1f;
	private float fireTimer = 0f;
	private bool firing = false;

	private float rad;

	private Vector3 lookDir;
	private Vector3 prevLookDir;	
	private Quaternion prevLookRot;
	private float interpTime = 0.1f;
	private float interpFac = 0f;


	private Vector3 offsetFrom;
	private Vector3 offsetTo;


	private bool targetLocked = false;



	public EyeNormal(){
		eyeAlive = true;
	}


	//public void setEyeOB(Transform eye){
	//	eyeOB = eye;
	//}

	private void setGodModeOnClose(){
		eyeHP.dmgModifier = (eyeOpen) ? 1f : 0f;
	}

	public bool forceEyeClose(){
		eyeOpen = false;
		ocTimer = 0f;
		return eyeOpen;
	}

	public bool eyeOpenClose(){

		if (eyeAlive){

			float rng = Random.Range (0f, 1f);
			// eye can open
			if (eyeOpen == false && ocTimer > closeTime){
				openTime = rng * (openT2 - openT1) + openT1;
				eyeOpen = true;
				ocTimer = 0f;
				setGodModeOnClose();
			}
			if (eyeOpen == true &&  ocTimer > openTime){
				closeTime = rng * (closeT2 - closeT1) + closeT1;
				eyeOpen = false;
				ocTimer = 0f;
				setGodModeOnClose();
			}
		}
		ocTimer += Time.deltaTime;

		return eyeOpen;
	}


	public void eyeFindTarget(Transform target){


		if (eyeOpen && eyeAlive){
			if (lookTimer == 0f){
				rad = Random.Range (0f, 1f) * 2f * Mathf.PI;
				float UP = Random.Range (0.4f, 1f);//Mathf.Sin (Random.Range (0f, 1f) *0.5f* Mathf.PI);

				lookDir = (new Vector3(Mathf.Cos (rad)* (1-UP), UP , Mathf.Sin(rad)*(1-UP) ) ).normalized;

				lookTimer += Time.deltaTime;
			}
			else{
				lookTimer += Time.deltaTime;
			}

			if (lookTimer > lookTime){
				lookTimer = 0f;
				prevLookDir = lookDir;
				prevLookRot = Quaternion.LookRotation(lookDir);
			}

			if (target != null){

				if (targetLocked == false){

					RaycastHit rayhit;
					Vector3 targetDir = (target.position - eyeOB.position).normalized;
					Physics.Raycast(eyeOB.position, targetDir, out rayhit, 10000f);

					float angle = Mathf.Acos (Vector3.Dot (targetDir, eyeOB.forward));

					if (angle < (targetAngle/180f * Mathf.PI) && rayhit.transform.tag.Equals("Player") ){
						//Debug.Log(angle);

						eyeOB.LookAt (target.position);
						targetLocked = true;

					}

					else{
						interpFac = Mathf.Min(lookTimer/interpTime, 1.0f);
						eyeOB.rotation = Quaternion.Lerp(prevLookRot, Quaternion.LookRotation(lookDir), interpFac);
					}
				}
				else{
					// target locked
					eyeOB.LookAt (target.position);
					chargeAndFire(target.position);
				}

			}

		}
		// if !eyeOpen
		else{

			targetLocked = false;
			firing = false;
			fireTimer = 0f;
			if (laserOB != null){
				GameObject.Destroy(laserOB);
				eyeOB.renderer.material.SetFloat("_fac", 0);
			}
		}
	}


	public void chargeAndFire(Vector3 targetPos){

		bool rdy2Fire = (ocTimer + chargeTime + fireTime) <= openTime;
		if (rdy2Fire){
			if (fireTimer < chargeTime){
				// charge effect code here
				// -shader that transitions color to red when fully charges for example;
				eyeOB.renderer.material.SetFloat("_fac", fireTimer/chargeTime);
			}


			if (fireTimer >= chargeTime && firing == false){
				firing = true;
				laserOB = GameObject.Instantiate(laser, eyeOB.position, eyeOB.rotation) as GameObject;
				laserOB.transform.position = eyeOB.position + eyeOB.forward * eyeOB.GetComponent<SphereCollider>().radius;
				laserOB.transform.parent = eyeOB;

				// assign dmg scale and time to laser
				PogoBossLaser pbl = laserOB.AddComponent<PogoBossLaser>();
				pbl.soundSpawner = soundSpawner;
				pbl.lifetime = fireTime;
				pbl.DMG = laserDMG;

				// create target offsets
				float rad1 = Random.Range (0f, 1f) * 2f * Mathf.PI;
				float rad2 = rad1 + (1f+Random.Range (-0.2f, 0.2f)) * Mathf.PI;

				offsetFrom = laserCutAccuracy *(new Vector3(Mathf.Cos (rad1), 0, Mathf.Sin(rad1) ) );
				offsetTo = laserCutAccuracy * (new Vector3(Mathf.Cos (rad2), 0, Mathf.Sin(rad2) ) );

			}
		}
		else{
			eyeOB.renderer.material.SetFloat("_fac", 0);
		}

		if (fireTimer >= (chargeTime + fireTime) && firing){
			firing = false;
			fireTimer = 0f;
			eyeOB.renderer.material.SetFloat("_fac", 0);
			GameObject.Destroy(laserOB);
		}

		if (firing == true){
			// fire at target in a path itercept
			//laserOB.transform.localScale = new Vector3(1, 1, 100);
			float interp = (fireTimer - chargeTime)/fireTime;
			Quaternion q1 = Quaternion.LookRotation ((targetPos + offsetFrom)-eyeOB.position);
			Quaternion q2 = Quaternion.LookRotation ((targetPos + offsetTo)-eyeOB.position);
			laserOB.transform.rotation = Quaternion.Lerp(q1 ,q2 ,interp);
			eyeOB.renderer.material.SetFloat("_fac", 1-(fireTimer - chargeTime)/fireTime);

		}


		fireTimer += Time.deltaTime;

	}

/*	public void eyeStatus(){
		if (eyeAlive && eyeHP.getHP() <= 0){
			deadEye ();
			eyeAlive = false;
		}
	}


	public void deadEye(){
		// replace mesh + make it inactive
		GameObject.Destroy(eyeOB.gameObject);

	}*/

}






[System.Serializable]
public class EyeBig : PogoBossEye{
	public float DMG = 0f;
	//public float hp = 100f;
	//public HPmanager eyeHP{get; set;}
	public float targetAngle = 25f;

	public float laserCutAccuracyMin = 3f;
	public float laserCutAccuracyMax = 8f;

	public float laserSpawnRate = 0.5f;

	public int nLasers = 4;
	public GameObject laserOB;
	public float laserLife = 1f;
	public float laserOffset = 6.5f;
	public float rotateY = 0.1f;

	private float t;

//	public bool eyeAlive = true;

//	private Transform eyeOB;

	public EyeBig(){
		eyeAlive = true;
	}

//	public void setEyeOB(Transform e){
//		eyeOB = e;
//	}


	public void eyeControl(Transform target){

		if (eyeAlive){
			t += Time.deltaTime;		
			eyeOB.Rotate(0f, rotateY, 0f);

			foreach (Transform tr in eyeOB.GetComponentsInChildren<Transform>()){
				tr.Rotate (0, -3f*rotateY, 0.1f);
			}

			eyeStatus(); // checks hp etc

			if ((t % laserSpawnRate) <= Time.deltaTime){
				for (int i = 0; i < nLasers; i++){
					eyeTargets(target);
				}
			}
		}
	}


	public void eyeTargets(Transform target){

		float r1 = Random.Range(0f,1f)-0.5f;
		float r2 = Random.Range(0f,1f)-0.5f;
		float r3 = Random.Range(0f,1f)-0.25f;

		Vector3 direction = (new Vector3(r1,r3,r2)).normalized;

		// raycast
		RaycastHit rayhit;
		Vector3 targetDir = (target.position - eyeOB.position).normalized;
		Physics.Raycast(eyeOB.position, targetDir, out rayhit, 10000f);
		
		float angle = Mathf.Acos (Vector3.Dot (targetDir, direction));
		
		if (angle < (targetAngle/180f * Mathf.PI) && rayhit.transform.tag.Equals("Player") ){
			// spawn laser
			GameObject laser = GameObject.Instantiate(laserOB) as GameObject;
			laser.transform.position = eyeOB.position + direction * laserOffset;
			laser.transform.parent = eyeOB;

			PogoBossLaser pbl = laser.AddComponent<PogoBossLaser>();
			pbl.soundSpawner = soundSpawner;
			pbl.lifetime = laserLife;
			pbl.DMG = DMG;

			// create target offsets
			float rad1 = Random.Range (0f, 1f) * 2f * Mathf.PI;
			float rad2 = rad1 + (1f+Random.Range (-0.2f, 0.2f)) * Mathf.PI;

			float laserCutAccuracy = laserCutAccuracyMin +  Random.Range (0f, 1f) *(laserCutAccuracyMax - laserCutAccuracyMin);

			Vector3 offsetFrom = laserCutAccuracy *(new Vector3(Mathf.Cos (rad1), 0, Mathf.Sin(rad1) ) );
			Vector3 offsetTo = laserCutAccuracy * (new Vector3(Mathf.Cos (rad2), 0, Mathf.Sin(rad2) ) );

			// assign to laser
			pbl.LERP = true;
			pbl.setLerpTarget(target, offsetFrom, offsetTo);

//			PogoBossLaser pbL

			// set targets of laser
		}
	}

}







/// <summary>
/// Pogo boss.
/// </summary>
public class PogoBoss : MonoBehaviour {

	// ideas
	// - make every eye unique in the way they act
	// - eyeX stays open between a range a to b seconds, and closed between c to d;
	// - eyeX charges laser fast and others targets player faster.
	// etc
	public EyeNormal[] eyeParams = new EyeNormal[4];		// index 0 == eye1 ... 3 == eye4
	public EyeBig eyeBigParam;
	public bool finished=false;


	public bool setEyeHpAsMaxVelTreshold = false;

	private GameObject player;

	private string[] eyeNames = new string[]{"Eye1", "Eye2", "Eye3", "Eye4"};

	private Transform[] eyeTransfArray;

	// requires some good tweaking, note that dmg == vel.magnitude
	public bool dmgOnVelHitsOnly = true;			// only takes hits from "velocity hits"
	public float maxVelHit = 30f;					// [m/s]
	public float minVelHit = 20f;					// [m/s]

	public float eyeStatFac = 0.7f;



	private Animator anim;

	private enum BossState {FourEyes, FinalEye, BossDefeated};
	private BossState bossState;

	private int[] eyeAliveState = new int[] {0, 0, 0, 0, 0}; // 0 == alive 1 == just died, 2 == dead 

	private int finalStateHash = Animator.StringToHash("BigEye.BigEyeIdle");

	private bool finalSound = false;

	// sound stuff
	public SoundObjectSpawner soundSpawner;


	// Use this for initialization
	void Start () {

		if (eyeParams.Length != 4){
			Debug.LogWarning("EyeParams need to have 4 entries!");
		}

		bossState = BossState.FourEyes;

		eyeTransfArray = new Transform[5];

		if (GetComponent<SoundObjectSpawner>()){
			soundSpawner = GetComponent<SoundObjectSpawner>();
		}
		else{
			Debug.Log (this.gameObject.name + " has no SoundObjectSpawner Component");
		}

		// initiate eye HPs
		foreach (Transform t in GetComponentsInChildren<Transform>()){

			// find 4 normal eyes;
			for (int i = 0; i < 4; i++){
				if (t.name.Equals("PogoBossEyeNormal" + (i+1) )){
					eyeParams[i].eyeHP = t.gameObject.AddComponent<HPmanager>();
					eyeParams[i].eyeHP.setHP(eyeParams[i].hp);
					eyeParams[i].eyeHP.velBasedDmg = dmgOnVelHitsOnly;
					eyeParams[i].eyeHP.setMinMaxVel(minVelHit, maxVelHit);
					eyeParams[i].setEyeOB(t);
					eyeParams[i].soundSpawner = soundSpawner;
					eyeTransfArray[i] = t;				

					if (setEyeHpAsMaxVelTreshold){
						eyeParams[i].eyeHP.setHP(maxVelHit);
					}

				}

			}
			if (t.name.Equals("PogoBossEyeBig")){
				eyeBigParam.eyeHP = t.gameObject.AddComponent<HPmanager>();
				eyeBigParam.eyeHP.setHP(eyeBigParam.hp);
				eyeBigParam.eyeHP.velBasedDmg = dmgOnVelHitsOnly;
				eyeBigParam.eyeHP.setMinMaxVel(minVelHit, maxVelHit);
				eyeBigParam.setEyeOB(t);
				eyeBigParam.soundSpawner = soundSpawner;

				eyeBigParam.eyeHP.dmgModifier = 0f; // start in godmode

				if (setEyeHpAsMaxVelTreshold){
					eyeBigParam.eyeHP.setHP(maxVelHit);
				}
			}
		}

		anim = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
		// check if player has a pogostick
		checkPlayerPogo();

		controlEyes();
	}

	private void eyeBlink(ref bool eyeNum, string stateNm){
		if (eyeNum == false){
			anim.SetBool(stateNm, true);
			eyeNum = true;
		}
		else{
			anim.SetBool(stateNm, false);
			eyeNum = false;
		}
	}


	private void openCloseEye( bool eyeState, string stateNm){
		anim.SetBool(stateNm, eyeState);
	}




	private void controlEyes(){

		//GameObject player = GameObject.FindGameObjectWithTag ("Player") as GameObject;
		//print (player.rigidbody.velocity.magnitude);

		bool fourEyesAlive = false;

		for (int i = 0; i < 4; i++){

			// play animation
			bool eyeOpen = eyeParams[i].eyeOpenClose();
			openCloseEye(eyeOpen, eyeNames[i]);

			if (eyeParams[i].eyeAlive && eyeParams[i].eyeHP.getHP() <= 0){
				increaseEyeStats(eyeStatFac);
				anim.SetTrigger("Hit");	
				// force other eyes in closed
				for (int j = 0; j < 4; j++){
					if (i != j){
						openCloseEye(eyeParams[j].forceEyeClose(), eyeNames[j]);
					}
				}
			}

			eyeParams[i].eyeStatus();




			//eyeTransfArray[i].LookAt(player.transform.position);
			if (player != null){

				eyeParams[i].eyeFindTarget( player.transform); //eyeTransfArray[i],
				if (i == 0){
					fourEyesAlive = eyeParams[0].eyeAlive;
				}
				else{
					fourEyesAlive = fourEyesAlive || eyeParams[i].eyeAlive;
				}
			}
		}

		if (!fourEyesAlive){

			if (bossState ==BossState.FourEyes){
				// sound of last spawn
				soundSpawner.InstantiateSound(this.transform.position, "Hurt2");
			}

			// spawn the final eye by animation
			bossState = BossState.FinalEye;
			anim.SetBool("EyeBig", true);



			if (anim.GetCurrentAnimatorStateInfo(5).IsTag("FinalState")){ //(finalStateHash == anim.GetCurrentAnimatorStateInfo(5).nameHash){
				// final eye control
				if (player != null){
					eyeBigParam.eyeControl(player.transform);
				}
				eyeBigParam.eyeHP.dmgModifier = 1f; 			// remove from godmode

				// boss defeated?
				if (eyeBigParam.eyeAlive == false){

					if (bossState != BossState.BossDefeated){
						// play sound
						if (finalSound == false){
							soundSpawner.InstantiateSound(transform.position, "Dead");
							finalSound = true;
						}
					}

					bossState = BossState.BossDefeated;

				}
			}
			else{
				// activate the big eye effect
				this.GetComponentInChildren<BigEyeFX>().activateFX = true;

			}

		}


		if (bossState == BossState.BossDefeated){
			defeated (15f);
		}



	}

	private void eyeFindTarget(GameObject player, float t){

		float rad = Random.Range (0f, 1f) * 2f * Mathf.PI;
		Vector3 lookDir = new Vector3(Mathf.Cos (rad),0 , Mathf.Sin(rad));

		if (player != null){

		}
	}

	private void increaseEyeStats(float fac){
		for (int i = 0; i < 4; i++){
			//eyeParams[i].fireTime *= fac;
			eyeParams[i].chargeTime *= fac;
		}
	}

	private void defeated(float t){
		// death anim here
		this.transform.Translate(-this.transform.up*Time.deltaTime);

		// finally:
		Destroy(this.gameObject, t);
		if(finished==false){
			GameObject Finish=ObjectSpawner.SpawnObjectWith(this.transform.position + new Vector3(0,5,0),"EndPoint");
			Finish.GetComponent<endPoint>().isBossLevel=true;
			this.finished=true;
		}
	}
	

	private void checkPlayerPogo(){
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;

		if (player != null){

			// if no pogo:
			if (player.GetComponent<PlayerManager>().getCurrentWeapon() == null || PlayerManager.useWeaponID != 1){
				if (!player.GetComponent<HitsByVelocity>()){
					player.AddComponent<HitsByVelocity>();
				}
			}
			else{
				if (player.GetComponent<HitsByVelocity>()){
					Destroy(player.GetComponent<HitsByVelocity>());
					print ("destroy component");
				}
			}
		}
	}



}
