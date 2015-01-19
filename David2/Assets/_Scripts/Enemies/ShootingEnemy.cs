using UnityEngine;
using System.Collections;

public class ShootingEnemy : basicEnemy {
	//Attack
	private float lastShot = 0f;
	public float reloadingTime;
	public GameObject EnemyBullet;

	override public void attackPlayer (){
		circleAroundPlayer ();
		if(lastShot < Time.time - reloadingTime){
			Instantiate(EnemyBullet, (transform.position - new Vector3(0,2.2f,0)), transform.rotation); 
			lastShot = Time.time;
		}
	}

}
