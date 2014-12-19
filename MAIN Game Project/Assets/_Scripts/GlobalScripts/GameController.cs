using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {
	public static bool fallingPossible;
	public static bool dead;

	public GameObject mainPlayer;
	public Vector3 spawnLocation = new Vector3(0.0f, 2.0f, 0.0f);

	private bool mainPlayerAlive = false;
	private GameObject thePlayer;

	private KeyManager keyManager;		// manager for key presses
	
	private Vector3 playerPosition;

	// Use this for initialization
	void Start () {
		keyManager = new KeyManager();
	}
	
	// Update is called once per frame
	void Update () {
		keyManager.Update();

		// simple spawner
		if (mainPlayerAlive == false && mainPlayer != null){
			thePlayer = GameObject.Instantiate(mainPlayer, spawnLocation, Quaternion.identity) as GameObject;
			playerPosition = mainPlayer.transform.position;
			mainPlayerAlive = true;
			dead = false;
			healthBar.playerHealth = 100;
			score.setToZero();
		}

		if (mainPlayerAlive == true && Input.GetKey (KeyCode.P) == true){
			Destroy (thePlayer);
			mainPlayerAlive = false;
			dead = true;
		}
		if (thePlayer.GetComponent<HPmanager>().hp <= 0){
			Destroy (thePlayer);
			mainPlayerAlive = false;
			dead = true;
		}

		if (thePlayer.transform.position.y <= -50 && fallingPossible) {
			Destroy (thePlayer);
			mainPlayerAlive = false;
			dead = true;
			score.restart = true;
		}
	}




}
