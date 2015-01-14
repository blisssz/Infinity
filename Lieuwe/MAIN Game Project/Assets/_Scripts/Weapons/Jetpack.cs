using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour {

	private GameObject player;
	private GameObject sliderFill;
	private Movement1 movementController;
	private bool playerAirborne;
	private bool jetpackActive = false;

	private float airControlSpeed = 1000;
	private float thrustSpeed = 1000;
	private static int maxFuel = 200;
	private int fuelLeft = maxFuel;

	public int FuelLeft {

		get { 
			return fuelLeft; 
		}
		set {
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Fuel");}
			fuelLeft = value;
			sliderFill.GetComponent<SlidingBar> ().setValueFade (fuelLeft, maxFuel, true);
		}
	}
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		movementController = player.GetComponent<Movement1> ();
		sliderFill = GameObject.FindWithTag ("Fuel");
	}

	void Update () {
		bool key2 = KeyManager.key2 == 1; // tap key once
		
		if (key2 == true) {
			jetpackActive = !jetpackActive;
			JetpackActivation.instance.StopAllCoroutines();
			JetpackActivation.instance.StartCoroutine ("Fade", jetpackActive);
		}
	}

	void FixedUpdate () {
		if ((movementController.MovementState == 0 || movementController.MovementState == 2) && FuelLeft < maxFuel && jetpackActive == true){
			FuelLeft++;
		}

		if(jetpackActive == true && player != null && FuelLeft > 0){
			//Change this control later to the spacebar?
			bool spaceHold = KeyManager.jump == 2;
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
		}
		else{
			//Player is null
			player = GameObject.FindWithTag ("Player");
		}


	}
}