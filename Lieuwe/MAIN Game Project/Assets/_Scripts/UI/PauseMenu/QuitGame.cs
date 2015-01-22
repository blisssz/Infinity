using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	public void quitGame(){
		Application.Quit ();
	}

	public void quitToMenu() {
		GameController.ResetAll();
		Application.LoadLevel ("StartMenu");
	}

	public void startGame() {
		GameController.ResetAll();
		Application.LoadLevel ("Main Scene");
	}
}
