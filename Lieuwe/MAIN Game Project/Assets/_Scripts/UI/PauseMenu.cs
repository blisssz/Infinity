using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour {

	public static bool paused = false;
	private static int pauseState = 1; //1 main pause menu, 2 options, 3 video, 4 audio, 5 controls, 6 customization etc...
	
	public AudioClip soundClick;

	public GameObject Pausemenu;
	public GameObject Main;
	public GameObject Options;
	public GameObject Video;
	public GameObject Audio;
	public GameObject Controls;
	public GameObject Customization;
	public GameObject ColorPreview;
	
	private Dictionary<int, GameObject> pauseScreens;
	
	// Use this for initialization
	void Start () {
		pauseScreens = new Dictionary<int, GameObject>();
		pauseScreens.Add (1, Main);
		pauseScreens.Add (2, Options);
		pauseScreens.Add (3, Video);
		pauseScreens.Add (4, Audio);
		pauseScreens.Add (5, Controls);
		pauseScreens.Add (6, Customization);
		//etc
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) == true) {
			switch(pauseState){
				case 1:
					togglePause ();
					break;
				case 2:
					//go back to main pause menu
					switchPauseState(1);
					break;
				case 3:
				case 4:
				case 5:
				case 6:
					//go back to options menu
					switchPauseState(2);
					break;
				default:
					Debug.Log ("Something went wrong or went missing");
					break;
			}
		}
	}

	public void togglePause(){
		if (PauseMenu.paused == false) {
			//Pause the game
			Screen.lockCursor = false;
			Screen.showCursor = true;
			paused = true;
			toggleGUI(Pausemenu, true);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<MouseLook>().enabled = false;
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>().enabled = false;
			Time.timeScale = 0.0f;
		} 
		else {
			//Unpause the game
			unPause ();
			Screen.showCursor = false;
			Screen.lockCursor = true;
		}
	}

	public void unPause(){
		//Unpause the game
		playAudio (soundClick, 0.5f);
		Time.timeScale = 1f;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<MouseLook>().enabled = true;
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>().enabled = true;
		toggleGUI(Pausemenu, false);
		paused = false;
	}

	/**
	 * A function for toggling a specific gameObject on or off
	 */
	public void toggleGUI(GameObject item, bool switcher){
		item.SetActive(switcher);
	}

	/**
	 * A function for changing only the alpha value of the current color of the color preview square
	 * 
	 * Note, due to limitations of the function calls of UI elements we can't easily combine these 
	 * color related functions with the same functions from the Crosshair.cs script.
	 */
	public void setTransparency(float transparency){
		Color color = ColorPreview.transform.GetComponent<Image>().color;
		color.a = transparency;
		ColorPreview.transform.GetComponent<Image>().color = color;
	}

	/**
	 * A function for changing only the red value of the current color of the color preview square
	 */
	public void setRed(float red){
		Color color = ColorPreview.transform.GetComponent<Image>().color;
		color.r = red;
		ColorPreview.transform.GetComponent<Image>().color = color;
	}

	/**
	 * A function for changing only the green value of the current color of the color preview square
	 */
	public void setGreen(float green){
		Color color = ColorPreview.transform.GetComponent<Image>().color;
		color.g = green;
		ColorPreview.transform.GetComponent<Image>().color = color;
	}

	/**
	 * A function for changing only the blue value of the current color of the color preview square
	 */
	public void setBlue(float blue){
		Color color = ColorPreview.transform.GetComponent<Image>().color;
		color.b = blue;
		ColorPreview.transform.GetComponent<Image>().color = color;
	}

	public void switchPauseState(int state){
		playAudio (soundClick, 0.5f);
		int previousState = pauseState;
		pauseState = state;
		pauseScreens [previousState].SetActive (false);
		pauseScreens [state].SetActive (true);
	}

	public void newLevel(){
		unPause ();
		Screen.showCursor = false;
		Screen.lockCursor = true;
		Application.LoadLevel ("Main Scene");
	}

	public void quitToMenu() {
		unPause ();
		Application.LoadLevel ("StartMenu");
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
