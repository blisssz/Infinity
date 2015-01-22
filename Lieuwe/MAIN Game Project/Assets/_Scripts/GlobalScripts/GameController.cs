using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {
	public static bool fallingPossible;
	public static bool dead;
	private static int startLifes = 10;
	public static int lifes;

	public GameObject mainPlayer;
	
	public GameObject UI;
	public static Vector3 spawnLocation;
	public static Vector3 StartPosition;
	public GameObject UIfixed;
	public static GameObject Crosshair;

	private bool mainPlayerAlive = false;
	private GameObject thePlayer;

	private KeyManager keyManager;		// manager for key presses
	
	private Vector3 playerPosition;

	// Use this for initialization
	void Start () {

		GameController.fallingPossible = true;
		AudioList.StartX ();
		spawnLocation=this.transform.position;   //StartPostion in Level
		keyManager = new KeyManager();
		Instantiate(UI);
		Crosshair = Instantiate(UIfixed) as GameObject;
		lifes=LivesScript.lifes;
		Jetpack.reset();
	}
	
	// Update is called once per frame
	void Update () {
		lifes=LivesScript.lifes;
		if(keyManager==null){keyManager = new KeyManager();}
		keyManager.Update();
		if (lifes <= 0) {
			ResetAll();
			score.setToZero();
			spawnLocation = StartPosition;
			Screen.showCursor=true;
			if(!Application.loadedLevel.Equals ("Credits")){
				Application.LoadLevel ("Credits");
				LivesScript.lifes=10;

			}
		}

		// simple spawner
		if (mainPlayerAlive == false && mainPlayer != null){
			thePlayer = GameObject.Instantiate(mainPlayer, spawnLocation, Quaternion.identity) as GameObject;
			thePlayer.GetComponent<PlayerManager>().Crosshair=UIfixed;
			playerPosition = mainPlayer.transform.position;
			mainPlayerAlive = true;
			dead = false;
			healthBar.playerHealth = 100;
			GameObject.Find("Sniper Camera").GetComponent<Camera>().enabled = false; 
		}

		if (thePlayer.GetComponent<HPmanager>().hp <= 0){
			KillPlayer ();
		}

		if (thePlayer.transform.position.y <= -20 && fallingPossible) {
			KillPlayer ();
		}
	}

	public void KillPlayer(){
		Jetpack.reset();
		thePlayer.GetComponent<PlayerManager>().DestroyWeapon();
		Destroy (thePlayer);
		mainPlayerAlive = false;
		dead = true;
		lifes -= 1;
		LivesScript.SetLives (lifes);
		Vignetting.FadeTheAlpha(100f);

	}

	public static void ResetAll(){
		checkPointList.Reset();
		ChunkList.Reset ();
		PointList.Reset();
			
	}

	public static void AddLifes(int number){
		lifes+=number;
		LivesScript.SetLives (lifes);
	}




}
