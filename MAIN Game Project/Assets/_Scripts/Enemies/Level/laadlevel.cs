using UnityEngine;
using System.Collections;

public class laadlevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (startLoading ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator startLoading(){
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel ("Generated");
	}
}
