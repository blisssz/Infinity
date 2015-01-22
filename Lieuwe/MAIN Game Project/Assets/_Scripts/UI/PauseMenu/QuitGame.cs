using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	public void quitGame(){
		Application.Quit ();
	}

	public void quitToMenu() {
		Application.LoadLevel ("StartMenu");
	}

	public void startGame() {
		Application.LoadLevel ("Main Scene");
	}
}
