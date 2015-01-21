using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public AudioSource Shooting;

	// Use this for initialization
	void Start () {
		Vector3 C=this.transform.position;
		TextCreator A=new TextCreator(this.transform.position);
		A.CreateText("Wat een kutidee;" +
		             "adfasdfasdfadfaf;" +
		             "adsfadf;");

		AudioList.StartX();
		AudioList.Add(Shooting, false);
	}

}
