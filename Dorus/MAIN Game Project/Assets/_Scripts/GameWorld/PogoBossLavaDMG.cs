using UnityEngine;
using System.Collections;

public class PogoBossLavaDMG : MonoBehaviour {

	public float damage = 5f;
	public float tickRate = 0.5f;
	private float timer = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
	
	}


	void OnTriggerStay(Collider other){
		if (other.tag.Equals("Player")){
			if(timer % tickRate <= Time.deltaTime){
				other.gameObject.GetComponent<HPmanager>().doDamage(damage);
			}
		}

	}
}
