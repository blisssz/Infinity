using UnityEngine;
using System.Collections;

public class platformCounter : MonoBehaviour {
	private bool visited;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if (visited = false){
		if (col.tag.Equals ("Player")) {
			score.platformsVisited += 1;
			highScore.totalPlatformsVisited +=1;
			visited = true;
		}}
	}
}
