using UnityEngine;
using System.Collections;

public class testAI : MonoBehaviour {


	private HPmanager hpManager;

	// Use this for initialization
	void Start () {
		if(!GetComponent<HPmanager>()){
			hpManager = this.gameObject.AddComponent<HPmanager>();
		}
		else{
			hpManager = this.gameObject.GetComponent<HPmanager>();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		//print(hpManager.getHP());
	
	}
}
