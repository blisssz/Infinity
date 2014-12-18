using UnityEngine;
using System.Collections;

public class Enemy : basicEnemy {
	//Attack
	public float attackTime;
	private float lastAttack;
	public float attackSpeed;
	private float attackX;
	private float attackY;
	private float bdx = 1f;
	private float dx;
	private float dy;
	public GameObject henk;

	override public void attackPlayer(){
		Debug.Log (lastAttack);
		if(lastAttack < Time.time - attackTime){
			inAttack = true;
			paraboleFlight ();
		}
		else{
			circleAroundPlayer();
		}
		if(transform.position.y < Player.transform.position.y){
			afterTrigger ("low");
		}
	}

	override public void afterTrigger(string tag){
		inAttack = false;
		lastAttack = Time.time;
		attackX = 0;
		attackY = 0;
		if (tag.Equals("Player")){
			healthBar.playerHealth -= 20;
		}
	}

	void paraboleFlight(){
		transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
		dx = bdx * Time.deltaTime * attackSpeed;
		dy = parabole (attackX, dx);
		attackX += dx;
		attackY += dy;
		transform.Translate (0, dy, dx, Space.Self);
	}

	float parabole(float x, float dx){
		float afwijking = -1.5f;
		return 0.25f*(-Mathf.Pow ((x + dx + afwijking),2) + x + dx + afwijking + Mathf.Pow ((x + afwijking),2) - x - afwijking);
	}
}

