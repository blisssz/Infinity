using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuPauseScreen : MonoBehaviour {
	
	public static bool paused = false;
	private static int pauseState = 1; //1 main pause menu, 2 options, 3 video, 4 audio, 5 controls, 6 login etc...
	
	public AudioClip soundClick;

	public GameObject Main;
	public GameObject Options;
	public GameObject Video;
	public GameObject Audio;
	public GameObject Controls;
	public GameObject Login;
	
	private Dictionary<int, GameObject> pauseScreens;
	
	// Use this for initialization
	void Start () {
		pauseScreens = new Dictionary<int, GameObject>();
		pauseScreens.Add (1, Main);
		pauseScreens.Add (2, Options);
		pauseScreens.Add (3, Video);
		pauseScreens.Add (4, Audio);
		pauseScreens.Add (5, Controls);
		pauseScreens.Add (6, Login);
		//etc
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) == true) {
			switch(pauseState){
			case 1:
				break;
			case 2:
				//go back to main menu
				switchPauseState(1);
				break;
			case 3:
			case 4:
			case 5:
				//go back to options menu
				switchPauseState(2);
				break;
			case 6:
				//go back to main menu
				switchPauseState(1);
				break;
			default:
				Debug.Log ("Something went wrong or went missing");
				break;
			}
		}
		Time.timeScale = 1f;
	}

	public void switchPauseState(int state){
		playAudio (soundClick, 0.5f);
		int previousState = pauseState;
		pauseState = state;
		pauseScreens [previousState].SetActive (false);
		pauseScreens [state].SetActive (true);
	}
	
	private void playAudio (AudioClip clip, float volume){
		Time.timeScale = 1f;
		AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0), volume);
		Time.timeScale = 0f;
	}
	
	public void playClickAudio(){
		playAudio (soundClick, 0.5f);
	}
}
