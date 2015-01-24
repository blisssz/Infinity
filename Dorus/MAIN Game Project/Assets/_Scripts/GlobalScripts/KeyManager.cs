using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This class manages key bindings used in the game;
 * For any key:
 * 0 = Key not pressed
 * 1 = Key is pressed
 * 2 = Key is held down
 * 3 = Key released 
 * 
 * GameController.cs makes this script run;
 */
public class KeyManager {

	// key order:
	// movement keys
	public static int forward {get; set;}
	public static int backward {get; set;}
	public static int left {get; set;}
	public static int right {get; set;}

	public static int jump {get; set;}
	public static int leftShift {get; set;}


	public static int key1 {get; set;}
	public static int key2 {get; set;}

	public static int keyQ {get; set;}
	public static int keyE {get; set;}

	public static int esc {get; set;}


	private static List<int> keyStatus; 
	private static List<string> keyCodes;



	// mouse keys (3 fixed keys)
	public static int leftMouse {get; set;}
	public static int rightMouse {get; set;}
	public static int middleMouse {get; set;}

	private static int[] mouseStatus = {0, 0, 0}; 



	//[HideInInspector]
	//public enum keyMode {Pressed = 1, Hold = 2, Released = 3}; (gives too long code in other code)

	// Initialization
	public KeyManager() {

		keyStatus = new List<int>();
		keyCodes =  new List<string>();

		// initial key press status
		forward = 0;
		backward = 0;
		right = 0;
		left = 0;
		jump = 0;
		leftShift = 0;
		key1 = 0;
		key2 = 0;
		esc = 0;

		// SUGGESTION: keys can be loaded from a desired key bindings file

		// add keys, must be in the right order:
		//keyStatus.Add (forward);
		keyCodes.Add("w");
		//keyStatus.Add (backward);
		keyCodes.Add("s");
		//keyStatus.Add (left);
		keyCodes.Add("a");
		//keyStatus.Add (right);
		keyCodes.Add("d");
		//keyStatus.Add (jump);
		keyCodes.Add("space");
		//keyStatus.Add (leftShift);
		keyCodes.Add("left shift");

		//keyStatus.Add(key1);
		keyCodes.Add ("1");
		//keyStatus.Add(key2);
		keyCodes.Add ("2");

		//keyStatus.Add(0);
		keyCodes.Add ("q");
		//keyStatus.Add(0);
		keyCodes.Add ("e");

		keyCodes.Add ("escape");

		//...add more keys if needed

		// assign 0 to all keyStatus
		for(int i = 0; i < keyCodes.Count; i++){
			keyStatus.Add (0);
		}

	}
	
	// Update is called once per frame
	public void Update () {

		for (int i = 0; i < keyCodes.Count; i++){
			if (keyStatus[i] == 0 && Input.GetKey(keyCodes[i]) ){
				keyStatus[i] = 1;
			}
			else if (Input.GetKey(keyCodes[i]) ){
				keyStatus[i] = 2;
			}
			else if ( ( (keyStatus[i] == 2) || (keyStatus[i] == 1) ) && !Input.GetKey(keyCodes[i]) ){
				keyStatus[i] = 3;
			}
			else{
				keyStatus[i] = 0;
			}
		}

		// assign keyStatus in the right order to the variable fields
		forward = keyStatus[0];
		backward = keyStatus[1];
		left = keyStatus[2];
		right = keyStatus[3];
		jump = keyStatus[4];
		leftShift = keyStatus[5];

		key1 = keyStatus[6];
		key2 = keyStatus[7];

		keyQ = keyStatus[8];
		keyE = keyStatus[9];

		esc = keyStatus[10];

		//...add more keys if needed



		// Mouse Button Input Status
		for (int i = 0; i < 3; i++){
			if (mouseStatus[i] == 0 && Input.GetMouseButton(i) ){
				mouseStatus[i] = 1;
			}
			else if (Input.GetMouseButton(i) ){
				mouseStatus[i] = 2;
			}
			else if ( ( (mouseStatus[i] == 2) || (mouseStatus[i] == 1) ) && !Input.GetMouseButton(i) ){
				mouseStatus[i] = 3;
			}
			else{
				mouseStatus[i] = 0;
			}
		}

		leftMouse = mouseStatus[0];
		rightMouse = mouseStatus[1];
		middleMouse = mouseStatus[2];




	}
}
