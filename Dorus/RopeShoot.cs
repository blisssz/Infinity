using UnityEngine;
using System.Collections;

public class RopeShoot : MonoBehaviour {

	public float ropeShootSpeed = 1;
	public float ropeRetractSpeed = 1;
	public float ropeAdjustSpeed = 1;
	public GameObject rope;

	void Update () {
	
		// Make the rope expand when the grappling hook has been fired
		if (SHOOTING == true && rope.gameObject.transform.localScale.y < distanceBetweenStartAndEndPoint){
			rope.gameObject.transform.localScale.y += RopeShootSpeed * Time.deltaTime;
		}


		// Make the rope retract once the hook has been detached or if the max length has been reached
		if (rope.gameObject.transform.localScale.y > 0 && SHOOTING == false) {
			rope.gameObject.transform.localScale.y -= RopeRetractSpeed * Time.deltaTime;
		}


		// Make the rope expand or retract once hooked
		if (HOOKED == true) {
			//expand button is pressed
			if(expandPressed == true){
				rope.gameObject.transform.localScale.y += RopeAdjustSpeed * Time.deltaTime; //NOTE: still needs a fix if trying to expand through object
			}
			//retract button is pressed
			else if(retractPressed == true && rope.gameObject.transform.localScale.y > 0){
				rope.gameObject.transform.localScale.y -= RopeAdjustSpeed * Time.deltaTime;
			}
		}

		//Change the location of the origin of the rope to a given point in the barrel of the gun
		rope.transform.position = StartPointVector3;

		//Change the x and z rotation of the rope
		rope.transform.LookAt(EndPointVector3);

	}
}