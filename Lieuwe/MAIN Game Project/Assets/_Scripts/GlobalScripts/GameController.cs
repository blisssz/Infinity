using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {
	public static bool fallingPossible;
	public static bool dead;
	private static int startLifes = 3;
	public static int lifes;

	public GameObject mainPlayer;
	
	public GameObject UI;
	public static Vector3 spawnLocation;
	public static Vector3 StartPosition;

	private bool mainPlayerAlive = false;
	private GameObject thePlayer;

	private KeyManager keyManager;		// manager for key presses
	
	private Vector3 playerPosition;

	// Use this for initialization
	void Start () {
		GameController.fallingPossible = true;
		spawnLocation=this.transform.position;   //StartPostion in Level
		keyManager = new KeyManager();
		lifes = startLifes;
		Instantiate(UI);
	}
	
	// Update is called once per frame
	void Update () {
		if(keyManager==null){keyManager = new KeyManager();}
		keyManager.Update();
		if (lifes <= 0) {
			lifes = startLifes;
			score.setToZero();
			spawnLocation = StartPosition;
			Application.LoadLevel (Application.loadedLevel);
		}

		// simple spawner
		if (mainPlayerAlive == false && mainPlayer != null){
			thePlayer = GameObject.Instantiate(mainPlayer, spawnLocation, Quaternion.identity) as GameObject;
			playerPosition = mainPlayer.transform.position;
			mainPlayerAlive = true;
			dead = false;
			healthBar.playerHealth = 100;
			GameObject.Find("Sniper Camera").GetComponent<Camera>().enabled = false; 
		}

		if (thePlayer.GetComponent<HPmanager>().hp <= 0){
			KillPlayer ();
		}

		if (thePlayer.transform.position.y <= -60 && fallingPossible) {
			KillPlayer ();
		}
	}

	public void KillPlayer(){
		thePlayer.GetComponent<PlayerManager>().DestroyWeapon();
		Destroy (thePlayer);
		mainPlayerAlive = false;
		dead = true;
		lifes -= 1;
	}




}
