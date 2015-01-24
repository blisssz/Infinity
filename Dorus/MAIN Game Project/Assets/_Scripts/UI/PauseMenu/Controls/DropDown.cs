using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

public class DropDown : MonoBehaviour {

	public GameObject dropdown;

	public PanelSwitcher panelSwitcher;

	public bool mouseOnTitle;
	public bool mouseOnDropdown;
	public bool dropDownEnabled;


	public void mouseEnter(){
//		Debug.Log ("mouse enter");
		mouseOnTitle = true;
		dropdown.SetActive (true);
		dropDownEnabled = true;
	}

	public void mouseEnterDropdown(){
//		Debug.Log ("mouse enter dropdown");
		mouseOnDropdown = true;
	}

	public void mouseExitDropdown(){
//		Debug.Log ("dropdown exit");
		dropdown.SetActive (false);
		dropDownEnabled = false;
		mouseOnTitle = false;
		mouseOnDropdown = true;
	}

	private void OnDisable () {
		dropdown.SetActive (false);
	}
}
