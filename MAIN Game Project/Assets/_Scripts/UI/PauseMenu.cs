using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public static bool paused = false;
	
	public GameObject Pausemenu;
	public GameObject ColorPreview;
	private Image background;

	//public Image background; //currently not used

	// Use this for initialization
	void Start () {
		background = Pausemenu.GetComponent<Image> ();
		//background.CrossFadeAlpha(0f,0f,false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) == true) {
			togglePause ();
		}
	}

	public void togglePause(){
		if (PauseMenu.paused == false) {
			Screen.lockCursor = false;
			Screen.showCursor = true;
			paused = true;
			toggleGUI(Pausemenu, true);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<MouseLook>().enabled = false;
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>().enabled = false;
			Time.timeScale = 0.0f;
		} 
		else {
			Time.timeScale = 1f;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<MouseLook>().enabled = true;
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>().enabled = true;
			toggleGUI(Pausemenu, false);
			Screen.showCursor = false;
			Screen.lockCursor = true;
			paused = false;
			Debug.Log ("got here");
		}
	}

	public void goToNewLevel(){
		togglePause ();
		Application.LoadLevel ("Main Scene");
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
}
