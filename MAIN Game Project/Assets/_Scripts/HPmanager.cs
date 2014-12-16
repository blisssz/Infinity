using UnityEngine;
using System.Collections;

public class HPmanager : MonoBehaviour {

	public float hp = 100f;
	public float dmgModifier = 1.0f;

	public void doDamage(float dmg){
		hp -= dmg * dmgModifier;
	}

	public float getHP(){
		return hp;
	}

	public void setHP(float newHP){
		hp = newHP;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
