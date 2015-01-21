using UnityEngine;
using System.Collections;

public class gravityBoss : MonoBehaviour {
	// Shooting
	private int noOfPieces = 20; 
	public shatteredGlass glass;
	private float[] glassOffset = new float[3];
	public float reloadTime;
	private float lastShot;
	private shatteredGlass[] glassPieces;

	// AI
	private static float[] smartGlassOffset = new float[3];
	public static float learningFactor;
	private static int smartCount;
	private static float[] total = new float[3];
	
	// Player
	private GameObject Player;
	private Vector3 toPlayer;

	// Status
	public int lifes;
	public GameObject weakPoint;
	private bool dead;
	public GameObject cannon;

	// Use this for initialization
	void Start () {
		shatteredGlass.boss = gameObject;
		glassPieces = new shatteredGlass[noOfPieces];
		for (int i = 0; i < 3; i++) {
			smartGlassOffset[i] = 0;		
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(Player == null){Player = GameObject.FindGameObjectWithTag ("Player");}
		if (Player == null){return;}
		else{transform.LookAt (Player.transform);}
		if(Time.time > lastShot + reloadTime){
			Shoot();
			lastShot = Time.time;
		}
	}

	// Shoots a cloud of glass pieces to the player
	void Shoot(){
		if(lastShot > 0){
			foreach (shatteredGlass piece in glassPieces) {
				if(piece != null){
					Destroy (piece);
				}
			}
			EvolutionaryStrategy ();
		}
		for(int i = 0; i<noOfPieces; i++){
			for(int j = 0; j<3; j++){
				glassOffset[j] = Random.value;
			}
			glassPieces[i] = Instantiate (glass, cannon.transform.position + new Vector3(glassOffset[0],glassOffset[1], glassOffset[2]), Quaternion.identity) as shatteredGlass;
		}
	}

	// If the boss gets hit by its own glasspieces, it reduces life
	public void applyDamage(float damage){
		lifes -= 1;
		if (lifes <= 0) { 
			Destroy (gameObject);
			GameObject Finish=ObjectSpawner.SpawnObjectWith(this.transform.position + new Vector3(0,0,0),"EndPoint");
			Finish.GetComponent<endPoint>().isBossLevel=true;
			if(!dead){
				score.gameScore += 150;
				dead = true;
			}

		}
	}

	private void EvolutionaryStrategy(){
		for(int i = 0; i < total.Length; i++) {
			total[i] *= learningFactor;
			smartGlassOffset[i] = learningFactor*total[i]/smartCount;
		}
		smartCount = 0;
	}


	public static void setSmartOffset(Vector3 updir){
		smartCount++;
		total = SuperSmartOffset.vecToRow (updir);
	}

	public static void setSmartOffset(Vector3 result, Vector3 desiredResult){
		smartCount++;
		smartGlassOffset = SuperSmartOffset.getSmartOffset (result,desiredResult);
	}

	public static float[] getSmartOffset(){
		return smartGlassOffset;
	}
}
