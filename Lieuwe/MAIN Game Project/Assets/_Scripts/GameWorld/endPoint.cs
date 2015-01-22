using UnityEngine;
using System.Collections;

public class endPoint : MonoBehaviour {
	private float speed = 0.5f;
	public bool isBossLevel;
	private Vector3 endPosition;
//	public static float minDistance;
//	private float distance;
//	private float startDistance;
//	private int maxScore = 200;
//	private Vector3 playerPosition;

	// Use this for initialization
	void Start () {		
		endPosition = transform.position;
//		startDistance = (PlayerManager.playerPosition - endPosition).magnitude;
//		minDistance = startDistance;
	}
	
	// Update is called once per frame


	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Player")&&isBossLevel==false) {
			OnTriggered();
			GameStart.loadBoss ();	
		}
		if(col.tag.Equals ("projectile")&&isBossLevel==false){
			OnTriggered();
			GameStart.loadBoss ();
		}
		if (col.tag.Equals ("Player")&&isBossLevel==true) {
			OnTriggered();
			GameStart.loadNormal ();	
		}
		if(col.tag.Equals ("projectile")&&isBossLevel==true){
			OnTriggered();
			GameStart.loadNormal ();
		}
	}

	private static void OnTriggered(){
		GameController.AddLifes(3);
		score.scoreUp(100);
	}
}
