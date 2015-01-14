using UnityEngine;
using System.Collections;

public class Boss1HitPlayer : MonoBehaviour {

	public GameObject Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Player == null) {
			Player = GameObject.FindWithTag ("Player");
			if(Player !=null){transform.LookAt (Player.transform);}
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.tag.Equals("Player")){
			//OtherScript otherscript = col.GetComponent<PlayerManager>;
			GameObject Player=col.transform.root.gameObject;
			Player.GetComponent<HPmanager>().doDamage(20);
		}
	}
}
