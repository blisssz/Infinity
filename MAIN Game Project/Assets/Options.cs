using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Options : MonoBehaviour {

	public void deleteHighScore(){
		PlayerPrefs.DeleteKey ("High Score");
	}
}

