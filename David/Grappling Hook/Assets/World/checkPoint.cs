using UnityEngine;
using System.Collections;

public class checkPoint : MonoBehaviour {
	private Vector3 checkPointPosition;
	// Use this for initialization
	void Start () {
		checkPointPosition = Vector3.one;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPosition (Vector3 pos){
		checkPointPosition = pos;
	}

	void OnTriggerEnter (Collider col){
		if (col.tag.Equals ("grapplingHook") || col.tag.Equals ("Player")) {
			GameController.spawnLocation = checkPointPosition;
		}
	}
}
