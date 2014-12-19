using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MagazinesLeft : MonoBehaviour {

	public static Text textValue;
	
	// Use this for initialization
	void Awake () {
		textValue = GetComponent<Text> ();
	}
}
