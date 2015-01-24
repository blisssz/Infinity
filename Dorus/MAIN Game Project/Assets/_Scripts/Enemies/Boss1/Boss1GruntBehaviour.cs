using UnityEngine;
using System.Collections;

public class Boss1GruntBehaviour : MonoBehaviour {
	
	// Update is called once per frame
	void Start () {
		Destroy(this.gameObject,2);
	}
}
