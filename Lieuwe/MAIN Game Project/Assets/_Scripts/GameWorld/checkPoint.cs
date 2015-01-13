using UnityEngine;
using System.Collections;

public class checkPoint : MonoBehaviour {
	private Vector3 checkPointPosition;
	private Vector3 playerPosition;
	// Use this for initialization
	void Start () {
		checkPointPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		playerPosition = PlayerManager.playerPosition;
		if((playerPosition - checkPointPosition).magnitude < 2){
			GameController.spawnLocation = checkPointPosition;
			Destroy (gameObject);
		}
		//print ((playerPosition - checkPointPosition).magnitude);
	}

	public void setPosition (Vector3 pos){
		checkPointPosition = pos;
	}
}
