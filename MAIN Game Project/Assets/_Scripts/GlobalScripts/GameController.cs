using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {

	public GameObject mainPlayer;
	public Vector3 spawnLocation = new Vector3(0.0f, 2.0f, 0.0f);

	private bool mainPlayerAlive = false;
	private GameObject thePlayer;

	private KeyManager keyManager;		// manager for key presses


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
			mainPlayerAlive = true;
		}

		if (mainPlayerAlive == true && Input.GetKey (KeyCode.P) == true){
			Destroy (thePlayer);
			mainPlayerAlive = false;
		}
	}
}
