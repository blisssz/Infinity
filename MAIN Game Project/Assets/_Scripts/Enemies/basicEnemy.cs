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

	//Player info
	public GameObject Player;
	private Vector3 playerPosition;
	private Vector3 toPlayer;
	private float distanceToPlayer;

	//Enemy state
	public float noOfLifes;
	private float enemyLife;
	private bool alarmed;
	public float sightLength;
	private bool enemyAlive;
	private Vector3 spawnPosition;
	public bool inAttack;
	
	//Attack
	public float attackingRadius;
	
	// Use this for initialization
	void Start () {
		noOfLifes = 2;
		enemyLife = noOfLifes;
		enemyAlive = true;
		spawnPosition = transform.position;
		perimeter = Mathf.PI * 2 * radius;
		rotationSpeed = 360 / (perimeter / enemySpeed);
	}
	
	// Update is called once per frame
	void Update () {
		if (Player == null) {
			Player = GameObject.FindWithTag ("Player");
		}
		if (score.restart){
			//respawn();
		}
		playerPosition = Player.transform.position;
		toPlayer = new Vector3((playerPosition.x - transform.position.x), 0f, playerPosition.z - transform.position.z);
		distanceToPlayer = toPlayer.magnitude;
		if (enemyLife <= 0) {
			enemyAlive = false;	//STERF-
			score.gameScore += 20;
			score.enemiesKilled += 1;
			highScore.totalEnemiesKilled += 1;
		}
		if (!enemyAlive){
			gameObject.SetActive(false);
		}
		if (distanceToPlayer <= sightLength || inAttack == true){
			attackPlayer();
		}
		else {
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
				transform.Translate(0, enemySpeed * Time.deltaTime, 0);
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
		if (transform.position.y < playerPosition.y + 1.8f) {
			transform.Translate(0,enemySpeed*Time.deltaTime,0);		
		}
		if (transform.position.y > playerPosition.y + 2.2f) {
			transform.Translate(0,-Time.deltaTime*enemySpeed,0);		
		}
	}
	
	void moveTowardsPlayer(){
		transform.Translate (toPlayer.normalized * chasingSpeed * Time.deltaTime, Space.World);
		heightCheck ();
	}
	
	void lookForPlayer(){
		transform.Translate (Vector3.forward, Space.Self);
		transform.LookAt (Player.transform);
		Ray sight = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		if(Physics.Raycast(sight, sightLength)){
			if(Physics.Raycast(sight, out hit)){
				if(hit.collider == Player){
					alarmed = true;
				}	}	}
	}	
	
	//void attackPlayer (){
	//	circleAroundPlayer ();
	//}
	
	void OnTriggerEnter(Collider col){
		if (col.tag.Equals("Environment")){
			enemySpeed *= 1;
			transform.Rotate (0,180,0);
		}
		if (col.tag.Equals ("Player")) {
			afterTrigger(col.tag);	
		}
	}
	
	void respawn(){
		this.gameObject.SetActive(true);
		enemyAlive = true;
		transform.position = spawnPosition;
		alarmed = false;
	}

	public void hookHit(){
		rigidbody.AddForce ((-30 * toPlayer.normalized), ForceMode.VelocityChange);
		applyDamage (20f);
		alarmed = true;
		score.gameScore += 5;
	}

	public void applyDamage(float damage){
		enemyLife -= damage;
	}

	virtual public void attackPlayer(){
	}

	virtual public void afterTrigger(string tag){
	}
}
