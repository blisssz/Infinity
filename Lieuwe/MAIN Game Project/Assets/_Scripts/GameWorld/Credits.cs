using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {


	// Use this for initialization
	void Start () {
		TextCreator A=new TextCreator(this.transform.position - new Vector3 (0, 50, 0));
		A.CreateText("Infinity inc presents;" +
		             "  INFINITY;" +
		             ";" + 
		             "In coorporation with;" +
		             "  TU Delft;" +
		             "  Minorproject software ontwerpen en toepassen;" +
		             ";" +
		             "Lead Artist;" +
		             "  Dorus van den Oord;" +
		             ";" +
		             "Lead Programmer; " +
		             "  Erik Veldhuis;" + 
		             ";" +
		             "Game Designer;" +
		             "  Daan Picavet;" +
		             ";" +
		             "World Builder;" +
		             "  Lieuwe Locht;" +
		             ";" +
		             "Producer;" +
		             "  David Akkerman;" +
		             ";" +
		             "Special Thanks;" +
		             "  Bas Dado;"
		             );

	}

	IEnumerator tijd() {
		yield return new WaitForSeconds(30);
		Application.LoadLevel ("Main Scene");
	}
}
