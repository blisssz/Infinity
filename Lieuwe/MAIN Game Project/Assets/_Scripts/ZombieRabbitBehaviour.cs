using UnityEngine;
using System.Collections;

public class ZombieRabbitBehaviour : MonoBehaviour {
	
	// tweak these values
	public float hp = 10f;
	public float dmg = 5f;
	public float attackSpeed = 1f;
	public float points = 10f;
	
	public float v_max;
	// attack stuff
	public float hitRange = 4f;
	public float stopDistance = 3f;
	
	private bool alive = true;
	
	private AgentS agentS;
	private Animator anim;
	public AudioClip snd_hit;
	public HPmanager hpmanager {get; set;}
	
	// anim stuff
	//public float animSpeed = 1f; // 2.63 for v = 10 m/s #### now based on velicoty
	private int attackHash;
	
	private int attackState = 0;
	
	private float distanceToGround;
	
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		agentS = GetComponent<AgentS>();
		
		agentS.stopDistance = stopDistance;
		agentS.cp.v_max = v_max;
		
		hpmanager = this.gameObject.GetComponent<HPmanager>();
		
		if (!hpmanager){
			hpmanager = this.gameObject.AddComponent<HPmanager>();
			hpmanager.hp = hp;
		}
		
		attackHash = Animator.StringToHash ("Base Layer.attacking");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (alive){
			bool playerAttention = agentS.targetsMainTarget;
			AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo (0);
//			bool attack = (astate.nameHash == attackHash);
			
			if (agentS.stopped == true){
				anim.SetFloat("Speed", 0f);
				
				if (agentS.distanceToTarget < hitRange){
					//play attack
					anim.SetBool("Attack",  true);
					if (attackState == 0){
						anim.speed = attackSpeed;
						attackState = 1;
					}
					if (astate.nameHash == attackHash && astate.normalizedTime > 0.6f && attackState == 1){
						// do damage
						doDamageToPlayer(dmg);
						playSound(snd_hit, 0.7f);
						attackState = 2;
					}
				}
				// reset
				if (astate.nameHash == attackHash && astate.normalizedTime > 0.9f){
					//attackState = 0;
				}
				if (astate.nameHash != attackHash){
					attackState = 0;
					anim.speed = 1f;
				}
				
			}
			else{
				anim.SetBool("Attack",  false);
				anim.SetFloat("Speed", 1f);
				
				Vector3 vel = rigidbody.velocity;
				vel.y = 0f;
				float velXY = vel.magnitude;
				float spd = velXY *0.263f;
				
				anim.speed = spd;
				attackState = 0;
			}
		}
		if (hpmanager.hp <= 0f){
			dead ();
		}
		
	}
	
	private void doDamageToPlayer(float dmg){
		GameObject gobj = GameObject.FindGameObjectWithTag("Player");
		if (gobj != null){
			gobj.GetComponent<HPmanager>().doDamage(dmg);
			CameraShake.shakeMainCamera();
		}
	}
	
	private void playSound(AudioClip ac, float vol = 1f){
		audio.PlayOneShot(ac, vol);
	}
	
	private void addScore(float s){
		
		GroundDamageEffects gde = this.gameObject.GetComponent<GroundDamageEffects>();
		
		if (gde != null && gde.groundStatus != GroundFXstatus.KilledBy){
			score.gameScore += (int)s;		
		}
	}
	
	private void dead(){
		if (alive){
			Destroy (agentS);			
			addScore(points);
			anim.SetBool ("isDeath",true);
			anim.speed = 1.5f;
			this.rigidbody.constraints = RigidbodyConstraints.None;
			this.rigidbody.constraints = RigidbodyConstraints.FreezePositionX |  RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			//this.rigidbody.constraints = RigidbodyConstraints.FreezePosition
			//this.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
			Destroy (this.gameObject, 3f);
		}
		
		alive = false;
		CapsuleCollider capsC = GetComponent<CapsuleCollider>();
		capsC.center = new Vector3(0, -0.58f, 0);
		capsC.height = 1f;
	}
}
