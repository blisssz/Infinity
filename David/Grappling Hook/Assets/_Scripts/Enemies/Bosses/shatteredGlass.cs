using UnityEngine;
using System.Collections;

public class shatteredGlass : MonoBehaviour {
	private Vector3 dir;
	private Vector3 bossToPlayer;
	private float[] offs = new float[3];
	public float glassSpeed;
	public GameObject[] otherpieces;
	public static GameObject boss;
	public static float[] playerOffset = new float[3];
	private float minDistance = Mathf.Infinity;
	private Vector3 minToPlayer;
	private Vector3 updatedDirection;
	private Vector3 toPlayer;
	private float[] smartOffset;

	// Use this for initialization
	void Start () {
		transform.LookAt (PlayerManager.playerPosition);
		smartOffset = gravityBoss.getSmartOffset ();
		//smartOffset[0] += 
		for(int i = 0; i < 3; i++){
			print (smartOffset[i]);
		}
		bossToPlayer = (PlayerManager.playerPosition - transform.position) + new Vector3 (smartOffset[0],smartOffset[1] -1f,smartOffset[2]);
		otherpieces = GameObject.FindGameObjectsWithTag ("projectile");
		rigidbody.velocity = bossToPlayer.normalized * glassSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		toPlayer = PlayerManager.playerPosition - transform.position;
		if(boss != null){
			Destroy(gameObject,5f);
		}
		if (toPlayer.magnitude < minDistance) {	missDirection ();}
	}

	void OnTriggerEnter(Collider col){
		if(col.tag.Equals ("Player")){
			healthBar.playerHealth -= 2;
		}
		if(col.tag.Equals("Enemy")){
			gravityBoss boss = col.transform.parent.gameObject.GetComponent<gravityBoss>();
			if(boss != null){
				boss.applyDamage(10f);
			}
		}
		if(col.tag.Equals ("Environment")){
			Destroy(gameObject);
		}
	}

	public void missDirection(){
		minDistance = toPlayer.magnitude;
		minToPlayer = toPlayer;
		updatedDirection = (transform.position - boss.transform.position) + minToPlayer;
	}

	void OnDestroy(){
		toPlayer = PlayerManager.playerPosition - transform.position;
		gravityBoss.setSmartOffset (minToPlayer, toPlayer);
	}

}
