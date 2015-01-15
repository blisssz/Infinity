using UnityEngine;
using System.Collections;

public class checkPoint : MonoBehaviour {
	public Vector3 checkPointPosition;
	public int ID;
	private Vector3 playerPosition;
	private GameObject Diamond;
	private GameObject Particles;
	public GameObject ParticlesNotFound;
	public GameObject ParticlesOff;
	public GameObject ParticlesOn;
	public Material MaterialOff;
	public Material MaterialOn;
	// Use this for initialization
	void Start () {
		checkPointPosition = transform.position;
		ID=checkPointList.AddCheckPoint(this);
		Diamond = transform.FindChild("Checkpoint1").gameObject;
		Particles=Instantiate (ParticlesNotFound,checkPointPosition,Quaternion.identity)as GameObject;
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.tag.Equals ("Player") || col.tag.Equals ("pogoStick")) {
			checkPointList.Activate(ID);
		}
	}

	public void setPosition (Vector3 pos){
		checkPointPosition = pos;
	}

	public void Activate(){
		GameController.spawnLocation = checkPointPosition+new Vector3(0,-2,0);
		Diamond.renderer.material=MaterialOn;
		Destroy (Particles);
		Particles=Instantiate (ParticlesOn,checkPointPosition,Quaternion.identity)as GameObject;
	}

	public void DeActivate(){
		Diamond.renderer.material=MaterialOff;
		Destroy (Particles);
		Particles=Instantiate (ParticlesOff,checkPointPosition,Quaternion.identity)as GameObject;
	}
	
}
