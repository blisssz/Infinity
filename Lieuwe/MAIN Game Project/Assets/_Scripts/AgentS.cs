using UnityEngine;
using System.Collections;

public class AgentS : MonoBehaviour {


	public static float globalFitness = -1f;
	public static Transform globalBestTransform;
	public static Vector3 globalBestPos;
	public const float GLOBAL_FITNESS_RANGE = 2000f;
	public static float lastFrame = Time.frameCount;
	private float globalFitnessStrength = 1f;
	public float globalFitnessFac = 1f;
	public int globalTrackFrames = 30;		// frame count to track globalFitness with strength globalFitnessStrength

	public string tagS = "Agent";	// must be set in the editor else it doesnt work
	private SensorProximity[] sensorArray;
	private GameObject[] friendlyArray;

	private float leftValues;
	private float rightValues;
	private float forwardValues;

	// tweak these two by hand
	public float turnFac = 4f;
	public float forwardFac = 1.1f;
	public float stopDistance = 5f;

	public float maxDistanceUpdate = 500f;
	public float minDistanceUpdate = 100f;

	private float minSensorUpdateSteps = 10;
	private float maxSensorUpdateSteps = 60;

	private float collisionCount = 0;
	private float avgCollisions = 0f;

	public int nSensors = 9;

	float learningRate = 0.1f;
	float forgettingFac = 0.0001f;


	public controlParameters cp;
	private AIMotor aimotor;

	// target track params
	public float targetRotFac = 0.1f;
	public float targetDistance = 20f;
	public float stickyTargetTime = 4f;
	public LayerMask ignoreTargetLayer;
	private float holdTargetTime = 0f;
	private Transform target;				// can be any transform not just the player for instance
	public float distanceToTarget {get; set;}

	public bool isTracking {get; set;}
	public bool targetsMainTarget {get; set;}
	public bool stopped {get; set;}

	// pso params
	// idea - initialize some params to a random value;
	public float psoTurnFac = 3f;
	public float trackFitness{get; set;}
	public float groupFitness{get; set;}
	public float groupLikelyhood{get;set;}
	public float groupChance {get; set;}
	public bool groupInvert = false;		// invert True will repel from its friendlies

	public float groupDistance = 20f;
	public float minGroupDistance = 5f;

	// randomness params
	public float pickDirTreshold = 0.1f;
	public float P_PickDirection = 0.01f;
	public float MaxAngle = 90f;
	private float randomTurnValue = 0f;
	public float RandomTurnTime = 0.2f;
	private float randomTime = 0f;

	// hazard detection
	public bool useHazardDetection;
	public float hazardRange = 4f;
	public float hazardDepth = 6f;
	private Vector3 hazardPosition = new Vector3(10000,10000,10000);
	private float hazardStrength = 1f;
	private float timeofDetect = Time.realtimeSinceStartup;
	private bool detectedLava = false;


	// flying parameters
	public bool flying = false;
	private float updownValue = 0f;	// 0f = nothing - down + up
	private float fflip = 1f;
	private float dUp = 0f;
	private float dDown = 0f;
	private float prevUpOrDown = 1f;

	// floaty parametrers
	public float amplitude = 0f;
	public float frequency = 0f;


	/// <summary>
	/// Decreases the fitness. Can only be called once per frame
	/// </summary>
	private static void decreaseGlobalFitness(){
		if (lastFrame != Time.frameCount){
			globalFitness *= 0.99f;
			lastFrame = Time.frameCount;
		}
	}


	void Start () {

		isTracking = false;
		targetsMainTarget = false;
		distanceToTarget = 0f;

		if (globalBestTransform == null){
			globalBestTransform = this.transform;
		}



		//Debug.Log (globalBestPos + " and fitness " + globalFitness);

		sensorArray = SensorProximity.createSensors(this.transform, nSensors, 180f, 1f, 0.5f, 0.3f);
		aimotor = new AIMotor(this.gameObject, cp);
		this.tag = tagS;

		trackFitness = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// update stuff based on main camera distance
		int updateStep = 20;
		if (Camera.main != null){
			float dfac =  ((Mathf.Max( (this.transform.position - Camera.main.transform.position).magnitude - minDistanceUpdate, 0f)) / (maxDistanceUpdate - minDistanceUpdate));
			updateStep = (int) (dfac * (maxSensorUpdateSteps - minSensorUpdateSteps) + minSensorUpdateSteps);
		}
		bool updateSensors = (Time.frameCount % updateStep) == 0;

		aimotor = new AIMotor(this.gameObject, cp);
		aimotor.floatingEffect(cp.Ldesired, frequency, amplitude);

		// set default target, targetTrack() takes care of the rest
		if (target == null){
			GameObject g = GameObject.FindGameObjectWithTag("Player");
			if (g != null){
				target = g.transform;
			}
		}

		// stop criterium
		float stops = 1f;
		stopped = false;
		if (targetsMainTarget && target != null){
			if ((target.position - this.transform.position).sqrMagnitude < stopDistance * stopDistance){
				stops = 0f;
				stopped = true;
			}
		}

		if (updateSensors){
			leftValues = 0f;
			rightValues = 0f;
			forwardValues = 0f;

			for (int i = 0; i < sensorArray.Length; i++){

				SensorProximity sens = sensorArray[i];

				float[] values = sens.runSensor();

				// left turn values;
				if (sens.angle > 0f){
					leftValues += values[0] * sens.angle/90f;
				}
				// right turn values
				if (sens.angle < 0f){
					rightValues += values[0] * -sens.angle/90f;
				}

				// forward values
				forwardValues += 1 - (values[0] * ( 1f - Mathf.Abs(sens.angle/90f)));

				collisionCount += values[1];

				// learn
				sens.addProxRange(avgCollisions*learningRate);
				sens.forgetProxRange(forgettingFac);

				//Hazard detection
				if (useHazardDetection){
					RaycastHit rayHit;
					
					//60 degrees downward detection
					Vector3 dir = (this.transform.forward + - 2f * this.transform.up).normalized;


					// cast ray at 0.3m in front and 45 degree down (default)
					if (Physics.Raycast(this.transform.position + this.transform.forward*0.3f, dir, out rayHit, 30f, ~ignoreTargetLayer)){
						GameObject hazardObj = rayHit.transform.gameObject;
						Vector3 xzpos = rayHit.point;
						xzpos.y = this.transform.position.y;

						if (hazardObj.GetComponent<Renderer>()){
							// set lava hazard position
							if (detectedLava == false && hazardObj.renderer.sharedMaterial.name.Contains("Lava") ){
								hazardPosition = xzpos;
								detectedLava = true;
								timeofDetect = Time.realtimeSinceStartup;
							}
						}

						// set hazard if gap to deep
						float gapdepth = xzpos.y - rayHit.point.y;

						if (gapdepth > hazardDepth){
							hazardPosition = this.transform.position + this.transform.forward * 3f;
						}
					}
					// no ray hit
					else{
						hazardPosition = this.transform.position + this.transform.forward * 3f;
					}

					if ((Time.realtimeSinceStartup - timeofDetect) > 5f){
						detectedLava = false;
					}
				}


				// if flying, set directions for flying up or down
				if (flying){
					assignFlyingDirection();
				}
			}
			avgCollisions = collisionCount/ ((float) nSensors);
			collisionCount = 0f;
		}


		// set a random direction
		if ( ((rightValues - leftValues)*(rightValues - leftValues)) < pickDirTreshold*pickDirTreshold ){
			if (Random.value < P_PickDirection){
				randomTurnValue = (Random.value * 2 -1) * MaxAngle *0.1f;
				randomTime = 0f;
			}
		}

		randomTime += Time.deltaTime;

		if (randomTime > RandomTurnTime){
			randomTurnValue = 0f;
			randomTime = 0f;
		}

		float[] trackValues = targetTrack(target, targetRotFac, stickyTargetTime);
		float targetRotValue = trackValues[0];
		float targetStrenght = trackValues[1];

		float psoAngle = PSO(groupDistance, minGroupDistance);

		float globalFitnessAngle = (globalBestTransform.GetInstanceID() == this.transform.GetInstanceID()) ? 0f :  getRelativeRotationToInXZ(globalBestPos);

		if (lastFrame % globalTrackFrames == 0){
			globalFitnessStrength = Random.value;
		}
		globalFitnessAngle *= globalFitnessStrength;

		// Movement
		float rotationY = stops * (globalFitnessAngle + psoAngle + 1*randomTurnValue +  (1-targetStrenght)*(rightValues-leftValues) * turnFac)
								+ targetRotValue * targetStrenght;


		// Hazard detection, get HazardStrengtht
		if (useHazardDetection){
			hazardStrength = doHazardDetection(hazardRange);
			float hazardRotation = 0f;

			// always repel from the hazard
			if (hazardStrength != 0){
				hazardRotation = 4f * getRelativeRotationToInXZ(hazardPosition, true);
				//Debug.Log ("repelled by hazard");
			}

			// Hazard always stronger than rotationY, apply to rotationY factored
			rotationY = (1-hazardStrength)*rotationY + hazardRotation;
		}



		transform.Rotate(0f, rotationY, 0f);
		//transform.Rotate(0f, -leftValues * leftFac, 0f);



		aimotor.runAIMotor(forwardValues * forwardFac * stops, !flying);

		if (flying){
			flyingUpForce();
		}


		// fitness decreases over time;
		trackFitness = 0;
		target = null;

		decreaseGlobalFitness();
		//trackFitness -= 0.01f;
		//trackFitness = Mathf.Max(0, trackFitness);
	}






	private bool targetMainTarget(string mainEnemyTag = "Player"){
		GameObject g = GameObject.FindGameObjectWithTag(mainEnemyTag);
		if (g != null){
			Transform t = g.transform;

			float dist = (t.position - this.transform.position).magnitude;
			updateGlobalFitness(dist);
			toFarAwayDestroy(dist);

			if (dist < targetDistance){
				Vector3 dir = (t.position - this.transform.position).normalized;
				RaycastHit rayHit;
				if (Physics.Raycast(this.transform.position, dir, out rayHit, targetDistance, ~ignoreTargetLayer)){
					
					if (rayHit.transform.root.GetInstanceID() == t.GetInstanceID()){
						// set target values
						target = rayHit.transform;
						holdTargetTime = 0f;
						isTracking = true;

						trackFitness = 1 - rayHit.distance / targetDistance;
						targetsMainTarget = true;

						return true;
					}
				}
			}
		}
		targetsMainTarget = false;
		return false;

	}


	/// <summary>
	/// tracks the target.
	/// </summary>
	/// <returns>float[2] {rotation, strength}</returns>
	/// <param name="rotSpeed">Rot speed.</param>
	/// <param name="tag">Tag.</param>
	/// <param name="targetLockTime">Target lock time.</param>
	private float[] targetTrack(Transform t, float rotSpeed, float targetLockTime, float strengthTreshold = 1f){

		float rotAngle = 0f;
		float strength = 0f;

		targetMainTarget();

		//GameObject g = GameObject.FindGameObjectWithTag(tag);

		if (t != null){
			//t = g.transform;

			if (!targetsMainTarget){
				Vector3 dir = (t.position - this.transform.position).normalized;
				RaycastHit rayHit;
				if (Physics.Raycast(this.transform.position, dir, out rayHit, targetDistance, ~ignoreTargetLayer)){

					if (rayHit.transform.root.GetInstanceID() == t.GetInstanceID()){
						// set target values
						target = rayHit.transform;
						distanceToTarget = rayHit.distance;
						holdTargetTime = 0f;
						isTracking = true;
					}
				}
			}

			if (isTracking && target != null){

				// get rotation angle and factor it by rot speed
				rotAngle = rotSpeed * getRelativeRotationToInXZ(t, false);
				strength = 1 - (target.position-this.transform.position).magnitude/(targetDistance * strengthTreshold);

				// hold target until x seconds
				holdTargetTime += Time.deltaTime;
				if (holdTargetTime >= stickyTargetTime){
					target = null;
					isTracking = false;
					holdTargetTime = 0f;
				}
			}
		}
		else{
			target = null;
			isTracking = false;
			holdTargetTime = 0f;
		}
		// convert rotation to degrees, and add the strength to a float[2]
		return new float[] {rotAngle/ (Mathf.PI*2) * 180f, strength};

	}


	/// <summary>
	/// Gets the relative rotation to a position in the XZ plane. In radians
	/// -rot -> left turn, else right turn
	/// </summary>
	/// <returns>The relative rotation to a target in XZ plane.</returns>
	/// <param name="t">T.</param>
	/// <param name="toOtherForward">bool.</param>
	/// <param name="invert">bool.</param>

	private float getRelativeRotationToInXZ(Transform t, bool toOtherForward = false, bool invert = false){
		Vector3 augDir;
		Vector3 interpVec;

		float ival = (invert) ? -1f : 1f;

		if (!toOtherForward){
			Vector3 targetAugPosition = t.position;
			targetAugPosition.y = this.transform.position.y;
			augDir = ival*(targetAugPosition - this.transform.position).normalized;
		}
		else{
			augDir = ival*t.forward;
		}

		interpVec = augDir - this.transform.forward;

		float augDotforward = Vector3.Dot (augDir, this.transform.forward);
		
		float rotAngle = Mathf.Acos (Mathf.Clamp(augDotforward, -1f, 1f));
		
		// determine left or right turn
		if (Vector3.Dot (interpVec.normalized, this.transform.right) < 0f){
			rotAngle *= -1;
		}
		return rotAngle;
	}


	private float getRelativeRotationToInXZ(Vector3 pos, bool invert = false){
		Vector3 augDir;
		Vector3 interpVec;
		
		float ival = (invert) ? -1f : 1f;

		Vector3 targetAugPosition = pos;
		targetAugPosition.y = this.transform.position.y;
		augDir = ival*(targetAugPosition - this.transform.position).normalized;
		
		interpVec = augDir - this.transform.forward;
		
		float augDotforward = Vector3.Dot (augDir, this.transform.forward);
		
		float rotAngle = Mathf.Acos (Mathf.Clamp(augDotforward, -1f, 1f));
		
		// determine left or right turn
		if (Vector3.Dot (interpVec.normalized, this.transform.right) < 0f){
			rotAngle *= -1;
		}
		return rotAngle;
	}



	/// <summary>
	/// Particle Swarom Optimization.
	/// </summary>
	private float PSO(float groupDistance, float minGroupDistance){
		float rotAngle = 0f;
		friendlyArray = GameObject.FindGameObjectsWithTag(this.tagS);

		/*
		 * <-unity bug-> clone object without instantiating while using find with tag, also not showing up in the hierachy. Restarting unity fixes it.
		for (int i = 0; i < friendlyArray.Length; i++){
			print(friendlyArray[i].name + i);
			if (friendlyArray[i].name.Equals("TheZombieRabbit(Clone)")){
				Destroy (friendlyArray[i].gameObject);
			}
		}*/

		PSOguideFriendlies(friendlyArray, groupDistance, minGroupDistance);

		// trackFitness is the fitness of the target it tracks, where 1 is the best and closest and 0 for further away
		if (Random.value < groupChance && trackFitness < 0.1f){
			rotAngle = PSOgrouping(friendlyArray, groupDistance);
			rotAngle *= psoTurnFac;
		}

		return rotAngle;
	}

	private void PSOguideFriendlies(GameObject[] friendlies, float groupDistance, float minGroupDistance){
		//float rotAngle = 0;

		if (targetsMainTarget){
			for (int i = 0; i < friendlies.Length; i++){
				AgentS otherAgent = friendlies[i].GetComponent<AgentS>();

				if (otherAgent != null && !otherAgent.targetsMainTarget){
					float sqrmag = (friendlies[i].transform.position - this.transform.position).sqrMagnitude; 

					if (sqrmag > minGroupDistance*minGroupDistance && sqrmag < groupDistance * groupDistance){
						//rotAngle += getRelativeRotationToInXZ(friendlies[i].transform);


						// not that the fitness is in range(0,1)
						if (otherAgent.trackFitness < trackFitness){
							otherAgent.trackFitness = trackFitness * 0.9f;
							otherAgent.target = this.transform;				// let the friend track me
						}
					}
				}
			}
		}
		//return rotAngle;
	}


	private float PSOgrouping(GameObject[] friendlies, float groupDistance){
		float n = 0;
		float rotAngle = 0;
		for (int i = 0; i < friendlies.Length; i++){
			if ((friendlies[i].transform.position - this.transform.position).sqrMagnitude < groupDistance * groupDistance){
				rotAngle += getRelativeRotationToInXZ(friendlies[i].transform, false, groupInvert);
				n += 1;
			}
		}

		if (n != 0){
			rotAngle /= n;
		}

		return rotAngle;
	}

	/// <summary>
	/// Dos the hazard detection.
	/// </summary>
	/// <returns>The hazard detection strength.</returns>
	/// <param name="hazardDetectDistance">Hazard detect distance.</param>
	private float doHazardDetection(float hazardDetectDistance){

		float str = 0f;

		float distance = (hazardPosition-this.transform.position).magnitude;
		if (distance < hazardDetectDistance){
			str = 1f - (distance/hazardDetectDistance);
		}

		return str;
	}

	private void assignFlyingDirection(){
		rigidbody.useGravity = false;
		RaycastHit ray;
		// ray flips
		if (Physics.Raycast(this.transform.position, this.transform.up * fflip, out ray, 10f, ~ignoreTargetLayer)){
			if (fflip == 1f){
				dUp = fflip * ray.distance;
			}
			else{
				dDown = fflip * ray.distance;
			}
		}
		
		else{
			if (fflip == 1f){
				dUp = fflip * 11f;
			}
			else{
				dDown = fflip * 11f;
			}
		}
		
		fflip *= -1f;
	}


	private void flyingUpForce(){

		float upOrDown = dUp + dDown;

		if (upOrDown < 0){
			upOrDown = -1f;
		}
		else if (upOrDown > 0){
			upOrDown = 1f;
		}
		else{
			upOrDown = prevUpOrDown;
		}

		prevUpOrDown = upOrDown;

		float a = 3;	// [m/s^2]
		float Fup = rigidbody.mass * a * upOrDown;

		rigidbody.AddForce(new Vector3(0,Fup, 0f));


	}



	/// <summary>
	/// Updates the global fitness.
	/// </summary>
	/// <param name="distanceToMain">Distance to main.</param>
	private  void updateGlobalFitness(float distanceToMain){
		float fit = 1f - (distanceToMain/GLOBAL_FITNESS_RANGE);

		if (globalBestTransform == null){
			globalBestTransform = this.transform;
			globalFitness = fit;
			globalBestPos = this.transform.position;
		}

		else if (fit > globalFitness){
			globalFitness = fit;
			globalBestPos = this.transform.position;
		}
	}

	private void toFarAwayDestroy(float dist){
		if (dist > 200f){
			Destroy (this.gameObject);
		}
	}


	public void PrintValues(){
		Debug.Log("leftval -> "+leftValues + " rightval -> "+ rightValues + " forwardVal ->" + forwardValues);
	}
}
