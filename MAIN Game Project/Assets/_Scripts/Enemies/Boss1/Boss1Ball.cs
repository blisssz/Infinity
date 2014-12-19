using UnityEngine;
using System.Collections;

public class Boss1Ball : MonoBehaviour {

	public GameObject Boss1Grunt;
	private bool hasPlayedSound = false;
	private int counter = 0;
	private GameObject[] getSoundCount;
	private int soundCount;

	void Update() {
		//zorgt dat het geluid niet te vaak achter elkaar wordt gespeeld
		if (counter > 60) {
			hasPlayedSound = false;
		}
		if (hasPlayedSound == true) {
			counter = counter + 1;	
		} else {
			counter = 0;
		}
		if((transform.position - transform.parent.transform.position).magnitude > 10){
			transform.parent = null;
		}

	}

	//als grappling hook bal raakt, 
	void OnTriggerEnter(Collider col){
		if (col.tag.Equals("grapplingHook")) {
			score.gameScore += 5;
			//maak soundobject 1 x aan.
			if (hasPlayedSound == false) {
				//zorg dat er maar 1 sound tegelijkertijd gespeeld wordt
				getSoundCount = GameObject.FindGameObjectsWithTag("GruntSound");
				soundCount = getSoundCount.Length;
				print (soundCount);
				if (soundCount < 2) {
					GameObject soundbody;
					soundbody = Instantiate(Boss1Grunt, (transform.position - new Vector3(0,1.2f,0)), transform.rotation) as GameObject;
					soundbody.tag = "GruntSound";
					hasPlayedSound = true;
				}
			}
		}
		else { 
			print(col.tag);
		}	
		if (col.tag.Equals("Environment")){
			transform.Rotate (0,180,0);
		}
	}
}
