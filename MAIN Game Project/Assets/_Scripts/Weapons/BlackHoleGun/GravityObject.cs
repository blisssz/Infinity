using UnityEngine;
using System.Collections;

//Add this script to all gameObjects with a rigidbody that should be affected by the gravity of black holes.
public class GravityObject : MonoBehaviour {
	
	void Start () {
		BlackHole.AddRigidbody (this);
	}

	void onDestroy () {
		BlackHole.DeleteRigidbody (this);
	}
}
