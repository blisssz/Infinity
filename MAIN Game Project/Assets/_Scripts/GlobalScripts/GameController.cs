using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {
	public static bool fallingPossible;
	public static bool dead;
	private static int startLifes = 3;
	public static int lifes;

	public GameObject mainPlayer;
	public static Vector3 spawnLocation = new Vector3(0.0f, 2.0f, 0.0f);

	private bool mainPlayerAlive = false;
	private GameObject thePlayer;

	private KeyManager keyManager;		// manager for key presses
	
	private Vector3 playerPosition;

	// Use this for initialization
	void Start () {
		keyManager = new KeyManager();
		lifes = startLifes;
	}
	
	// Update is called once per frame
	void Update () {
		keyManager.Update();
		if (lifes <= 0) {
			lifes = startLifes;
			score.setToZero();
			spawnLocation = new Vector3(0,2,0);
			Application.LoadLevel (Application.loadedLevel);
		}

		// simple spawner
		if (mainPlayerAlive == false && mainPlayer != null){
			thePlayer = GameObject.Instantiate(mainPlayer, spawnLocation, Quaternion.identity) as GameObject;
			playerPosition = mainPlayer.transform.position;
			mainPlayerAlive = true;
			dead = false;
			healthBar.playerHealth = 100;
		}

		if (mainPlayerAlive == true && Input.GetKey (KeyCode.P) == true){
			Destroy (thePlayer);
			mainPlayerAlive = false;
			dead = true;
			lifes -= 1;
			print (lifes);
		}
		if (thePlayer.GetComponent<HPmanager>().hp <= 0){
			Destroy (thePlayer);
			mainPlayerAlive = false;
			dead = true;
			lifes -= 1;
			print (lifes);
		}

		if (thePlayer.transform.position.y <= -30 && fallingPossible) {
			Destroy (thePlayer);
			mainPlayerAlive = false;
			dead = true;
			lifes -= 1;
			print (lifes);
		}
	}

	public static void checkPoint(Vector3 pos){
		spawnLocation = pos;
		lifes = startLifes;
	}



}
