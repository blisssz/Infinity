using UnityEngine;
using System.Collections;

[System.Serializable]
public class EyeParams{

	public float hp = 100f;

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

	private Transform eyeOB;

	public void setEyeOB(Transform eye){
		eyeOB = eye;
	}

	public bool eyeOpenClose(){

		float rng = Random.Range (0f, 1f);
		// eye can open
		if (eyeOpen == false && ocTimer > closeTime){
			openTime = rng * (openT2 - openT1) + openT1;
			eyeOpen = true;
			ocTimer = 0f;
		}
		if (eyeOpen == true &&  ocTimer > openTime){
			closeTime = rng * (closeT2 - closeT1) + closeT1;
			eyeOpen = false;
			ocTimer = 0f;
		}
		
		ocTimer += Time.deltaTime;

		return eyeOpen;
	}


	public void eyeFindTarget(Transform target){

		if (prevLookDir == null){
			prevLookDir = eyeOB.forward;
			prevLookRot = eyeOB.rotation;
		}

		if (eyeOpen){
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
			GameObject.Destroy(laserOB);
		}
	}


	public void chargeAndFire(Vector3 targetPos){

		bool rdy2Fire = (ocTimer + chargeTime + fireTime) <= openTime;

		if (fireTimer < chargeTime){
			// charge effect code here
			// -shader that transitions color to red when fully charges for example;
		}

		if (rdy2Fire){
			if (fireTimer >= chargeTime && firing == false){
				firing = true;
				laserOB = GameObject.Instantiate(laser, eyeOB.position, eyeOB.rotation) as GameObject;
				laserOB.transform.position = eyeOB.position + eyeOB.forward * eyeOB.GetComponent<SphereCollider>().radius;
				laserOB.transform.parent = eyeOB;

				// assign dmg scale and time to laser
				PogoBossLaser pbl = laserOB.AddComponent<PogoBossLaser>();
				pbl.lifetime = fireTime;
				pbl.DMG = laserDMG;

				// create target offsets
				float rad1 = Random.Range (0f, 1f) * 2f * Mathf.PI;
				float rad2 = rad1 + (1f+Random.Range (-0.2f, 0.2f)) * Mathf.PI;

				offsetFrom = 5f*(new Vector3(Mathf.Cos (rad1), 0, Mathf.Sin(rad1) ) );
				offsetTo = 5f*(new Vector3(Mathf.Cos (rad2), 0, Mathf.Sin(rad2) ) );

			}
		}

		if (fireTimer >= (chargeTime + fireTime) && firing){
			firing = false;
			fireTimer = 0f;
			GameObject.Destroy(laserOB);
		}

		if (firing == true){
			// fire at target in a path itercept
			//laserOB.transform.localScale = new Vector3(1, 1, 100);
			float interp = (fireTimer - chargeTime)/fireTime;
			Quaternion q1 = Quaternion.LookRotation ((targetPos + offsetFrom)-eyeOB.position);
			Quaternion q2 = Quaternion.LookRotation ((targetPos + offsetTo)-eyeOB.position);
			laserOB.transform.rotation = Quaternion.Lerp(q1 ,q2 ,interp);

		}


		fireTimer += Time.deltaTime;

	}
}





public class PogoBoss : MonoBehaviour {

	// ideas
	// - make every eye unique in the way they act
	// - eyeX stays open between a range a to b seconds, and closed between c to d;
	// - eyeX charges laser fast and others targets player faster.
	// etc
	public EyeParams[] eyeParams = new EyeParams[4];		// index 0 == eye1 ... 3 == eye4

	private float[] timeArray;
	private float[] openTimeArray;
	private float[] closeTimeArray;

	private float[] lookTimeArray;

	private string[] eyeNames = new string[]{"Eye1", "Eye2", "Eye3", "Eye4"};

	private Transform[] eyeTransfArray;

	public float eyeHP = 200f;

	private bool eye1 = false;
	private bool eye2 = false;
	private bool eye3 = false;
	private bool eye4 = false;


	private HPmanager hpEye1;
	private HPmanager hpEye2;
	private HPmanager hpEye3;
	private HPmanager hpEye4;
	private HPmanager hpEyeBig;

	private Animator anim;

	private enum BossState {FourEyes, FinalEye};
	private BossState bossState;

	// Use this for initialization
	void Start () {

		if (eyeParams.Length != 4){
			Debug.LogWarning("EyeParams need to have 4 entries!");
		}


		timeArray = new float[] {0f, 0f, 0f, 0f};
		openTimeArray = new float[] {0f, 0f, 0f, 0f};
		closeTimeArray = new float[] {0f, 0f, 0f, 0f};
		lookTimeArray = new float[] {0f, 0f, 0f, 0f};

		bossState = BossState.FourEyes;

		eyeTransfArray = new Transform[5];

		// initiate eye HPs
		foreach (Transform t in GetComponentsInChildren<Transform>()){
			if (t.name.Equals("PogoBossEyeNormal1")){
				hpEye1 = t.gameObject.AddComponent<HPmanager>();
				hpEye1.setHP(eyeHP);
				eyeParams[0].setEyeOB(t);
				eyeTransfArray[0] = t;
			}
			else if (t.name.Equals("PogoBossEyeNormal2")){
				hpEye2 = t.gameObject.AddComponent<HPmanager>();
				hpEye2.setHP(eyeHP);
				eyeParams[1].setEyeOB(t);
				eyeTransfArray[1] = t;
			}
			else if (t.name.Equals("PogoBossEyeNormal3")){
				hpEye3 = t.gameObject.AddComponent<HPmanager>();
				hpEye3.setHP(eyeHP);
				eyeParams[2].setEyeOB(t);
				eyeTransfArray[2] = t;
			}
			else if (t.name.Equals("PogoBossEyeNormal4")){
				hpEye4 = t.gameObject.AddComponent<HPmanager>();
				hpEye4.setHP(eyeHP);
				eyeParams[3].setEyeOB(t);
				eyeTransfArray[3] = t;
			}
			else if (t.name.Equals("PogoBossEyeBig")){
				hpEyeBig = t.gameObject.AddComponent<HPmanager>();
				hpEyeBig.setHP(eyeHP);
				eyeTransfArray[4] = t;
			}
		}

		anim = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
		print (hpEye1.getHP());

		int rng = Random.Range(0, 100);

	/*	if (rng == 4){
			eyeBlink(ref eye1, "Eye1");
		}

		if (rng == 9){
			eyeBlink(ref eye2, "Eye2");
		}

		if (rng == 54){
			eyeBlink(ref eye3, "Eye3");
		}

		if (rng == 78){
			eyeBlink(ref eye4, "Eye4");
		}*/

		float openTime = Random.Range (0f, 1f) * (eyeParams[0].openT2 - eyeParams[0].openT1) + eyeParams[0].openT1;
		//float

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

		GameObject player = GameObject.FindGameObjectWithTag ("Player") as GameObject;

		for (int i = 0; i < 4; i++){

			// play animation
			bool eyeOpen = eyeParams[i].eyeOpenClose();
			openCloseEye(eyeOpen, eyeNames[i]);


			//eyeTransfArray[i].LookAt(player.transform.position);

			eyeParams[i].eyeFindTarget( player.transform); //eyeTransfArray[i],


		}
	}

	private void eyeFindTarget(GameObject player, float t){

		float rad = Random.Range (0f, 1f) * 2f * Mathf.PI;
		Vector3 lookDir = new Vector3(Mathf.Cos (rad),0 , Mathf.Sin(rad));

		if (player != null){
		}
	}

	private void aimEye(){
	}

}
