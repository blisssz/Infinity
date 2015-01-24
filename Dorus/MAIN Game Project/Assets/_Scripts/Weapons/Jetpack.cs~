using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour {

	private GameObject player;
	private static GameObject sliderFill;
	private Movement1 movementController;
	private bool playerAirborne;
	private static bool jetpackActive = false;

	private float airControlSpeed = 1000;
	private float thrustSpeed = 1000;
	private static int maxFuel = 200;
	private static int fuelLeft = maxFuel;
	private AudioSource Sound;
	public float minVolume;
	public float maxVolume;
	private bool Volume;

	private bool spaceHold;

	public static int FuelLeft {

		get { 
			return fuelLeft; 
		}
		set {
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Fuel");}

			fuelLeft = value;
			if(sliderFill!=null){
			sliderFill.GetComponent<SlidingBar> ().setValueJetpack (fuelLeft, maxFuel, true);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		movementController = player.GetComponent<Movement1> ();
		sliderFill = GameObject.FindWithTag ("Fuel");
		Sound=this.GetComponent<AudioSource>();
		if(Volume){
			Sound.volume=minVolume;
			Volume=false;
		}
	}

	void Update () {
		bool key2 = KeyManager.key2 == 1; // tap key once
		spaceHold = KeyManager.jump == 2;

		if (key2 == true) {
			jetpackActive = !jetpackActive;
			JetpackActivation.instance.StopAllCoroutines();
			JetpackActivation.instance.StartCoroutine ("Fade", jetpackActive);
		}
		if (PauseMenu.paused == true){
			Sound.volume=minVolume;
		}else if (spaceHold == true && jetpackActive == true && player != null && FuelLeft > 0){
			Sound.volume=maxVolume;
		}
	}

	void FixedUpdate () {
		if ((movementController.MovementState == 0 || movementController.MovementState == 2) && FuelLeft < maxFuel && jetpackActive == true){
			FuelLeft++;
		}

		if(jetpackActive == true && player != null && FuelLeft > 0){

			//Change this control later to the spacebar?
			spaceHold = KeyManager.jump == 2;
			bool forward = KeyManager.forward == 2;
			bool backward = KeyManager.backward == 2;
			bool left = KeyManager.left == 2;
			bool right = KeyManager.right == 2;
			bool[] directions = {forward, backward, left, right};
			int trueCount = 0;
			foreach (bool direction in directions){
				if (direction == true){
					trueCount++;
				}
			}
			float multiplier = 1f - (0.25f * trueCount);
			float thrustMultiplier = 0.75f;
			if(trueCount == 0){
				thrustMultiplier = 1f;
			}

			if (spaceHold) {
				if(!Volume){
					Sound.volume=maxVolume;
					Volume=true;
				}
				player.rigidbody.AddForce(0,thrustSpeed * thrustMultiplier,0);

				if (forward == true){
					player.rigidbody.AddForce (player.transform.forward * airControlSpeed * multiplier);
				}
				
				if (backward == true){
					player.rigidbody.AddForce (-player.transform.forward * airControlSpeed * multiplier);
				}
				
				if (left == true){
					player.rigidbody.AddForce (-player.transform.right * airControlSpeed * multiplier);
				}
				
				if (right == true){
					player.rigidbody.AddForce (player.transform.right * airControlSpeed * multiplier);
				}

				FuelLeft--;
			}
			if (!spaceHold) {
				if(Volume){
					Sound.volume=minVolume;
					Volume=false;
				}
			}
		}
		else{
			//Player is null
			if(Volume){
				Sound.volume=minVolume;
				Volume=false;
			}
			if(player==null){
			player = GameObject.FindWithTag ("Player");
			}
		}


	}

	public static void reset(){
		FuelLeft=maxFuel;
		jetpackActive = true;
		if(JetpackActivation.instance!=null){
		JetpackActivation.instance.StopAllCoroutines();
			JetpackActivation.instance.Reset();
		}
	}
}