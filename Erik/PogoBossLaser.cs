using UnityEngine;
using System.Collections;

public class PogoBossLaser : MonoBehaviour {


	public float DMG = 0f;
	private bool hasHit = false;
	public float lifetime = 1f;


	public float xyScale = 1f;
	private float t = 0f;

	public bool LERP = false;
	private Vector3 fromOffset;
	private Vector3 toOffset;
	public Transform target;

	public GameObject SoundGameObject;
	public SoundObjectSpawner soundSpawner;

	void Update () {

		RaycastHit hit;

		float d = 0f;

		// simple quadratic function for scale from 0 to 1 to 0;
		float xyS = xyScale * (-1f/(0.25f*lifetime*lifetime) *Mathf.Pow(t - 0.5f*lifetime, 2f) + 1f);
		t += Time.deltaTime;

		if (Physics.Raycast(transform.position, transform.forward, out hit, 10000f) ){
			d = hit.distance;
			transform.localScale = new Vector3(xyS, xyS, d);
			if (!hasHit && hit.transform.tag == "Player"){
				hasHit = true;
				hit.transform.GetComponent<HPmanager>().doDamage (DMG);
				//Debug.Log ("Player got Hit");
			}
		}
		else{
			transform.localScale = new Vector3(xyS, xyS, 10000f);
		}

		if( Camera.main != null){


			float d2 =  Vector3.Dot ((Camera.main.transform.position - transform.position), transform.forward);

			if (d2 < d){

				d = d2;
			}
			Vector3 pos = this.transform.position + this.transform.forward * d;
			playSound(pos);
		}


		if (LERP == true && target != null){

			lerp2Target();
		}

		if (t > lifetime){
			Destroy (this.gameObject);
		}

	
	}

	public void setLerpTarget(Transform tr, Vector3 from, Vector3 to){
		target = tr;
		fromOffset = from;
		toOffset = to;
	}

	private void playSound(Vector3 pos){
		if (Time.frameCount % 10 == 0){
			soundSpawner.InstantiateSound(pos, "Laser");

		//	if (GetComponent<SoundObjectSpawner>()){
				//GetComponent<SoundObjectSpawner>().InstantiateRandomSound(pos, Quaternion.identity);//InstantiateSound(pos, Quaternion.identity);

		//		GetComponent<SoundObjectSpawner>().InstantiateSound(pos, "tag");
		//	}

			//Instantiate(SoundGameObject, transform.position, Quaternion.identity);
		}
	}

	private void lerp2Target(){
		float interp = (t/lifetime);
		Quaternion q1 = Quaternion.LookRotation ((target.position + fromOffset)-this.transform.position);
		Quaternion q2 = Quaternion.LookRotation ((target.position + toOffset)-this.transform.position);
		this.transform.rotation = Quaternion.Lerp(q1 ,q2 ,interp);
	}
}
