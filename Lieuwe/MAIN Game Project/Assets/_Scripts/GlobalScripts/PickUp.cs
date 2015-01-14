using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
		public float rotationSpeed;
		private Vector3 pos;
		public ParticleSystem coinPickup;
		public int PickUpNumber;

		// Use this for initialization
		void Start ()
		{
				transform.Rotate (90, 0, 0);
				pos = transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.Rotate (0, 0, rotationSpeed * Time.deltaTime);
		}

		void OnTriggerEnter (Collider col)
		{
				if (col.tag.Equals ("Player") || col.tag.Equals ("pogoStick")) {
						GameObject Player=col.transform.root.gameObject;
						Destroy (gameObject);
						if (PickUpNumber == 0) {
								Instantiate (coinPickup, pos, Quaternion.identity);
								score.scoreUp (10);
								highScore.pickUpCoin ();
						} else if (PickUpNumber == 1) {
								Instantiate (coinPickup, pos, Quaternion.identity);
								Player.GetComponent<HPmanager>().doDamage(-10);
								highScore.pickUpHealth ();

			} else if (PickUpNumber == 2) {
				Instantiate (coinPickup, pos, Quaternion.identity);
				Player.GetComponent<HPmanager>().doDamage(-10);
				highScore.pickUpAmmo ();
				
			}
				}
		}
}
