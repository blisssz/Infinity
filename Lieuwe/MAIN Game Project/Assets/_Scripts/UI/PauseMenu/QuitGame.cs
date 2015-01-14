using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	public void quitGame(){
		Application.Quit ();
	}

	public void quitToMenu() {
		Time.timeScale = 1f;
		Application.LoadLevel ("StartMenu");
	}
}
