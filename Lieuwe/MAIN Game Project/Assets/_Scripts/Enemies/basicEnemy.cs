using UnityEngine;
using System.Collections;

public class basicEnemy : MonoBehaviour {
	//Moving around
	public float enemySpeed;
	public float chasingSpeed;
	private float perimeter;
	public float radius;
	private float rotationSpeed;
	private Vector3 enemyRotation;
	private GrapplingRopeSwing ropeSwing;
	private float animationStart;
	
	//Player info
	public GameObject Player;
	private Vector3 playerPosition;
	private Vector3 toPlayer;
	private float distanceToPlayer;
	private bool playerNear;
	
	//Enemy state
	private bool alarmed;
	public float sightLength;
	private bool enemyAlive;
	private Vector3 spawnPosition;
	public bool inAttack;
	
	//Attack
	public float attackingRadius;
	
	// Use this for initialization
	void Start () {
		enemyAlive = true;
		spawnPosition = transform.position;
		perimeter = Mathf.PI * 2 * radius;
		rotationSpeed = 360 / (perimeter / enemySpeed);
		animationStart = Random.value;
		animation ["Default Take"].time = animationStart;
		animation ["ArmatureAction_002"].time = animationStart;
	}
	
	// Update is called once per frame
	void Update () {
		if (Player == null) {
			Player = GameObject.FindWithTag ("Player");
		}
		if (Player == null) {return;}
		playerPosition = Player.transform.position;
		toPlayer = new Vector3((playerPosition.x - transform.position.x), 0f, playerPosition.z - transform.position.z);
		distanceToPlayer = toPlayer.magnitude;
		if (transform.GetComponent<HPmanager>().hp <= 0) {
			enemyAlive = false;	//STERF-
			score.gameScore += 20;
			score.enemiesKilled += 1;
		}
		if (!enemyAlive){
			gameObject.SetActive(false);
		}
		lookForPlayer ();
		if ( playerNear || inAttack == true){
			alarmed = true;
			attackPlayer();
		}
		else {
			//print (alarmed);
			if(alarmed == true){
				moveTowardsPlayer();
			}
			else{
				movingCircle();
			}
			if(transform.position.y < spawnPosition.y - 0.2f){
				transform.Translate(0, enemySpeed * Time.deltaTime, 0);
			}
			if(transform.position.y > spawnPosition.y + 0.2f){
				transform.Translate(0, -enemySpeed * Time.deltaTime, 0);
			}
		}
		transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
	}
	
	void movingCircle(){
		transform.Rotate (0,rotationSpeed * Time.deltaTime,0, Space.Self);
		transform.Translate (transform.forward * enemySpeed * Time.deltaTime, Space.World);
	}
	
	public void circleAroundPlayer(){
		transform.LookAt (Player.transform);
		transform.Translate(Vector3.right * enemySpeed * Time.deltaTime, Space.Self);
		if(distanceToPlayer > (attackingRadius + 0.5)){
			transform.Translate(toPlayer * enemySpeed * Time.deltaTime, Space.World); 
		}
		if(distanceToPlayer < (attackingRadius - 0.5)){
			transform.Translate(toPlayer * (-1f) * enemySpeed * Time.deltaTime, Space.World); 
		}
		heightCheck ();
	}
	
	public void heightCheck(){
		if (transform.position.y < playerPosition.y + 2.8f) {
			transform.Translate(0, 2 * enemySpeed*Time.deltaTime,0);		
		}
		if (transform.position.y > playerPosition.y + 3.2f) {
			transform.Translate(0, 2 * -Time.deltaTime*enemySpeed,0);		
		}
	}
	
	void moveTowardsPlayer(){
		transform.LookAt (Player.transform);
		transform.Translate (transform.forward * chasingSpeed * Time.deltaTime, Space.World);
		heightCheck ();
	}
	
	void lookForPlayer(){
		if(distanceToPlayer <= sightLength) {
			playerNear = true;
			alarmed = true;
		}
		else { playerNear = false; }
		if(distanceToPlayer > 20) {
			playerNear = false;
			alarmed = false;
		}
	}	
	
	//void attackPlayer (){
	//	circleAroundPlayer ();
	//}
	
	void OnTriggerEnter(Collider col){
		if (col.tag.Equals("Environment")){
			transform.Rotate (0,180,0);
		}
		if (col.tag.Equals ("Player")) {
			afterTrigger(col.tag);	
		}
	}
	
	//	void respawn(){
	//		this.gameObject.SetActive(true);
	//		enemyAlive = true;
	//		transform.position = spawnPosition;
	//		alarmed = false;
	//	}
	
	public void hookHit(){
		applyDamage (20f);
	}
	
	public void applyDamage(float damage){
		transform.GetComponent<HPmanager> ().doDamage (damage);
		rigidbody.AddForce ((-30 * toPlayer.normalized), ForceMode.VelocityChange);
		alarmed = true;
		//score.gameScore += 5;
	}
	
	void OnDestroy(){
		if(transform.GetComponent<HPmanager> ().hp<=0){
		highScore.enemyKill ();
		score.gameScore += 5; 
		}
	}
	
	virtual public void attackPlayer(){
	}
	
	virtual public void afterTrigger(string tag){
	}
}