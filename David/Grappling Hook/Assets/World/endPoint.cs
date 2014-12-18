using UnityEngine;
using System.Collections;

public class endPoint : MonoBehaviour {
	private float speed = 0.5f;
	private Vector3 endPosition;
	public static float minDistance;
	private float distance;
	private float startDistance;
	private int maxScore = 200;
	// Use this for initialization
	void Start () {		
		endPosition = transform.position;
		startDistance = (PlayerManager.playerPosition - endPosition).magnitude;
		minDistance = startDistance;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (speed, speed, speed);
		distance = (PlayerManager.playerPosition - endPosition).magnitude;
		if (distance < minDistance){
			minDistance = distance;
		}
		score.inGameScore = (int)Mathf.Round(((startDistance - minDistance)/startDistance)*maxScore);
	}

	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Player")) {
			Application.LoadLevel("Generated");	
		}
	}
}
