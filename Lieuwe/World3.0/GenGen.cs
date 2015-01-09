using UnityEngine;
using System.Collections;

public class GenGen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GenerateCases.GenCases2();
		GenerateCases.WriteToFile();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
