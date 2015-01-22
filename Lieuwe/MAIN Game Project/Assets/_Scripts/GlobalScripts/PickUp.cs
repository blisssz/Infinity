using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
		public float rotationSpeed;
		public GameObject coinPickup;
		public int PickUpNumber;
		private bool Started;
		private float TimeSinceStarted;

		// Use this for initialization
		void Start ()
		{
				if (PickUpNumber == 2) {
						transform.Rotate (90, 0, 0);
				}
		TimeSinceStarted=Time.timeSinceLevelLoad;
		Started=false;
		}
	
		// Update is called once per frame
		void Update ()
		{

				if (PickUpNumber == 0 || PickUpNumber == 1) {
						transform.Rotate (0, rotationSpeed * Time.deltaTime, 0);
				} else {
						transform.Rotate (0, 0, rotationSpeed * Time.deltaTime);
				}
		}

		void OnTriggerEnter (Collider col)
		{
				
				if((!Started)&&(TimeSinceStarted+5<Time.timeSinceLevelLoad)){
			Started=true;
		}
		if (col.transform.name.Contains("Chunk")&&!Started) {
			Destroy (this.gameObject);
		}
				if (col.tag.Equals ("Player") || col.tag.Equals ("pogoStick")) {
						GameObject Player = col.transform.root.gameObject;
						Destroy (gameObject);
						if (PickUpNumber == 0) {
								Instantiate (coinPickup, this.transform.position, Quaternion.identity);
								score.scoreUp (10);
								highScore.pickUpCoin ();
						} else if (PickUpNumber == 1) {
								Instantiate (coinPickup, this.transform.position, Quaternion.identity);
								Player.GetComponent<HPmanager> ().doDamage (-50);
								//highScore.pickUpHealth ();

						} else if (PickUpNumber == 2) {
								Instantiate (coinPickup, this.transform.position, Quaternion.identity);
								GameObject Weapon = Player.GetComponent<PlayerManager> ().getCurrentWeapon ();
								if (Weapon.GetComponent<Gun> ()) {
										Weapon.GetComponent<Gun> ().addMagazines(2);
								}
								//highScore.pickUpAmmo ();
				
						}
				}


		}
}
