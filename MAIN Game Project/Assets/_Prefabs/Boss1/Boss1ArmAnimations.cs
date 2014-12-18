using UnityEngine;
using System.Collections;

public class Boss1ArmAnimations : MonoBehaviour {
	protected Animator animator;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//set to right co-ordinates
		//transform.localPosition = new Vector3(0.0f, -6.222f, 0.0f);

	if (Player == null) {
			Player = GameObject.FindWithTag ("Player");
		}
	
	if (animator) {
			//get the current state
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
			print (DistanceToObject (Player));

			if (DistanceToObject (Player) <18){
				animator.SetBool("attack",true);
			}
			else {
				animator.SetBool("attack",false);
			}
		}
	}

	float DistanceToObject(GameObject other) {
		return Vector3.Distance (this.transform.position, other.transform.position);
	}

	void OnTriggerEnter(Collider col){
		if(col.tag.Equals("Player")){
			//OtherScript otherscript = col.GetComponent<PlayerManager>;
			healthBar.playerHealth -= 10;
			print (healthBar.playerHealth);
		}
	}
}
